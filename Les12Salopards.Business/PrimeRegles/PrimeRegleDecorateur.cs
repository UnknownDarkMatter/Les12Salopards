using Les12Salopards.Business.PrimeRules;
using Les12Salopards.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public abstract class PrimeRegleDecorateur : IPrimeRegle
    {
        protected IPrimeRegle PrimeContenue;
        public IPrimeRegle PreviousRegle { get; set; }
        public bool IsSatisfied { get; set; }

        public PrimeRegleDecorateur(IPrimeRegle primeContenue)
        {
            primeContenue.PreviousRegle = this;
            PrimeContenue = primeContenue;
            IsSatisfied = false;
        }

        public virtual decimal CalculerPrime(decimal primeExistante)
        {
            return PrimeContenue.CalculerPrime(primeExistante);
        }

        public IPrimeRegle RechercheRegle<T>()
        {
            if (GetType() == typeof(T)) { return this; }
            if (PreviousRegle == null) { return default(IPrimeRegle); }
            if(PreviousRegle.GetType() != typeof(T)) { return PreviousRegle.RechercheRegle<T>(); }

            return PreviousRegle;
        }
    }
}
