using Incohearent.Controllers;
using Incohearent.Data;
using Incohearent.Models;
using Incohearent.ViewModels;
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
        // Da se vrijednosti automatski dohvaćaju / mijenjaju te da se mogu koristiti metode iz VM-a
        public LoginViewModel ViewModel
        {
            get { return BindingContext as LoginViewModel; }
            set { BindingContext = value; }
        }
        public LoginPage()
        {
            var userStore = new LoginController(DependencyService.Get<ISQlite>());      // Metode za rad s bazom i poziv bazi
            var pageStore = new PageService();                                          // Metode za obavijesti (neiskorišteno)
            ViewModel = new LoginViewModel(userStore, pageStore);                       // Inicijaliziraj ViewModel
            InitializeComponent();            
            Init();                                                                     // Pri pokretanju postavi određene elemente

            //----PORUKE----//

            // Korisnik nije unio ime
            MessagingCenter.Subscribe<LoginViewModel, string>(this, "loginFail", (sender, user) => {
                DisplayAlert(Constants.LoginFailedTitle, Constants.LoginFailedText, "Got it!");          
            });

            // Greška kod dohvata IP adrese uređaja
            MessagingCenter.Subscribe<LoginViewModel, string>(this, "ipNotFound", (sender, user) => {
                DisplayAlert(Constants.LoginFailedUnknownTitle, Constants.LoginFailedUnknownText, "Well, okay.");
            });

            // Upozorenje da korisnik nije spojen na WiFi mrežu
            MessagingCenter.Subscribe<LoginViewModel, string>(this, "notOnWifi", (sender, info) => {
                DisplayAlert(Constants.NotOnWifiTitle, Constants.NotOnWifiWarning, "Got it!");
            });

            // Korisnik nije općenito spojen na mrežu
            MessagingCenter.Subscribe<LoginViewModel, string>(this, "networkFailure", (sender, info) => {
                DisplayAlert(Constants.NoInternetTitle, Constants.NoInternetWarning, "Got it!");
            });

            // Uspješan login - korisnik svoje podatke šalje u novu komponentu
            MessagingCenter.Subscribe<LoginViewModel, User>(this, "loggedIn", (sender, user) =>
            {
                if (Device.RuntimePlatform == Device.Android)
                {        
                    Application.Current.MainPage = new NavigationPage(new LobbyPage(user));
                }
            });
        }

        private void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            LBLUser.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;

            // Provjeri je li ostvarena mrežna veza
            App.StartCheckIfInternet(lbl_NoInternet, this);
        }
    }
}