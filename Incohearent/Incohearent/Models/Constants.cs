using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Incohearent.Models
{
    // Postavke aplikacije
    public class Constants
    {
        public static bool IsDev = true; // debug

        //----Color scheme----//
        public static Color BackgroundColor = Color.White;
        public static Color MainTextColor = Color.Black;
        public static Color PlayerTextColor = Color.White;

        public static Color[] PlayerColors = {
            Color.DarkOrchid,
            Color.DarkGoldenrod,
            Color.DarkCyan,
            Color.Tomato,
            Color.LightSlateGray,
            Color.MediumVioletRed,
            Color.MidnightBlue
        };

        //----Login banner--//
        public static int LoginIconHeight = 150;

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

        public static string LoginFailedUnknownTitle = "Hmm?";
        public static string LoginFailedUnknownText = "What an odd error to have.";


        //----Session warning----//
        public static string OnePlayerOnlyDetectedTitle = "Introvert Much?";
        public static string OnePlayerOnlyDetectedText = "Better grab some friends to play with! This is a party game! :)";

        //----Web server configuration----//
        public static string LocalWebServer = "http://localhost:44733/gameHub";
        public static string AzureWebServer = "https://incohearentwebserver.azurewebsites.net/gameHub";
        public static string NgrokServer = "https://66b6df9121e8.ngrok.io/gameHub"; // testni server

        public static string ServerConfiguration = NgrokServer;

        //----Session----//
        public static string NoOneWins = "No One (Womp Womp)";
        public static string HourGlassImageSrc = "hourglass.gif";
        public static string AlarmImageSrc = "alarm.gif";
        public static int IconHeight = 50;

        public static string DeclareWinnerTitle = "We have a winner!";
        public static string DeclareWinnerInfo = "A shiny point for: ";

        public static string TimeUpTitle = "Time's Up!";
        public static string TimeUpInfo = "No one was worthy of a point...";

        //----Endcard----//
        public static string PlayerName = "Player name: ";
        public static string PlayerPoints = "Player points: ";
        public static string GameMasterInfo = "Was Ringleader";
    }
}
