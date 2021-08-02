using Les12Salopards.Business.Infrastructure;
using Les12Salopards.Business.PrimeRules;
using Les12Salopards.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.PrimeRegles
{
    public class RegleBudgetMax : PrimeRegleDecorateur
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegleBudgetMax));

        private readonly List<ISalopardRealisationContrat> _salopardRealisationContrats;
        private readonly decimal _budgetMax;
        private readonly ISalopardRealisationContrat _salopardRealisationContrat;
        private readonly decimal _pourcentageVerséCondamnéEchappé;
        private readonly TrancheMajoration<int> _tranchesMajorationNombreCondamnésApportés;
        private readonly TrancheMajoration<int> _tranchesMajorationTousVivants;

        public RegleBudgetMax(decimal budgetMax,
            ISalopardRealisationContrat salopardRealisationContrat,
            decimal pourcentageVerséCondamnéEchappé,
            TrancheMajoration<int> tranchesMajorationNombreCondamnésApportés,
            TrancheMajoration<int> tranchesMajorationTousVivants,
            List<ISalopardRealisationContrat> salopardRealisationContrats,
            IPrimeRegle primeRegleContenue) :
            base (primeRegleContenue)
        {
            _budgetMax = budgetMax;
            _salopardRealisationContrat = salopardRealisationContrat;
            _salopardRealisationContrats = salopardRealisationContrats;
            _pourcentageVerséCondamnéEchappé = pourcentageVerséCondamnéEchappé;
            _tranchesMajorationNombreCondamnésApportés = tranchesMajorationNombreCondamnésApportés;
            _tranchesMajorationTousVivants = tranchesMajorationTousVivants;
        }

        public override decimal CalculerPrime(decimal primeExistante)
        {
            Dictionary<ISalopard, decimal> primesSalopards = CalculatePrimesSalopards();
            decimal sum = CalcumateSommePrimes(primesSalopards);
            decimal primeMinorée = primeExistante;
            if(sum != 0)
            {
                primeMinorée = (_budgetMax * primesSalopards[_salopardRealisationContrat.Salopard]) / sum;
                primeMinorée = primeMinorée < primeExistante ? primeMinorée : primeExistante;
            }
            IsSatisfied = primeMinorée != primeExistante;
            log.Debug($"CalculerPrime : Application de maximum de {_budgetMax} sur la prime de {primeExistante} de {_salopardRealisationContrat.Salopard.Nom}");
            return base.CalculerPrime(primeMinorée);
        }

        private Dictionary<ISalopard, decimal> CalculatePrimesSalopards()
        {
            var reglesFabrique = new PrimeReglesFabrique();
            Dictionary<ISalopard, decimal> primesSalopards = new Dictionary<ISalopard, decimal>();
            foreach (var salopardRealisationContrat in _salopardRealisationContrats)
            {
                var reglesParSalopart = reglesFabrique.FabriqueReglesParSalopard(
                    salopardRealisationContrat, _pourcentageVerséCondamnéEchappé, 
                    _tranchesMajorationNombreCondamnésApportés, _tranchesMajorationTousVivants, null);

                var prime = reglesParSalopart.CalculerPrime(0);
                primesSalopards.Add(salopardRealisationContrat.Salopard, prime);
            }
            return primesSalopards;
        }

        private decimal CalcumateSommePrimes(Dictionary<ISalopard, decimal> primesSalopards)
        {
            decimal sum = 0;
            foreach (var prime in primesSalopards)
            {
                sum += prime.Value;
            }
            return sum;
        }

    }
}
