using Incohearent.Data;
using Incohearent.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Incohearent.ViewModels
{
    // Upravljanje Lobbyjem
    public class LobbyAssignViewModel : IncohearentBaseViewModel
    {
        private LobbyViewModel lobbyVm;     // ViewModel svojstvo za poziv iz Viewa
        private ILobbyStore lobbyStore;     // Metode za rad s tablicom Lobby modela u bazi podataka 
        private IPageService pageService;   // Metode za rad sa obavijestima (neiskorišteno)
        
        public int PlayerCount { get; set; }        // Broj trenutnih igrača
        public User User { get; private set; }      // Igrač - korisnik
        public Lobby Lobby { get; private set; }    // Lobby u kojem se nalazi
        
        public LobbyViewModel LobbyVm {
            get => lobbyVm;
            set
            {
                SetValue(ref lobbyVm, value);
            }
        }

        private HubConnection hubConn; // Veza na Hub

        public ICommand ConnectToLobbyCommand { get; private set; }         // Naredba za spajanje u Lobby
        public ICommand DisconnectFromLobbyCommand { get; private set; }    // Naredba za izlazak iz Lobbyja
        public ICommand SaveLobbyCommand { get; private set; }              // Spremanje zapisa o Lobbyju
        public ICommand StartSessionCommand { get; private set; }           // Pokretanje igre

        public LobbyAssignViewModel(User user, ILobbyStore ls, IPageService ps)
        {
            lobbyStore = ls;
            pageService = ps;
            User = user;
            PlayerCount = 0;
           
            hubConn = new HubConnectionBuilder().WithUrl(Constants.ServerConfiguration).Build(); // Stvori novu vezu između Hub-a na serveru i klijenta

            // Inicijalizacija naredbi
            ConnectToLobbyCommand = new Command(async () => await ConnectToLobby(user));
            DisconnectFromLobbyCommand = new Command(async () => await DisconnectFromLobby(user));
            SaveLobbyCommand = new Command(async () => await SaveLobby(Lobby));
            StartSessionCommand = new Command(async () => await StartSession(User));

            // Inicijalizacija objekta lobbyja za spremanje u bazu
            Lobby = new Lobby();
            
            //----PORUKE----//

            // Kada se korisnik spoji u lobby, obavijesti ostale igrače i dodaj njegov zapis u bazu podataka
            hubConn.On<User, Lobby>("JoinLobby", (loggedUser, newLobby) =>
            {
                SaveLobbyCommand.Execute(newLobby);
                MessagingCenter.Send(this, "joinedLobby", $"User {loggedUser.Username} has joined the lobby ({newLobby.GatewayIp}).");                                                         
            });

            // Kada se korisnik odspoji iz lobbyja, obavijesti druge igrače
            hubConn.On<User>("LeaveLobby", (loggedUser) =>
            {
                MessagingCenter.Send(this, "leftLobby", $"User {loggedUser.Username} has left the lobby."); 
            });

            // Kada jedan od korisnika pokrene igru, provjeri broj igrača i pokreni igru ako je sve u redu           
            hubConn.On<User>("StartGame", async (gameMaster) =>
            {           
                if (PlayerCount<2) { MessagingCenter.Send(this, "lessThanTwo", true); }
                else {
                    MessagingCenter.Send(this, "sessionStart", gameMaster);
                    await hubConn.StopAsync();
                }                                    
            });

            // Broj igrača dobiva se sa servera sa OnConnected/OnDisconnected eventovima 
            // Svaki put kad se novi igrač pridruži pošalje se poruka sa servera
            hubConn.On<int>("NumberOfPlayers", (amount) => { 
                PlayerCount = amount;              
            });
        }
       
        //----METODE----//

        // Pokretanje igre
        private async Task StartSession(User user)
        {
            await hubConn.InvokeAsync("StartGame", user);
            //await hubConn.StopAsync();
        }

        // Priključivanje u Lobby
        private async Task ConnectToLobby(User user)
        {                     
            await hubConn.StartAsync();
            await hubConn.InvokeAsync("JoinLobby", user);
        }

        // Isključivanje iz Lobbyja
        private async Task DisconnectFromLobby(User user)
        {           
            await hubConn.InvokeAsync("LeaveLobby", user);
            await hubConn.StopAsync();
            MessagingCenter.Send(this, "exitApp", "OK");
        }

        // Spremanje Lobbyja u bazu podataka
        private async Task SaveLobby(Lobby lobby)
        {
            Lobby = lobby;
            await lobbyStore.AddLobby(Lobby);         
        }
    }
}
