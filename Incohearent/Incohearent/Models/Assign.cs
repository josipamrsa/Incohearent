using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Incohearent.Models
{   
    public class Assign
    {
        [PrimaryKey]
        public int AssignId { get; set; } // zapravo userId
        public string SessionId { get; set; }
        public string PhraseId { get; set; }

        public Assign()
        {

        }
    }
}
