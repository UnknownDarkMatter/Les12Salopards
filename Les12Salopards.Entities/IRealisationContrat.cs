using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public interface IRealisationContrat
    {
        ICondamné Condamné { get; set; }
        EtatPersonne ComdamnéEtat { get; set; }
        IContrat Contrat { get; set; }
        EtatCondamnation EtatCondamnation { get; set; }
    }
}
