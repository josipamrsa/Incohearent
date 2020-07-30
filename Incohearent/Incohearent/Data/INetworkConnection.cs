using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Data
{
    // Interface za metode za rad s mrežom
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
        string GetIpAddressDevice();
        string GetIPAddressCellularNetwork();
        bool UserIsOnWifi();
    }
}
