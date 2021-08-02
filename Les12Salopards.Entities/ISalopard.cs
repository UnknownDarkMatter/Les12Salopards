using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public interface ISalopard
    {
        string Nom { get; set; }

        List<IPrime> PrimesRealisées { get; set; }

        decimal CapitalInitial { get; set; }

        void AjoutePrimeRealisée(IPrime prime);
    }
}
