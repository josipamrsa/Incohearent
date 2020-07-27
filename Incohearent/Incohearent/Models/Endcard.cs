using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace Incohearent.Models
{
    public class Endcard
    {       
        public int SessionId { get; set; }
        public int PhraseId { get; set; }
        public int UserId { get; set; }
        public bool Guessed { get; set; }
        public int RoundNo { get; set; }
        public int RoundTime { get; set; } // možda radije Time
        public Endcard()
        {
            
        }
    }

    public class Points : User
    {
        public int PointsWon { get; set; }
        public bool IsGameMaster { get; set; }
        public Points() { }
        public Points(int points, string userAddr, string username, bool gm)
        {
            PointsWon = points;
            this.PrivateAddress = userAddr;
            this.Username = username;
            IsGameMaster = gm;
        }
    }
}
