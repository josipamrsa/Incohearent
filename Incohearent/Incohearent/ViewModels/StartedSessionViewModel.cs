using Incohearent.Data;
using Incohearent.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Incohearent.ViewModels
{
    public class StartedSessionViewModel : IncohearentBaseViewModel
    {
        private SessionViewModel sessionVm;     // ViewModel svojstvo za poziv iz Viewa
        private ISessionStore sessionStore;     // Metode za rad s tablicom Session modela u bazi podataka (trenutno neiskorišteno)
        private IPageService pageService;       // Metode za rad sa obavijestima (neiskorišteno)

        public User User { get; private set; }                      // Korisnik
        public User GameMaster { get; private set; }                // Sudac - GameMaster
        public Session Session { get; private set; }                // Sesija
        public PhoneticPhrases Phrase { get; private set; }         // Dobivena fraza
        public List<User> PlayersInSession { get; private set; }    // Igrači u sesiji

        public Points PlayerPoints { get; private set; }            // Broj bodova igrača

        public SessionViewModel SessionVm
        {
            get => sessionVm;
            set
            {
                SetValue(ref sessionVm, value);
            }
        }

        private HubConnection hubConn; // Veza na Hub
        public ICommand FetchPhrasesCommand { get; private set; }       // Dohvati frazu
        public ICommand ConnectSessionCommand { get; private set; }     // Spoji u sesiju
        public ICommand SendWinnerCommand { get; private set; }         // Obavijest o igraču koji je pobjednik runde
        public ICommand EndSessionCommand { get; private set; }         // Izlaz iz sesije
        
        public StartedSessionViewModel(User user, User gm, ISessionStore ss, IPageService ps)
        {
            sessionStore = ss;
            pageService = ps;

            User = user;
            GameMaster = gm;

            Phrase = new PhoneticPhrases();
            PlayersInSession = new List<User>();
            PlayerPoints = new Points(0, user.PrivateAddress, user.Username, false);

            hubConn = new HubConnectionBuilder().WithUrl(Constants.ServerConfiguration).Build();

            ConnectSessionCommand = new Command(async () => await ConnectToSession(User));         
            FetchPhrasesCommand = new Command(async () => await FetchPhrases(User, GameMaster, Phrase));
            SendWinnerCommand = new Command<User>(async (player) => await SendInWinner(player));
            EndSessionCommand = new Command(async () => await EndSession(User));

            Session = new Session();

            //----PORUKE----//

            // Dobivanje izokrenute fraze za ostale igrače
            hubConn.On<string>("PhrasesGenerated", (phrase) =>
            {
                MessagingCenter.Send(this, "phraseGenerated", phrase);
            });

            // Dobivanje originalne fraze za suca
            hubConn.On<string>("OriginalPhraseFetched", (phrase) =>
            {
                PlayerPoints.IsGameMaster = true;
                MessagingCenter.Send(this, "originalPhraseFetch", phrase);              
            });

            // Izlistavanje svih aktivnih igrača
            hubConn.On<int>("ListAllPlayers", (code) =>
            {
                MessagingCenter.Send(this, "listAllPlayers", PlayersInSession);
            });

            // Postavljanje timera
            hubConn.On<int>("SetupTimer", (code) =>
            {
                MessagingCenter.Send(this, "setupTimer", 0);
            });

            // Kada generiranje fraze nije uspješno, generiraj novu
            hubConn.On<PhoneticPhrases>("PhrasesNotGenerated", (phrase) =>
            {
                MessagingCenter.Send(this, "phraseNotGenerated", phrase);
            });

            // Spoji u sesiju
            hubConn.On<User>("ConnectSession", (logged) =>
            {               
                PlayersInSession.Add(logged);
            });

            // Proglašenje pobjednika
            hubConn.On<User>("WinnerDeclared", (logged) =>
            {
                if (logged.PrivateAddress == User.PrivateAddress) {                    
                    PlayerPoints.PointsWon++;
                }
                MessagingCenter.Send(this, "wonNotification", logged);
            });

            // Izlaz iz sesije
            hubConn.On<User>("DisconnectSession", async (logged) =>
            {
                MessagingCenter.Send(this, "exitSession", PlayerPoints);
                await hubConn.StopAsync();
            });
        }

        //----METODE----//

        // Izlaz iz sesije
        private async Task EndSession(User user)
        {
            await hubConn.InvokeAsync("DisconnectSession", user);
            MessagingCenter.Send(this, "exitSession", PlayerPoints);
        }

        // Proglasi pobjednika
        private async Task SendInWinner(User player)
        {
            if (player!=null)
                await hubConn.InvokeAsync("DeclareWinner", player);
            else
                await hubConn.InvokeAsync("DeclareWinner", GameMaster);
        }

        // Spoji u sesiju
        private async Task ConnectToSession(User u)
        {
            await hubConn.StartAsync();
            await hubConn.InvokeAsync("ConnectSession", u);
            MessagingCenter.Send(this, "userSession", "session");
        }

        // Dohvati frazu
        private async Task FetchPhrases(User user, User gm, PhoneticPhrases phrase)
        {
            await hubConn.InvokeAsync("GeneratePhrases", user, gm, phrase);                        
        }
    }
}
