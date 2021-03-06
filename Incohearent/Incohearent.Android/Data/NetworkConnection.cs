﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Telecom;
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
        // Metode provjere povezanosti na mrežu
        public bool IsConnected { get; set; }
        
        // Provjeri povezanost na mrežu
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
        
        // Dohvati interni IP ako je na WiFi-ju
        public string GetIpAddressDevice()
        {
            WifiManager wifiMan = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
            WifiInfo wifiInfo;
            string ipDevice = "";

            wifiInfo = wifiMan.ConnectionInfo;
            if (wifiInfo.SupplicantState == SupplicantState.Completed)
            {
                
                ipDevice = Formatter.FormatIpAddress(wifiInfo.IpAddress);
            }

            return ipDevice;
        }

        // Dohvati interni IP ako nije na WiFi-ju
        public string GetIPAddressCellularNetwork()
        {
            IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());
            if (adresses != null && adresses[0] != null) { return adresses[0].ToString(); }
            else { return null; }
        }

        // Provjeri stanje povezanosti na WiFi
        public bool UserIsOnWifi()
        {
            bool userOnWifi = false;
            ConnectivityManager connMan = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            NetworkInfo isWifi = connMan.GetNetworkInfo(ConnectivityType.Wifi);
            if (isWifi.IsConnected) userOnWifi = true;
            return userOnWifi;
        }
    }
}