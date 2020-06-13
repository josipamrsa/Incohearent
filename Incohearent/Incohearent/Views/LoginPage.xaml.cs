using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Incohearent.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            LBLUser.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            App.StartCheckIfInternet(lbl_NoInternet, this);
        }

        private void SignIntoLobby(object sender, EventArgs e)
        {
            User u = new User(EnUser.Text, "127.0.0.1"); // test

            if (u.CheckInformation())  { 
                DisplayAlert("Information", "Success", "OK");
                CheckWiFi();
                LobbyAssign();
            }
            else
                DisplayAlert("Information", "Login Failed", "OK");
        }
        private void CheckWiFi()
        {
            /*
            
            Aplikacija je zamišljena da se korisnici spajaju u određeni lobby na način da bi svi trebali biti
            na istom WiFi-ju ako žele igrati skupa. Primjerice, u grupi od 4 ljudi, ako jedan nije spojen na WiFi
            na koji su svi ostali spojeni, on će se pojaviti u drukčijem lobbyju nego ostali. Igra i jest zamišljena
            tako da se igra u grupi, kao i originalna kartaška igra.
            
            */
        }

        private void LobbyAssign()
        {
            /*
            
            Ova metoda će, nakon provjere WiFi-ja, stavljati igrača u prikladnu sobu/lobby.
            
            */
        }       
    }
}