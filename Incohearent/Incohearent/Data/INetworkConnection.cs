using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Data
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
        string GetIpAddressDevice();
        string GetIPAddressCellularNetwork();
        bool UserIsOnWifi();
    }
}
