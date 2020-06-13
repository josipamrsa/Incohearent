using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Data
{
    public interface IPhrases
    {
        // Ovdje će doći metode za generiranje i dodjeljivanje frazi

        // Definiranje frazi (samo trenutno void tip)
        void DefinePhrase();
        void TransformPhrasePieces();
        void ConnectPhonetic();

        // Dodjeljivanje frazi
        void AssignToPlayer();
    }
}
