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
                LBLPhrases.Text = phrase;
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, List<User>>(this, "listAllPlayers", (sender, list) =>
            {
                Color[] ColorScheme = Constants.PlayerColors;
                Random rnd = new Random();

                // TESTIRATI
                //List<User> otherPlayers = list;
                //otherPlayers.RemoveAll(x => x.PrivateAddress == gameMaster.PrivateAddress);

                foreach (User player in list)
                    PlayerButtons(player.Username, ColorScheme[rnd.Next(0, ColorScheme.Length)]);                                
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "originalPhraseFetch", (sender, phrase) =>
            {
                LBLPhrases.Text = phrase;
            });

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.ConnectSessionCommand.Execute(null);            
        }

        public void PlayerButtons(string name, Color color)
        {
            Button playerButton = new Button { Text = name };
            playerButton.SetBinding(Button.CommandProperty, new Binding("SendWinnerCommand"));
            playerButton.BindingContext = ViewModel;
            playerButton.BackgroundColor = color;
            playerButton.TextColor = Constants.PlayerTextColor;
            parentLayout = sessionStack;
            parentLayout.Children.Add(playerButton);
        }
    }
}