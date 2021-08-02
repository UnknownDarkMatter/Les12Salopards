using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public class RealisationContrat : IRealisationContrat
    {
        public ICondamné Condamné { get; set; }
        public EtatPersonne ComdamnéEtat { get; set; }
        public IContrat Contrat { get; set; }
        public EtatCondamnation EtatCondamnation { get; set; }

        public RealisationContrat(IContrat contrat, ICondamné condamné, EtatPersonne comdamnéEtat, EtatCondamnation etatCondamnation)
        {
            Condamné = condamné;
            ComdamnéEtat = comdamnéEtat;
            Contrat = contrat;
            EtatCondamnation = etatCondamnation;
        }
    }
}
