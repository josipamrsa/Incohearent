using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.ViewModels
{
    public class LobbyViewModel : IncohearentBaseViewModel
    {
        public string Ip { get; set; }       

        public LobbyViewModel() { }
        public LobbyViewModel(Lobby lobby) { }

        private string lobbyUserPublicIp;
        private string lobbyUserPrivateIp;
        private int userId;
        private bool isActive;

        public string LobbyUserPublicIp { 
            get => lobbyUserPublicIp;
            set {
                SetValue(ref lobbyUserPublicIp, value);
                OnPropertyChanged(nameof(LobbyUserPublicIp));
            }
        }
        public string LobbyUserPrivateIp { 
            get => lobbyUserPrivateIp;
            set
            {
                SetValue(ref lobbyUserPrivateIp, value);
                OnPropertyChanged(nameof(LobbyUserPrivateIp));
            }
        }
        public int UserId { 
            get => userId;
            set
            {
                SetValue(ref userId, value);
                OnPropertyChanged(nameof(UserId));
            }
        }
        public bool IsActive { 
            get => isActive;
            set
            {
                SetValue(ref isActive, value);
                OnPropertyChanged(nameof(IsActive));
            }
        }
    }
}
