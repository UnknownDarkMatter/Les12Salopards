using Les12Salopards.Business.PrimeRules;
using Les12Salopards.Entities;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public class RegleRealisationContrat : PrimeRegleDecorateur
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegleRealisationContrat));

        private readonly IRealisationContrat _realisationContrat;

        public decimal MontantRealisationContrat { get; set; }

        public RegleRealisationContrat(IRealisationContrat realisationContrat, IPrimeRegle primeRegleContenue) :
            base(primeRegleContenue)
        {
            _realisationContrat = realisationContrat;
            MontantRealisationContrat = 0;
        }

        public override decimal CalculerPrime(decimal primeExistante)
        {
            decimal montant;
            switch (_realisationContrat.ComdamnéEtat)
            {
                case EtatPersonne.Mort:
                    {
                        montant = _realisationContrat.Contrat.MiseAPrixMort;
                        IsSatisfied = true;
                        break;
                    }
                case EtatPersonne.Vivant:
                    {
                        montant = _realisationContrat.Contrat.MiseAPrixVivant;
                        IsSatisfied = true;
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("_realisationContrat.ComdamnéEtat");
                    }
            }
            log.Debug($"CalculerPrime : ajout de {montant} car l'état du condamné est {_realisationContrat.ComdamnéEtat}");
            MontantRealisationContrat = montant;
            return base.CalculerPrime(primeExistante + montant);
        }

    }
}
