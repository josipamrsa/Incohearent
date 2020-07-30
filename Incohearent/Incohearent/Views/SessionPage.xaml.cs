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
        public StartedSessionViewModel ViewModel
        {
            get { return BindingContext as StartedSessionViewModel; }
            set { BindingContext = value; }
        }

        public User User { get; set; }
        public User GameMaster { get; set; }
        public SessionTimer SessionTimer { get; private set; }

        StackLayout parentLayout;

        public SessionPage(User user, User gameMaster)
        {
            InitializeComponent();
            var sessionStore = new SessionController(DependencyService.Get<ISQlite>());
            var pageStore = new PageService();

            User = user;
            GameMaster = gameMaster;

            ViewModel = new StartedSessionViewModel(User, GameMaster, sessionStore, pageStore);

            MessagingCenter.Subscribe<StartedSessionViewModel, Points>(this, "exitSession", (sender, points) => {              
                if (Device.RuntimePlatform == Device.Android)
                {
                    Application.Current.MainPage = new NavigationPage(new EndcardPage(points, User));
                }
            });

            // Izlistaj igrače
            MessagingCenter.Subscribe<StartedSessionViewModel, List<User>>(this, "listAllPlayers", (sender, list) =>
            {
                Color[] ColorScheme = Constants.PlayerColors;
                Random rnd = new Random();

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
        protected override void OnAppearing()
        {
            base.OnAppearing();            
            ViewModel.ConnectSessionCommand.Execute(null);
            if (User.PrivateAddress == GameMaster.PrivateAddress)
                BTNExitSession.IsVisible = true;
        }

        public void AddPlayerButtons(string name, Color color, User player)
        {         
            Button playerButton = new Button { Text = name };
            
            playerButton.SetBinding(Button.CommandProperty, new Binding("SendWinnerCommand"));
            playerButton.CommandParameter = player;           

            playerButton.BindingContext = ViewModel;
            playerButton.BackgroundColor = color;
            playerButton.TextColor = Constants.PlayerTextColor;
            playerButton.Margin = new Thickness(60, 10, 60, 0);

            parentLayout = buttonStack;
            parentLayout.Children.Add(playerButton);
        }

        public void SetupTimer(Image timeClock, User user, User gm)
        {
            timeClock.HeightRequest = Constants.IconHeight;
            timeClock.Source = Constants.HourGlassImageSrc;

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

            else
            {
                SessionTimer.Stop();
                SessionTimer.Start();
            }                            
        }
    }
}