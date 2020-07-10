using Incohearent.Controllers;
using Incohearent.Data;
using Incohearent.Models;
using Incohearent.ViewModels;
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

            MessagingCenter.Subscribe<LobbyAssignViewModel, string>(this, "userSignOut", (sender, info) => {
                if (Device.RuntimePlatform == Device.Android)
                {                    
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
            });
        }

        protected override void OnAppearing()
        {
            LBLPlayerConnections.Text = "";
            ViewModel.ConnectToLobbyCommand.Execute(LoggedUser);          
            base.OnAppearing();
        }


    }
}