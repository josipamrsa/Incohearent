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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Incohearent.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SessionPage : ContentPage
    {
        // Da se vrijednosti automatski dohvaćaju / mijenjaju te da se mogu koristiti metode iz VM-a
        public StartedSessionViewModel ViewModel
        {
            get { return BindingContext as StartedSessionViewModel; }
            set { BindingContext = value; }
        }

        public User User { get; set; }                              // Igrač
        public User GameMaster { get; set; }                        // Sudac - GameMaster
        public SessionTimer SessionTimer { get; private set; }      // Timer za igru 

        StackLayout parentLayout;                                   // Container za dugmad igrača

        public SessionPage(User user, User gameMaster)
        {
            InitializeComponent();
            var sessionStore = new SessionController(DependencyService.Get<ISQlite>());             // Metode za rad s bazom i poziv bazi
            var pageStore = new PageService();                                                      // Metode za obavijesti (neiskorišteno)

            User = user;                                                                            // Spremanje vrijednosti igrača
            GameMaster = gameMaster;                                                                // Spremanje vrijednosti GameMastera

            ViewModel = new StartedSessionViewModel(User, GameMaster, sessionStore, pageStore);     // Inicijaliziraj ViewModel

            //---PORUKE----//

            // Izlaz iz igre
            MessagingCenter.Subscribe<StartedSessionViewModel, Points>(this, "exitSession", (sender, points) => {              
                if (Device.RuntimePlatform == Device.Android)
                {
                    Application.Current.MainPage = new NavigationPage(new EndcardPage(points, User));
                }
            });

            // Izlistaj igrače
            MessagingCenter.Subscribe<StartedSessionViewModel, List<User>>(this, "listAllPlayers", (sender, list) =>
            {
                // Boje dugmadi
                Color[] ColorScheme = Constants.PlayerColors;
                Random rnd = new Random();

                // Izbacivanje GameMastera iz dobivene liste
                List<User> otherPlayers = list;
                otherPlayers.RemoveAll(x => x.PrivateAddress == gameMaster.PrivateAddress);

                if (parentLayout != null) buttonStack.Children.Clear();
                foreach (User player in list)
                    AddPlayerButtons(player.Username, ColorScheme[rnd.Next(0, ColorScheme.Length)], player);               
            });

            // Pokreni timer
            MessagingCenter.Subscribe<StartedSessionViewModel, int>(this, "setupTimer", (sender, code) =>
            {                
                SetupTimer(TimerClock, User, GameMaster);
            });

            // Obavijesti o pobjedniku
            MessagingCenter.Subscribe<StartedSessionViewModel, User>(this, "wonNotification", (sender, player) =>
            {
                DisplayAlert(Constants.DeclareWinnerTitle, Constants.DeclareWinnerInfo + player.Username, "Great!");
                if (User.PrivateAddress == GameMaster.PrivateAddress)
                    ViewModel.FetchPhrasesCommand.Execute(null);
            });

            // Uključi igrače u sesiju
            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "userSession", (sender, state) =>
            {
                if (User.PrivateAddress == GameMaster.PrivateAddress)
                    ViewModel.FetchPhrasesCommand.Execute(null);                
            });

            // Ako nema optimalne fraze, šalji zahtjev ponovno
            MessagingCenter.Subscribe<StartedSessionViewModel, PhoneticPhrases>(this, "phraseNotGenerated", (sender, phrase) =>
            {
                ViewModel.FetchPhrasesCommand.Execute(null);
            });

            // Igračima podijeli randomizirani fonetski oblik fraze
            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "phraseGenerated", (sender, phrase) =>
            {               
                LBLPhrases.Text = phrase;
            });

            // GameMasteru podijeli originalni oblik fraze
            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "originalPhraseFetch", (sender, phrase) =>
            {
                LBLPhrases.Text = phrase;
            });          
        }

        // Pri pojavljivanju komponente
        protected override void OnAppearing()
        {
            base.OnAppearing();            
            ViewModel.ConnectSessionCommand.Execute(null);          // Spoji igrače u sesiju
            if (User.PrivateAddress == GameMaster.PrivateAddress)   // Daj GameMasteru mogućnost izlaska iz igre
                BTNExitSession.IsVisible = true;
        }

        // Dodavanje dugmadi za igrače - stvara novo dugme za svakog igrača 
        // kako bi GameMaster mogao potvrditi tko je pobjednik runde
        public void AddPlayerButtons(string name, Color color, User player)
        {         
            // Ime dugmeta
            Button playerButton = new Button { Text = name };
            
            // Koju naredbu koristi i koji parametar šalje
            playerButton.SetBinding(Button.CommandProperty, new Binding("SendWinnerCommand"));
            playerButton.CommandParameter = player;           

            // Da se naredba iz VM-a može aktivirati
            playerButton.BindingContext = ViewModel;

            // Stil dugmeta - svaki igrač dobiva svoju boju
            playerButton.BackgroundColor = color;
            playerButton.TextColor = Constants.PlayerTextColor;
            playerButton.Margin = new Thickness(60, 10, 60, 0);

            // Dodaj dugme u container
            parentLayout = buttonStack;
            parentLayout.Children.Add(playerButton);
        }

        // Postavljanje timera
        public void SetupTimer(Image timeClock, User user, User gm)
        {
            // Prikladna slika dok timer otkucava
            timeClock.HeightRequest = Constants.IconHeight;
            timeClock.Source = Constants.HourGlassImageSrc;

            // Ako timer nije postavljen (početak igre), neka vrijeme otkucava 120 sekundi, te 
            // po isteku vremena se ikona mijenja u prikladnu sliku i pokreće se naredba za
            // dohvaćanjem nove fraze. Timer se onda pokreće.
            if (SessionTimer == null)
            {
                SessionTimer = new SessionTimer(TimeSpan.FromSeconds(120), () =>
                {
                    timeClock.Source = Constants.AlarmImageSrc;
                    DisplayAlert(Constants.TimeUpTitle, Constants.TimeUpInfo, "Got it!");
                    if (user.PrivateAddress == gm.PrivateAddress)
                        ViewModel.FetchPhrasesCommand.Execute(null);
                });
                SessionTimer.Start();
            }

            // Ako se metoda pozove ponovo (igrač je pogodio frazu), trenutni timer se zaustavlja, te
            // se novi pokreće ako je timer već inicijaliziran
            else
            {
                SessionTimer.Stop();
                SessionTimer.Start();
            }                            
        }
    }
}