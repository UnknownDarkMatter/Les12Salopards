using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public class SalopartRealisationContrat : ISalopardRealisationContrat
    {
        public ISalopard Salopard { get; set; }
        public List<IRealisationContrat> RealisationContrats { get; set; }

        public SalopartRealisationContrat(ISalopard salopard, List<IRealisationContrat> realisationContrats)
        {
            Salopard = salopard;
            RealisationContrats = realisationContrats;
        }
    }
}
