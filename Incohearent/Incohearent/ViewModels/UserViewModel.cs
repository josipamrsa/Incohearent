using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.ViewModels
{
    public class UserViewModel : IncohearentBaseViewModel
    {
        public int Id { get; set; }
        
        public UserViewModel() { }
        public UserViewModel(User user)
        {

        }

        private string username;
        public string Username { 
            get => username;
            set { 
                SetValue(ref username, value);
                OnPropertyChanged(nameof(Username));
            }
        }
        
        private string privateAddr;
        
        public string PrivateAddress { 
            get => privateAddr;
            set
            {
                SetValue(ref privateAddr, value);
                OnPropertyChanged(nameof(PrivateAddress));
            }
        }

        private string publicAddr;
        public string PublicAddress { 
            get => publicAddr;
            set
            {
                SetValue(ref publicAddr, value);
                OnPropertyChanged(nameof(PublicAddress));
            }
        }

        private bool loggedIn;
        public bool LoggedIn { 
            get => loggedIn;
            set
            {
                SetValue(ref loggedIn, value);
                OnPropertyChanged(nameof(LoggedIn));
            }
        }     
    }
}
