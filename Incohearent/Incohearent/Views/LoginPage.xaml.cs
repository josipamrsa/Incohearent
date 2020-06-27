using Incohearent.Data;
using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            RegisterUser(EnUser.Text);
            LobbyAssign();
        }
        private void RegisterUser(string username)
        {
            /*
            
            Aplikacija je zamišljena da se korisnici spajaju u određeni lobby na način da bi svi trebali biti
            na istom WiFi-ju ako žele igrati skupa. Primjerice, u grupi od 4 ljudi, ako jedan nije spojen na WiFi
            na koji su svi ostali spojeni, on će se pojaviti u drukčijem lobbyju nego ostali. Igra i jest zamišljena
            tako da se igra u grupi, kao i originalna kartaška igra.
            
            */

            var networkConnection = DependencyService.Get<INetworkConnection>();
            string publicAddress = App.RestApi.GetPublicIpAddress();
            string privateAddress = networkConnection.GetIpAddressDevice();

            try
            {
                if (!username.Equals(""))
                {                   
                    User u = new User(username, publicAddress, privateAddress);
                    System.Diagnostics.Debug.WriteLine(u.Username + ">>" + u.PrivateAddress + ", " + u.PublicAddress);
                    //App.UserDb.SaveUser(u);
                    DisplayAlert("Login Succesful", "Proceed!", "OK");
                }
            }

            catch (Exception ex)
            {
                DisplayAlert("Login Failed", "Please choose a name!", "OK");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }        
        }

        private void LobbyAssign()
        {
            /*
            
            Ova metoda će, nakon provjere WiFi-ja, stavljati igrača u prikladnu sobu/lobby.
            
            */
        }       
    }
}