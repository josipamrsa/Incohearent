using Incohearent.Controllers;
using Incohearent.Data;
using Incohearent.Models;
using Incohearent.ViewModels;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Incohearent.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LobbyPage : ContentPage
    {
        public LobbyAssignViewModel ViewModel
        {
            get { return BindingContext as LobbyAssignViewModel; }
            set { BindingContext = value; }
        }

        public User LoggedUser { get; set; }
        public bool MoreThanOnePlayer { get; set; }

        public LobbyPage(User user)
        {
            InitializeComponent();
            var lobbyStore = new LobbyController(DependencyService.Get<ISQlite>());
            var pageStore = new PageService();
            LoggedUser = user;
            ViewModel = new LobbyAssignViewModel(LoggedUser, lobbyStore, pageStore);

            MessagingCenter.Subscribe<LobbyAssignViewModel, string>(this, "joinedLobby", (sender, info) => {
                LBLPlayerConnections.Text += info + "\n";
            });

            MessagingCenter.Subscribe<LobbyAssignViewModel, string>(this, "leftLobby", (sender, info) => {
                LBLPlayerConnections.Text += info + "\n";
            });

            MessagingCenter.Subscribe<LobbyAssignViewModel, string>(this, "exitApp", (sender, info) => {
                if (Device.RuntimePlatform == Device.Android)
                {                   
                    Environment.Exit(0); 
                }
            });

            MessagingCenter.Subscribe<LobbyAssignViewModel, bool>(this, "lessThanTwo", (sender, less) => {
                DisplayAlert(Constants.OnePlayerOnlyDetectedTitle, Constants.OnePlayerOnlyDetectedText, "Got it!");          
            });
           
            MessagingCenter.Subscribe<LobbyAssignViewModel, User>(this, "sessionStart", (sender, gameMaster) =>
            {               
                if (Device.RuntimePlatform == Device.Android)
                {
                    Application.Current.MainPage = new NavigationPage(new SessionPage(LoggedUser, gameMaster));
                }
            });           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LBLPlayerConnections.Text = "";
            ViewModel.ConnectToLobbyCommand.Execute(null);        
        }       
    }
}