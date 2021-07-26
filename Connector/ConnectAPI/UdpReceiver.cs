using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Shared.Types.ConnectAPI;

namespace Connector.ConnectAPI
{
    public class UdpReceiver
    {
        private const int UdpPort = 15000;
        private int TimesToTryReceiveUdpBroadcast = 1;
        
        public IfInstanceInformation TryFindRunningInstances()
        {
            var udpListener = new UdpClient(UdpPort);
            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, UdpPort);

            var currentAttempt = 0;

            while (currentAttempt < TimesToTryReceiveUdpBroadcast)
            {
                byte[] dataReceived = udpListener.Receive(ref listenEndPoint);

                if (dataReceived.Length > 0)
                {
                    return ProcessReceivedData(dataReceived);
                }

                currentAttempt++;
            }
            
            return null;
        }

        private IfInstanceInformation ProcessReceivedData(byte[] dataReceived)
        {
            var receivedString = Encoding.UTF8.GetString(dataReceived);

            IfInstanceInformation parsedData;

            try
            {
                parsedData = JsonConvert.DeserializeObject<IfInstanceInformation>(receivedString);
            }
            catch
            {
                return null;
            }
            
            return parsedData;
        }
    }
}