using Les12Salopards.Business.PrimeRegles;
using Les12Salopards.Business.PrimeRules;
using Les12Salopards.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Business.Infrastructure
{
    public class PrimeReglesFabrique
    {
        public IPrimeRegle FabriqueToutesLesRegles(
            decimal pourcentageVerséCondamnéEchappé,
            decimal budgetMax,
            ISalopardRealisationContrat salopardRealisationContrat,
            List<ISalopardRealisationContrat> toutesLesRealisationsContrats,
            TrancheMajoration<int> tranchesMajorationNombreCondamnésApportés,
            TrancheMajoration<int> tranchesMajorationTousVivants)
        {
            IPrimeRegle derniereRegle = new RegleInitiale();

            derniereRegle = new RegleBudgetMax(budgetMax, salopardRealisationContrat, pourcentageVerséCondamnéEchappé,
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants,
                toutesLesRealisationsContrats, derniereRegle);

            derniereRegle = FabriqueReglesParSalopard(salopardRealisationContrat, 
                pourcentageVerséCondamnéEchappé,
                tranchesMajorationNombreCondamnésApportés,
                tranchesMajorationTousVivants, derniereRegle);

            return derniereRegle;
        }

        public IPrimeRegle FabriqueReglesParSalopard(
            ISalopardRealisationContrat salopardRealisationContrat,
            decimal pourcentageVerséCondamnéEchappé,
            TrancheMajoration<int> tranchesMajorationNombreCondamnésApportés,
            TrancheMajoration<int> tranchesMajorationTousVivants,
            IPrimeRegle derniereRegle)
        {
            if(derniereRegle == null)
            {
                derniereRegle = new RegleInitiale();
            }

            derniereRegle = new RegleMajorationPlusieursContratsSatisfaits(
                salopardRealisationContrat.RealisationContrats,
                tranchesMajorationNombreCondamnésApportés,
                derniereRegle);

            derniereRegle = new RegleMajorationTousVivants(salopardRealisationContrat.RealisationContrats,
                tranchesMajorationTousVivants, derniereRegle);

            foreach (var realisationContrat in salopardRealisationContrat.RealisationContrats)
            {
                derniereRegle = new RegleMinorationCondamnéEchappé(
                    pourcentageVerséCondamnéEchappé, realisationContrat, derniereRegle);

                derniereRegle = new RegleRealisationContrat(realisationContrat, derniereRegle);
            }
            derniereRegle = new RegleAppliqueCapitalInitial(salopardRealisationContrat.Salopard.CapitalInitial, derniereRegle);
            return derniereRegle;
        }
    }
}
