using Les12Salopards.Business.PrimeRules;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public class RegleAppliqueCapitalInitial : PrimeRegleDecorateur
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegleAppliqueCapitalInitial));

        private readonly decimal _capitalInitial;

        public RegleAppliqueCapitalInitial(decimal capitalInitial, IPrimeRegle primeRegleContenue):
            base(primeRegleContenue)
        {
            _capitalInitial = capitalInitial;
        }

        public override decimal CalculerPrime(decimal primeExistante)
        {
            IsSatisfied = true;
            log.Debug($"CalculerPrime : Ajout du capital initial de {_capitalInitial} à la prime de {primeExistante}");
            return base.CalculerPrime(primeExistante + _capitalInitial);
        }
    }
}
