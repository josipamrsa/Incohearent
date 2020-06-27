using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

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

        public User() { }
        public User(string username, string publAddr, string privAddr)
        {
            this.Username = username;
            this.PrivateAddress = privAddr;
            this.PublicAddress = publAddr;
        }

        public bool CheckInformation()
        {
            if (this.Username.Equals(""))
                return false;
            else
                return true;
        }
    }
}
