using Incohearent.Data;
using Incohearent.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Incohearent.ViewModels
{
    public class StartedSessionViewModel : IncohearentBaseViewModel
    {
        private SessionViewModel sessionVm;
        private ISessionStore sessionStore;
        private IPageService pageService;      

        public User User { get; private set; }
        public User GameMaster { get; private set; }
        public Session Session { get; private set; }
        public PhoneticPhrases Phrase { get; private set; }
        public List<User> PlayersInSession { get; private set; }

        public SessionViewModel SessionVm
        {
            get => sessionVm;
            set
            {
                SetValue(ref sessionVm, value);
            }
        }

        private HubConnection hubConn;
        public ICommand FetchPhrasesCommand { get; private set; }
        public ICommand ConnectSessionCommand { get; private set; }
        public ICommand ListPlayersCommand { get; private set; }
        public ICommand SendWinnerCommand { get; private set; }

        public StartedSessionViewModel(User user, User gm, ISessionStore ss, IPageService ps)
        {
            sessionStore = ss;
            pageService = ps;

            User = user;
            GameMaster = gm;
            Phrase = new PhoneticPhrases();
            PlayersInSession = new List<User>();

            hubConn = new HubConnectionBuilder().WithUrl(Constants.ServerConfiguration).Build();

            ConnectSessionCommand = new Command(async () => await ConnectToSession(User));         
            FetchPhrasesCommand = new Command(async () => await FetchPhrases(User, GameMaster, Phrase));           
            SendWinnerCommand = new Command(async () => await SendInWinner());

            Session = new Session();

            hubConn.On<string>("PhrasesGenerated", (phrase) =>
            {
                MessagingCenter.Send(this, "phraseGenerated", phrase);
            });

            hubConn.On<string>("OriginalPhraseFetched", (phrase) =>
            {
                MessagingCenter.Send(this, "originalPhraseFetch", phrase);
                MessagingCenter.Send(this, "listAllPlayers", PlayersInSession);
            });

            hubConn.On<PhoneticPhrases>("PhrasesNotGenerated", (phrase) =>
            {
                MessagingCenter.Send(this, "phraseNotGenerated", phrase);
            });

            hubConn.On<User>("ConnectSession", (logged) =>
            {              
                PlayersInSession.Add(logged);
            });
        }

        private async Task SendInWinner()
        {
             
        }

        private async Task ConnectToSession(User user)
        {
            await hubConn.StartAsync();
            await hubConn.InvokeAsync("ConnectSession", user);
            MessagingCenter.Send(this, "userSession", "session");
        }

        private async Task FetchPhrases(User user, User gm, PhoneticPhrases phrase)
        {
            await hubConn.InvokeAsync("GeneratePhrases", user, gm, phrase);                        
        }
    }
}
