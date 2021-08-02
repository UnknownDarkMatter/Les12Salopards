using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.PrimeRules
{
    public interface IPrimeRegle
    {
        IPrimeRegle PreviousRegle { get; set; }
        decimal CalculerPrime(decimal primeExistante);
        IPrimeRegle RechercheRegle<T>();
        bool IsSatisfied { get; set; }
    }
}
