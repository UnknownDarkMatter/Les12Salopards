using Les12Salopards.Business.PrimeRules;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public class RegleInitiale : IPrimeRegle
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegleInitiale));
        public IPrimeRegle PreviousRegle { get; set; } = null;
        public bool IsSatisfied { get; set; }

        public decimal CalculerPrime(decimal primeExistante)
        {
            IsSatisfied = true;
            log.Debug($"RegleInitiale : retourne la prime de {primeExistante} inchangée");
            return primeExistante;
        }
        public IPrimeRegle RechercheRegle<T>()
        {
            if (this.GetType() != typeof(T)) { return default(IPrimeRegle); }

            return this;
        }

    }
}
