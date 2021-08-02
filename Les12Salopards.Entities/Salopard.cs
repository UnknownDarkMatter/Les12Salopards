using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public class Salopard : ISalopard
    {
        public string Nom { get; set; }

        public List<IPrime> PrimesRealisées { get; set; }

        public decimal CapitalInitial { get; set; }

        public Salopard(string nom, decimal capitalInitial)
        {
            Nom = nom;
            CapitalInitial = capitalInitial;
            PrimesRealisées = new List<IPrime>();
        }

        public void AjoutePrimeRealisée(IPrime prime)
        {
            PrimesRealisées.Add(prime);
        }

        public override string ToString()
        {
            return Nom;
        }
    }
}
