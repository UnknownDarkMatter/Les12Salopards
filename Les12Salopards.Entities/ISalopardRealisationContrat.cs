using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public interface ISalopardRealisationContrat
    {
        ISalopard Salopard { get; set; }
        List<IRealisationContrat> RealisationContrats { get; set; }
    }
}
