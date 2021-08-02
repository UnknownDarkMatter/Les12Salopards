using Les12Salopards.Entities;
using System;
using System.Collections.Generic;

namespace Les12Salopards.Business
{
    public interface IPrimeCalculator
    {
        IPrime CalculatePrime(ISalopardRealisationContrat salopardRealisationContrat, 
            List<ISalopardRealisationContrat> toutesLesRealisationsContrats);
    }
}
