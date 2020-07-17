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
    public class LobbyAssignViewModel : IncohearentBaseViewModel
    {
        private LobbyViewModel lobbyVm;
        private ILobbyStore lobbyStore;
        private IPageService pageService;
        
        public User User { get; private set; }
        public Lobby Lobby { get; private set; }
        
        public LobbyViewModel LobbyVm {
            get => lobbyVm;
            set
            {
                SetValue(ref lobbyVm, value);
            }
        }

        private HubConnection hubConn;

        public ICommand ConnectToLobbyCommand { get; private set; }
        public ICommand DisconnectFromLobbyCommand { get; private set; }
        public ICommand SaveLobbyCommand { get; private set; }
        public ICommand StartSessionCommand { get; private set; }

        public LobbyAssignViewModel(User user, ILobbyStore ls, IPageService ps)
        {
            lobbyStore = ls;
            pageService = ps;
            User = user;
           
            hubConn = new HubConnectionBuilder().WithUrl("https://incohearentwebserver.azurewebsites.net/gameHub").Build();

            ConnectToLobbyCommand = new Command(async () => await ConnectToLobby(user));
            DisconnectFromLobbyCommand = new Command(async () => await DisconnectFromLobby(user));
            SaveLobbyCommand = new Command(async () => await SaveLobby(Lobby));
            StartSessionCommand = new Command(async () => await StartSession(User));

            Lobby = new Lobby();
            
            hubConn.On<User, Lobby>("JoinLobby", (loggedUser, newLobby) =>
            {
                SaveLobbyCommand.Execute(newLobby);
                MessagingCenter.Send(this, "joinedLobby", $"User {loggedUser.Username} has joined the lobby ({newLobby.GatewayIp}).");                                                         
            });

            hubConn.On<User>("LeaveLobby", (loggedUser) =>
            {               
                MessagingCenter.Send(this, "leftLobby", $"User {loggedUser.Username} has left the lobby."); 
            });

            hubConn.On<User>("StartGame", async (gameMaster) =>
            {
                MessagingCenter.Send(this, "sessionStart", hubConn.ConnectionId.ToString());
                await hubConn.StopAsync();
            });
        }
      
        private async Task StartSession(User user)
        {
            await hubConn.InvokeAsync("StartGame", user);
            await hubConn.StopAsync();
        }

        private async Task ConnectToLobby(User user)
        {                     
            await hubConn.StartAsync();
            await hubConn.InvokeAsync("JoinLobby", user);
        }

        private async Task DisconnectFromLobby(User user)
        {           
            await hubConn.InvokeAsync("LeaveLobby", user);
            await hubConn.StopAsync();
            MessagingCenter.Send(this, "exitApp", "OK");
        }

        private async Task SaveLobby(Lobby lobby)
        {
            Lobby = lobby;
            await lobbyStore.AddLobby(Lobby);         
        }
    }
}
