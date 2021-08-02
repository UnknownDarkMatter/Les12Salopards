using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public class Contrat: IContrat
    {
        public ICondamné Condamné { get; set; }
        public decimal MiseAPrixVivant { get; set; }
        public decimal MiseAPrixMort { get; set; }

        public Contrat(ICondamné condamné, decimal miseAPrixVivant, decimal miseAPrixMort)
        {
            Condamné = condamné;
            MiseAPrixVivant = miseAPrixVivant;
            MiseAPrixMort = miseAPrixMort;
        }
    }
}
