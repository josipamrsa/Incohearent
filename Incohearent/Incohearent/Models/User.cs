using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;

namespace Incohearent.Models
{
    public class User
    {       
        [PrimaryKey]
        [AutoIncrement]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PrivateAddress { get; set; }
        public string PublicAddress { get; set; }
        public bool LoggedIn { get; set; }

        public User() { }
        public User(string username, string publAddr, string privAddr, bool log)
        {
            this.Username = username;
            this.PrivateAddress = privAddr;
            this.PublicAddress = publAddr;
            this.LoggedIn = log;
        }

        
    }
}
