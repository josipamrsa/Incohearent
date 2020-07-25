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
    public partial class SessionPage : ContentPage
    {
        public StartedSessionViewModel ViewModel
        {
            get { return BindingContext as StartedSessionViewModel; }
            set { BindingContext = value; }
        }

        public User User { get; set; }
        public User GameMaster { get; set; }
        StackLayout parentLayout;

        public SessionPage(User user, User gameMaster)
        {
            InitializeComponent();
            var sessionStore = new SessionController(DependencyService.Get<ISQlite>());
            var pageStore = new PageService();

            User = user;
            GameMaster = gameMaster;
           
            ViewModel = new StartedSessionViewModel(User, GameMaster, sessionStore, pageStore);
            
            MessagingCenter.Subscribe<StartedSessionViewModel, List<User>>(this, "listAllPlayers", (sender, list) =>
            {
                Color[] ColorScheme = Constants.PlayerColors;
                Random rnd = new Random();

                List<User> otherPlayers = list;
                otherPlayers.RemoveAll(x => x.PrivateAddress == gameMaster.PrivateAddress);

                foreach (User player in list)
                    AddPlayerButtons(player.Username, ColorScheme[rnd.Next(0, ColorScheme.Length)], player);

                AddPlayerButtons(Constants.NoOneWins, Constants.PlayerColors.First(), null);
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "notifyWinner", (sender, player) =>
            {
                //DisplayAlert("Winner", "Test", "OK");              
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "userSession", (sender, state) =>
            {
                ViewModel.FetchPhrasesCommand.Execute(null);
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, PhoneticPhrases>(this, "phraseNotGenerated", (sender, phrase) =>
            {
                ViewModel.FetchPhrasesCommand.Execute(null);
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "phraseGenerated", (sender, phrase) =>
            {
                TimerClock.HeightRequest = Constants.IconHeight;
                TimerClock.Source = Constants.HourGlassImageSrc;
                StartTimer(TimerClock);
                LBLPhrases.Text = phrase;
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "originalPhraseFetch", (sender, phrase) =>
            {
                TimerClock.HeightRequest = Constants.IconHeight;
                TimerClock.Source = Constants.HourGlassImageSrc;
                StartTimer(TimerClock);
                LBLPhrases.Text = phrase;
            });          
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.ConnectSessionCommand.Execute(null);                             
        }

        public void AddPlayerButtons(string name, Color color, User player)
        {
            Button playerButton = new Button { Text = name };
            
            playerButton.SetBinding(Button.CommandProperty, new Binding("SendWinnerCommand"));
            playerButton.CommandParameter = player;           

            playerButton.BindingContext = ViewModel;
            playerButton.BackgroundColor = color;
            playerButton.TextColor = Constants.PlayerTextColor;

            parentLayout = sessionStack;
            parentLayout.Children.Add(playerButton);
        }

        public void StartTimer(Image timeClock)
        {           
            Device.StartTimer(TimeSpan.FromSeconds(60), () =>
            {
                timeClock.Source = Constants.AlarmImageSrc;
                return false; // True = Repeat again, False = Stop the timer
            });
        }
    }
}