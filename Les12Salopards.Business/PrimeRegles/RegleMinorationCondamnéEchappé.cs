using Les12Salopards.Business.PrimeRules;
using Les12Salopards.Entities;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public class RegleMinorationCondamnéEchappé : PrimeRegleDecorateur
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegleMinorationCondamnéEchappé));

        private readonly IRealisationContrat _realisationContrat;
        private readonly decimal _pourcentageVerséCondamnéEchappé;

        public RegleMinorationCondamnéEchappé(decimal pourcentageVerséCondamnéEchappé, IRealisationContrat realisationContrat, IPrimeRegle primeRegleContenue) :
            base(primeRegleContenue)
        {
            _pourcentageVerséCondamnéEchappé = pourcentageVerséCondamnéEchappé;
            _realisationContrat = realisationContrat;
        }

        public override decimal CalculerPrime(decimal primeExistante)
        {
            decimal tauxResteApresMinotation;
            switch (_realisationContrat.EtatCondamnation)
            {
                case EtatCondamnation.PeineApplicable:
                    {
                        tauxResteApresMinotation = 1;
                        break;
                    }
                case EtatCondamnation.CondamnéEchappé:
                    {
                        tauxResteApresMinotation = _pourcentageVerséCondamnéEchappé;
                        IsSatisfied = true;
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("_realisationContrat.EtatCondamnation");
                    }
            }
            var primeRealisationContrat = RechercheRegle<RegleRealisationContrat>() as RegleRealisationContrat;
            decimal minoration = primeRealisationContrat.MontantRealisationContrat * (1 - tauxResteApresMinotation);
            log.Debug($"CalculerPrime : application du taux de {tauxResteApresMinotation.ToString("0.##")} sur la realisation contrat de {primeRealisationContrat.MontantRealisationContrat} car l'état de condamnation est {_realisationContrat.EtatCondamnation}");
            return base.CalculerPrime(primeExistante - minoration);
        }

    }
}
