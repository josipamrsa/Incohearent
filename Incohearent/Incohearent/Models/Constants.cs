using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Incohearent.Models
{
    public class Constants
    {
        public static bool IsDev = true; // Za debug/dev svrhu

        //----Color scheme----//
        public static Color BackgroundColor = Color.White;
        public static Color MainTextColor = Color.Black;

        //----Login banner--//
        public static int LoginIconHeight = 180;

        //----Network connection state--//
        public static string NoInternetText = "Oh no! You're not connected to the Internet!";
        public static string NoInternetTitle = "No Internet Connection!";
        public static string NoInternetWarning = "Please connect to a network! Preferably WiFi.";

        //----Wifi check----//
        public static string NotOnWifiTitle = "Vibe Check!";
        public static string NotOnWifiWarning = "We've noticed you are not connected to a WiFi. " +
                                                "If you want to play with your friends, make sure you're all " +
                                                "connected to the same WiFi network!";

        //----Login fail----//
        public static string LoginFailedTitle = "Tell Us Your Name First";
        public static string LoginFailedText = "We need to know your screen name first, before we let you in!";

    }
}
