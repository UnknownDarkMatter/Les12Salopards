using Les12Salopards.Business.PrimeRules;
using Les12Salopards.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public class RegleMajorationTousVivants : PrimeRegleDecorateur
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegleMajorationTousVivants));

        private readonly TrancheMajoration<int> _tranchesMajorationTousVivants;
        private readonly List<IRealisationContrat> _realisationContrats;

        public RegleMajorationTousVivants(List<IRealisationContrat> realisationContrats,
            TrancheMajoration<int> tranchesMajorationTousVivants,
            IPrimeRegle primeRegleContenue):
            base(primeRegleContenue)
        {
            _realisationContrats = realisationContrats;
            _tranchesMajorationTousVivants = tranchesMajorationTousVivants;
        }

        public override decimal CalculerPrime(decimal primeExistante)
        {
            decimal majoration = 0;
            if(TousCondamnesSontVivantsEtNonEchappés())
            {
                majoration = _tranchesMajorationTousVivants.GetMajoration(_realisationContrats.Count);
                IsSatisfied = majoration != 0;
            }
            log.Debug($"CalculerPrime : application majoration de {majoration} tous les condamnés sont vivants et non échappés");
            return base.CalculerPrime(primeExistante + majoration);
        }

        private bool TousCondamnesSontVivantsEtNonEchappés()
        {
            if(_realisationContrats.Count == 0) { return false; }

            return _realisationContrats.FirstOrDefault(m => m.ComdamnéEtat != EtatPersonne.Vivant 
            || m.EtatCondamnation != EtatCondamnation.PeineApplicable) == null;
        }

    }
}
