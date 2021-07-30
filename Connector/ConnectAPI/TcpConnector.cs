using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shared.Types;
using Shared.Types.ConnectAPI;

namespace Connector.ConnectAPI
{
    public class TcpConnector
    {
        private const int ApiPort = 10112;

        private IfInstanceInformation _discoveredInstance;
        public bool CanUpdateFlightState => _discoveredInstance.State == "Playing";
        
        private UdpReceiver _udpReceiver = new ();
        private TcpClient _tcp = new ();
        private NetworkStream _stream;
        
        private List<ManifestEntry> _manifest;
        
        public static event EventHandler ManifestReceived = delegate { };
        public static event EventHandler PlaneStateReceived = delegate { };
        
        public static List<PlaneStateEntry> PlaneStateEntries { get; set; } = new ();
        private Dictionary<int, ManifestEntry> ManifestEntryByID { get; set; } = new ();
        private Dictionary<int, PlaneStateEntry> PlaneStateEntryByID { get; set; } = new ();
        
        private object _tcpLock = new ();
        private CancellationTokenSource _streamReaderToken = new ();
        private CancellationTokenSource _stateRefresherToken = new ();
        
        public void StartPlaneStateRefresher() => StartAction(PlaneStateRefresher);
        public void StopPlaneStateRefresher() => StopAction(_stateRefresherToken);

        public IfInstanceInformation DiscoverRunningInstances()
        {
            var response = _udpReceiver.TryFindRunningInstances();
            _discoveredInstance = response;
            
            return response;
        }

        public ConnectionAttemptResult TryConnectToTcpServer()
        {
            if (_discoveredInstance == null) return ConnectionAttemptResult.NoInstanceInformation;
            if (_discoveredInstance.IpAddressesArray.Length <= 0) return ConnectionAttemptResult.NoInstanceInformation;

            try
            {
                _tcp.Connect(_discoveredInstance.IpAddressesArray, ApiPort);
            }
            catch
            {
                return ConnectionAttemptResult.Fail;
            }

            if (!_tcp.Connected) return ConnectionAttemptResult.Fail;
            
            _stream = _tcp.GetStream();
            _tcp.NoDelay = true;
            Connected();

            return ConnectionAttemptResult.Success;
        }
        
        private void RefreshNecessaryPlaneStates()
        {
            if (_manifest == null) return;
            
            // Console.WriteLine("refresh");
            
            foreach (var item in _manifest)
            {
                // only request those we need
                
                    //Console.WriteLine($"requesting #{item.ID} of type {item.Type} with path {item.Path}");
                    SendGetStateCommand(item.ID);
                
            }
        }
        
        private void Connected()
        {
            StartAction(StreamReader);
            
            // get the manifest
            SendGetStateCommand(-1);
        }

        private void StartAction(Action action)
        {
            Task.Run(action);
        }

        private void StreamReader()
        {
            while (!_streamReaderToken.Token.IsCancellationRequested)
            {
                // Console.WriteLine($"Read with {_tcp.Available} bytes in stream.");

                try
                {
                    ReadCommandInStream();
                }
                catch
                {
                    // ignored
                }
            }
        }
        
