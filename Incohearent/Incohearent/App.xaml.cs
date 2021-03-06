﻿using Incohearent.Controllers;
using Incohearent.Data;
using Incohearent.Models;
using Incohearent.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Incohearent
{
    public partial class App : Application
    {
        // Kontrole
        private static Label labelScreen;
        private static Page currentPage;

        // Internet
        private static bool hasInternet;      
        private static Timer timer;
        private static bool noInternetShow;

        // REST API
        static RestApiService restApi;
        
        public static RestApiService RestApi
        {
            get
            {
                if (restApi==null) { restApi = new RestApiService(); }
                return restApi;
            }
        }
        
        public App()
        {
            InitializeComponent();
            MainPage = new LoginPage(); // Što je početna stranica
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        // Pokretanje provjere povezanosti na mrežu
        public static void StartCheckIfInternet(Label label, Page page)
        {
            // Potrebne kontrole za prikaz statusa mreže
            labelScreen = label;
            label.Text = Constants.NoInternetText;
            label.IsVisible = false;
            hasInternet = true;
            currentPage = page;

            // Provjeravaj svakih 10 sekundi stanje povezanosti na mrežu
            if (timer == null)
            {
                timer = new Timer((e) => {
                    CheckIfInternetOverTime();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        // Metoda provjere povezanosti na mrežu
        private static void CheckIfInternetOverTime()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            if (!networkConnection.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    if (hasInternet)
                    {
                        if (!noInternetShow)
                        {
                            hasInternet = false;
                            labelScreen.IsVisible = true;
                            await ShowDisplayAlert();
                        }
                    }
                });
            }

            else
            {
                Device.BeginInvokeOnMainThread(() => {
                    hasInternet = true;
                    labelScreen.IsVisible = false;
                });
            }
        }

        // Instantno provjerava ima li interneta - primjerice pritisak na botun provjere interneta
        public static async Task<bool> CheckIfInternet()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            return networkConnection.IsConnected;
        }

        // Async verzija gornje metode
        public static async Task<bool> CheckIfInternetAlertAsync()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            if (!networkConnection.IsConnected)
            {
                if (!noInternetShow)
                {
                    await ShowDisplayAlert();
                }
                return false;
            }
            return true;
        }

        // Prikaz obavijesti o stanju povezanosti na mrežu
        private static async Task ShowDisplayAlert()
        {
            noInternetShow = false;
            await currentPage.DisplayAlert(Constants.NoInternetTitle, Constants.NoInternetWarning, "Got it!");
            noInternetShow = false;
        }
    }
}
