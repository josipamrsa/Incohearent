using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Models
{
    public class Phrase
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PhraseId { get; set; }
        public string PhraseGenerated { get; set; }
        public List<string> PhraseDissect { get; set; }
        public List<string> PhrasePhonetic { get; set; }

        public Phrase()
        {

        }
    }
}
