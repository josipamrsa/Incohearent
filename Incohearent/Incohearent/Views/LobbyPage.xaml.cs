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
        // Da se vrijednosti automatski dohvaćaju / mijenjaju te da se mogu koristiti metode iz VM-a
        public LobbyAssignViewModel ViewModel
        {
            get { return BindingContext as LobbyAssignViewModel; }
            set { BindingContext = value; }
        }

        public User LoggedUser { get; set; } // Ulogirani korisnik

        public LobbyPage(User user)
        {
            InitializeComponent();
            var lobbyStore = new LobbyController(DependencyService.Get<ISQlite>());     // Metode za rad s bazom i poziv bazi
            var pageStore = new PageService();                                          // Metode za obavijesti (neiskorišteno)
            LoggedUser = user;                                                          // Zabilježi ulogiranog korisnika
            ViewModel = new LobbyAssignViewModel(LoggedUser, lobbyStore, pageStore);    // Inicijaliziraj ViewModel

            //----PORUKE----//

            // Tko se priključio lobbyju
            MessagingCenter.Subscribe<LobbyAssignViewModel, string>(this, "joinedLobby", (sender, info) => {
                LBLPlayerConnections.Text += info + "\n";
            });

            // Tko je izašao iz lobbyja
            MessagingCenter.Subscribe<LobbyAssignViewModel, string>(this, "leftLobby", (sender, info) => {
                LBLPlayerConnections.Text += info + "\n";
            });

            // Kada korisnik izađe iz lobbyja, izlazi se skroz iz aplikacije
            MessagingCenter.Subscribe<LobbyAssignViewModel, string>(this, "exitApp", (sender, info) => {
                if (Device.RuntimePlatform == Device.Android)
                {                   
                    Environment.Exit(0); 
                }
            });

            // Ukoliko je samo jedan igrač prisutan
            MessagingCenter.Subscribe<LobbyAssignViewModel, bool>(this, "lessThanTwo", (sender, less) => {
                DisplayAlert(Constants.OnePlayerOnlyDetectedTitle, Constants.OnePlayerOnlyDetectedText, "Got it!");          
            });
           
            // Pokreni igru kada su svi uvjeti zadovoljeni
            MessagingCenter.Subscribe<LobbyAssignViewModel, User>(this, "sessionStart", (sender, gameMaster) =>
            {               
                if (Device.RuntimePlatform == Device.Android)
                {
                    Application.Current.MainPage = new NavigationPage(new SessionPage(LoggedUser, gameMaster));
                }
            });           
        }

        // Event kad se nova komponenta učitava
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LBLPlayerConnections.Text = "";                       // Labela za prikaz ulogiranih korisnika
            ViewModel.ConnectToLobbyCommand.Execute(null);        // Spajanje u lobby
        }       
    }
}