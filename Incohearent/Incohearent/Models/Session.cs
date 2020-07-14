using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Models
{   
    public class Session
    {
        [PrimaryKey]
        [AutoIncrement]
        public int SessionId { get; set; }
        //public string GameType { get; set; }
        public int UserId { get; set; }
        public int RoundNum { get; set; }
        public int PlayerNum { get; set; }

        public Session() { }
        public Session(int id, int rn, int pn)
        {
            UserId = id;
            RoundNum = rn;
            PlayerNum = pn;
        }
    }
}
