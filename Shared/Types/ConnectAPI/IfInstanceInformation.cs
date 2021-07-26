using System;
using System.Collections.Generic;
using System.Net;

namespace Shared.Types.ConnectAPI
{
    /// <summary>
    /// The information broadcasted by Infinite Flight instances over the UDP.
    /// </summary>
    public class IfInstanceInformation
    {
        /// <summary>
        /// Equals to "Playing" if this instance has an active flight (not in the main menu).
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// The port this instance's TCP server is running on.
        /// Seems to be always equal to 10111 (as of 2021). The v2 API runs on 10112, so this field is unused.
        /// </summary>
        public int Port { get; set; }
        
        /// <summary>
        /// The ID of the device this instance is running on.
        /// Ex.: iPad7.
        /// </summary>
        public string DeviceID { get; set; }
        
        /// <summary>
        /// The current aircraft. Can be "Unknown" if this instance does not have a flight active yet.
        /// Ex.: Cessna 172.
        /// </summary>
        public string Aircraft { get; set; }
        
        /// <summary>
        /// The version of the client.
        /// Ex.: 21.3.0.1030.
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// The (custom) name of the device this instance is running on.
        /// Ex.: Alex’s iPad.
        /// </summary>
        public string DeviceName { get; set; }
        
        /// <summary>
        /// The IP addresses this instance's TCP server runs on.
        /// Ex.: ["fe80::18c4:27e2:15c3:3662%11", "10.0.0.227", "2601:184:4080:fa60:15d7:c755:4f75:f46b"]
        /// </summary>
        public List<string> Addresses { get; set; }
        
        /// <summary>
        /// The current livery name. Can be "Unknown" if this instance does not have a flight active yet.
        /// Ex.: Default.
        /// </summary>
        public string Livery { get; set; }
        
        public IPAddress[] IpAddressesArray
        {
            get
            {
                if (Addresses == null || Addresses.Count <= 0) return Array.Empty<IPAddress>();

                var list = new List<IPAddress>();

                foreach (var stringAddress in Addresses)
                {
                    try
                    {
                        list.Add(IPAddress.Parse(stringAddress));
                    }
                    catch
                    {
                        continue;
                    }
                }
                
                return list.ToArray();
            }
        }
    }
}