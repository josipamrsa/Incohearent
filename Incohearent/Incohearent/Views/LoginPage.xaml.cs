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
        public LoginViewModel ViewModel
        {
            get { return BindingContext as LoginViewModel; }
            set { BindingContext = value; }
        }
        public LoginPage()
        {
            var userStore = new LoginController(DependencyService.Get<ISQlite>());
            var pageStore = new PageService();
            ViewModel = new LoginViewModel(userStore, pageStore);
            InitializeComponent();            
            Init();

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "loginFail", (sender, user) => {
                DisplayAlert(Constants.LoginFailedTitle, Constants.LoginFailedText, "Got it!");          
            });

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "ipNotFound", (sender, user) => {
                DisplayAlert(Constants.LoginFailedUnknownTitle, Constants.LoginFailedUnknownText, "Well, okay.");
            });

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "notOnWifi", (sender, info) => {
                DisplayAlert(Constants.NotOnWifiTitle, Constants.NotOnWifiWarning, "Got it!");
            });

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "networkFailure", (sender, info) => {
                DisplayAlert(Constants.NoInternetTitle, Constants.NoInternetWarning, "Got it!");
            });

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
            App.StartCheckIfInternet(lbl_NoInternet, this);
        }
    }
}