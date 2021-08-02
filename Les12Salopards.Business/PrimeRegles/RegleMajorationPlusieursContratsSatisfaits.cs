using Les12Salopards.Business.PrimeRules;
using Les12Salopards.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public class RegleMajorationPlusieursContratsSatisfaits : PrimeRegleDecorateur
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegleMajorationPlusieursContratsSatisfaits));

        private readonly List<IRealisationContrat> _realisationContrats;
        private readonly TrancheMajoration<int> _tranchesMajorationNombreCondamnésApportés;
        public RegleMajorationPlusieursContratsSatisfaits(List<IRealisationContrat> realisationContrats,
            TrancheMajoration<int> tranchesMajorationNombreCondamnésApportés,
            IPrimeRegle primeRegleContenue)
            : base(primeRegleContenue)
        {
            _realisationContrats = realisationContrats;
            _tranchesMajorationNombreCondamnésApportés = tranchesMajorationNombreCondamnésApportés;
        }

        public override decimal CalculerPrime(decimal primeExistante)
        {
            decimal majoration = 0;
            var regleMajorationTousvivants = RechercheRegle<RegleMajorationTousVivants>() as RegleMajorationTousVivants;
            if (!regleMajorationTousvivants.IsSatisfied)
            {
                majoration = _tranchesMajorationNombreCondamnésApportés.GetMajoration(_realisationContrats.Count);
                IsSatisfied = majoration != 0;
            }
            log.Debug($"CalculerPrime : application majoration de {majoration} car {_realisationContrats.Count} condamnés apportés");
            return base.CalculerPrime(primeExistante + majoration);
        }
    }
}
