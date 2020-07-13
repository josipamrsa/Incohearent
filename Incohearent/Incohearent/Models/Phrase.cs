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
        public string PhrasePhonetic { get; set; }

        public Phrase(string gen, string phonetic)
        {
            PhraseGenerated = gen;
            PhrasePhonetic = phonetic;
        }
    }
}
