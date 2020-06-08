using System;
using System.Collections.Generic;
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
}
