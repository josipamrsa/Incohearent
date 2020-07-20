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
        public User GameMaster {get;set;}

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

            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "originalPhraseFetch", (sender, phrase) =>
            {
                LBLPhrases.Text = phrase;
            });

            MessagingCenter.Subscribe<StartedSessionViewModel, string>(this, "phraseGenerated", (sender, phrase) =>
            {
                LBLPhrases.Text = phrase;
            });

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.ConnectSessionCommand.Execute(null);            
        }
    }
}