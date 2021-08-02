using Les12Salopards.Business.Infrastructure;
using Les12Salopards.Entities;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;

namespace Les12Salopards.Business
{
    public class PrimeCalculator : IPrimeCalculator
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PrimeCalculator));

        public decimal PourcentageVerséCondamnéEchappé { get; set; }
        public decimal BudgetMax { get; set; }
        public TrancheMajoration<int> TranchesMajorationNombreCondamnésApportés { get; set; }
        public TrancheMajoration<int> TranchesMajorationTousVivants { get; set; }

        public PrimeCalculator(decimal pourcentageVerséCondamnéEchappé, decimal budgetMax,
            TrancheMajoration<int> tranchesMajorationNombreCondamnésApportés,
            TrancheMajoration<int> tranchesMajorationTousVivants)
        {
            BudgetMax = budgetMax;
            PourcentageVerséCondamnéEchappé = pourcentageVerséCondamnéEchappé;
            TranchesMajorationNombreCondamnésApportés = tranchesMajorationNombreCondamnésApportés;
            TranchesMajorationTousVivants = tranchesMajorationTousVivants;
        }

        public IPrime CalculatePrime(ISalopardRealisationContrat salopardRealisationContrat,
            List<ISalopardRealisationContrat> toutesLesRealisationsContrats)
        {
            log.Debug("CalculatePrime - BEGIN");
            var fabriquePrimeRegles = new PrimeReglesFabrique();
            var primeRegles = fabriquePrimeRegles.FabriqueToutesLesRegles(PourcentageVerséCondamnéEchappé, BudgetMax,
                salopardRealisationContrat, toutesLesRealisationsContrats,
                TranchesMajorationNombreCondamnésApportés, TranchesMajorationTousVivants);
            var primeMontant = primeRegles.CalculerPrime(0);
            return new Prime(primeMontant);
        }
    }
}
