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
        public int Id { get; set; }
        public string Username { get; set; }
        public string IpAddress { get; set; }

        public User() { }
        public User(string username, string ipAddr)
        {
            this.Username = username;
            this.IpAddress = ipAddr;
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