        private void PlaneStateRefresher()
        {
            while (!_stateRefresherToken.Token.IsCancellationRequested)
            {
                try
                {
                    RefreshNecessaryPlaneStates();
                    
                    // only do it every 100 ms
                    Thread.Sleep(500);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void StopAction(CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
        }
        
        private void SendGetStateCommand(int id)
        {
            lock (_tcpLock)
            {
                SendInt(id);
                SendBoolean(false);
            }
        }
        
        private void ReadCommandInStream()
        {
            if (_stream == null) return;
            if (_tcp.Available <= 0 || !_stream.DataAvailable || !_stream.CanRead) return;

            //Console.WriteLine("read comm. avail: " + _tcp.Available);

            var commandID = ReadInt();
            var dataLength = ReadInt();
            
            //Console.WriteLine("read comm 2. commId = " + commandID);
            
            if (commandID == -1) 
            {
                ReadManifest();                
            }
            else
            {
                var stateInfo = ManifestEntryByID[commandID];
                var state = PlaneStateEntryByID[commandID];

                //if (stateInfo.Path.Contains("joystick")) return;
                
                //Console.WriteLine("read comm 3. statePath = " + stateInfo.Path);
                
                if (stateInfo.Type == typeof(bool))
                {
                    var value = ReadBoolean(); // only double for now
                    Console.WriteLine("{0}: {1}", stateInfo.Path, value);
                    state.Value = value;
                }
                else if (stateInfo.Type == typeof(int))
                {
                    var value = ReadInt(); // only double for now
                    Console.WriteLine("{0}: {1}", stateInfo.Path, value);
                    state.Value = value;
                }
                else if (stateInfo.Type == typeof(float))
                {
                    var value = ReadFloat(); // only double for now
                    Console.WriteLine("{0}: {1}", stateInfo.Path, value);
                    state.Value = value;
                }
                else if (stateInfo.Type == typeof(double))
                {
                    var value = ReadDouble(); // only double for now
                    Console.WriteLine("{0}: {1}", stateInfo.Path, value);
                    state.Value = value;
                }
                else if (stateInfo.Type == typeof(string))
                {
                    var value = ReadString(); // only double for now
                    Console.WriteLine("{0}: {1}", stateInfo.Path, value);
                    state.Value = value;
                }
                else if (stateInfo.Type == typeof(long))
                {
                    var value = ReadLong(); // only double for now
                    state.Value = value;
                }

                PlaneStateReceived(this, EventArgs.Empty);
            }
            
            //Console.WriteLine("read comm end");
        }

        private void ReadManifest()
        {
            Console.WriteLine("Reading Manifest...");
            var str = ReadString();

            var lines = str.Split('\n');

            Console.WriteLine("States: {0}", lines.Length);

            _manifest = new List<ManifestEntry>();

            for (int i = 0; i < lines.Length; i++)
            {
                var items = lines[i].Split(',');

                if (items.Length == 3)
                {
                    var stateId = Int32.Parse(items[0]);

                    _manifest.Add(new ManifestEntry { ID = stateId, Type = GetTypeFromIndex(Int32.Parse(items[1])), Path = items[2] });
                    ManifestEntryByID[stateId] = _manifest[i];

                    var stateData = new PlaneStateEntry { Path = items[2], Id = stateId, Value = string.Empty };
                    PlaneStateEntries.Add(stateData);
                    PlaneStateEntryByID[stateId] = stateData;
                }
            }

            ManifestReceived(this, EventArgs.Empty);
            
            //RefreshNecessaryPlaneStates();
        }
        
        public static short GetTypeIndex(Type type)
        {
            if (type == typeof(bool))
            {
                return 0;
            }
            else if (type == typeof(int))
            {
                return 1;
            }
            else if (type == typeof(float))
            {
                return 2;
            }
            else if (type == typeof(double))
            {
                return 3;
            }
            else if (type == typeof(string))
            {
                return 4;
            }
            else if (type == typeof(long))
            {
                return 5;
            }

            return -1;
        }

        public static Type GetTypeFromIndex(int index)
        {
            if (index == 0)
                return typeof(bool);
            if (index == 1)
                return typeof(int);
            if (index == 2)
                return typeof(float);
            if (index == 3)
                return typeof(double);
            if (index == 4)
                return typeof(string);
            if (index == 5)
                return typeof(long);

            return null;
        }
        
        private void SendInt(int v)
        {
            if (_stream == null) return;
            
            var data = BitConverter.GetBytes(v);
            _stream.Write(data, 0, 4);
        }

        private void SendBoolean(bool v)
        {
            if (_stream == null) return;

            var data = BitConverter.GetBytes(v);
            _stream.Write(data, 0, 1);
        }

        private void SendString(string v)
        {
            if (_stream == null) return;

            var data = Encoding.UTF8.GetBytes(v);
            SendInt(data.Length);
            _stream.Write(data, 0, data.Length);
        }

        private void SendFloat(float v)
        {
            if (_stream == null) return;

            var data = BitConverter.GetBytes(v);
            _stream.Write(data, 0, 4);
        }

        private void SendDouble(double v)
        {
            if (_stream == null) return;

            var data = BitConverter.GetBytes(v);
            _stream.Write(data, 0, 8);
        }

        private void SendLong(long v)
        {
            if (_stream == null) return;

            var data = BitConverter.GetBytes(v);
            _stream.Write(data, 0, 8);
        }
        
        private Int32 ReadInt()
        {
            if (_stream == null) return -1;

            byte[] data = new byte[4];
            _stream.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }

        private double ReadDouble()
        {
            if (_stream == null) return -1;

            byte[] data = new byte[8];
            _stream.Read(data, 0, 8);
            return BitConverter.ToDouble(data, 0);
        }

        private float ReadFloat()
        {
            if (_stream == null) return -1f;

            byte[] data = new byte[4];
            _stream.Read(data, 0, 4);
            return BitConverter.ToSingle(data, 0);
        }

        private long ReadLong()
        {
            if (_stream == null) return -1L;

            byte[] data = new byte[8];
            _stream.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }

        private bool ReadBoolean()
        {
            if (_stream == null) return false;

            byte[] data = new byte[1];
            _stream.Read(data, 0, 1);
            return BitConverter.ToBoolean(data, 0);
        }

        private string ReadString()
        {
            if (_stream == null) return String.Empty;

            var size = ReadInt();
            
            Console.WriteLine("readString size: " + size);
            
            byte[] data = new byte[size];
            var totalRead = 0;
            var sizeToRead = size;
            while (totalRead != size)
            {
                //Console.WriteLine("readString: size - " + size + ", totalread - " + totalRead + ", sizeToRead - " + sizeToRead);
                
                var read = _stream.Read(data, totalRead, sizeToRead);
                sizeToRead -= read;
                totalRead += read;
            }
            var str = Encoding.UTF8.GetString(data);

            //Console.WriteLine("readString2. str:" + str);

            return str;
        }
    }
}