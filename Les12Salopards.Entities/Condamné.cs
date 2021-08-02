using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public class Condamné : ICondamné
    {
        public string Nom { get; set; }

        public Condamné(string nom)
        {
            Nom = nom;
        }
        public override string ToString()
        {
            return Nom;
        }

    }
}
