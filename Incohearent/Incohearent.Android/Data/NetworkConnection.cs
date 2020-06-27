using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using Incohearent.Data;
using Incohearent.Droid.Data;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(NetworkConnection))]

namespace Incohearent.Droid.Data
{
    public class NetworkConnection : INetworkConnection
    {              
        public bool IsConnected { get; set; }
        public void CheckNetworkConnection()
        {
            var ConnMan = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var ActiveNetworkInfo = ConnMan.ActiveNetworkInfo;
            if (ActiveNetworkInfo != null && ActiveNetworkInfo.IsConnectedOrConnecting)  //je deprecated/obsolete
            {
                IsConnected = true;
            }

            else
            {
                IsConnected = false;
            }

        }
        
        public string GetIpAddressDevice()
        {

            WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
            WifiInfo wifiInfo;
            string ipDevice = "";

            wifiInfo = wifiManager.ConnectionInfo;
            if (wifiInfo.SupplicantState == SupplicantState.Completed)
            {
                
                ipDevice = Formatter.FormatIpAddress(wifiInfo.IpAddress);
            }

            return ipDevice;
        }
    }
}