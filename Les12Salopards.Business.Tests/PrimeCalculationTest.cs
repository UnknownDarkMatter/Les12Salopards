using Les12Salopards.Business;
using Les12Salopards.Entities;
using log4net;
using log4net.Config;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Les12Salopards.Business.Tests
{
    public class PrimeCalculationTest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PrimeCalculationTest));

        [SetUp]
        public void SetupTests()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo($"{Environment.CurrentDirectory}\\log4net.config"));
            log.Debug("Démarrage des tests ...");
        }

        [Test]
        public void simple_recompense_condamné_mort()
        {
            log.Debug("TEST : simple_recompense_condamné_mort ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné = new Condamné("Fredo");
            decimal miseAPrixVivant = 200;
            decimal miseAPrixMort = 100;
            var contrat = new Contrat(condamné, miseAPrixVivant, miseAPrixMort);
            var realisationContrat = new RealisationContrat(contrat, condamné, 
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);
            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax, 
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);
            
            var realisationContratSalopard = new SalopartRealisationContrat(salopard, 
                new List<IRealisationContrat>() { realisationContrat });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            Assert.AreEqual(miseAPrixMort, prime.Montant);
        }

        [Test]
        public void simple_recompense_condamné_mort_avec_capital_initial_negatif()
        {
            log.Debug("TEST : simple_recompense_condamné_mort_avec_capital_initial_negatif ...");
            //Setup
            decimal capitalInitial = -100;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné = new Condamné("Fredo");
            decimal miseAPrixVivant = 200;
            decimal miseAPrixMort = 100;
            var contrat = new Contrat(condamné, miseAPrixVivant, miseAPrixMort);
            var realisationContrat = new RealisationContrat(contrat, condamné,
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);
            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax, tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard, 
                new List<IRealisationContrat>() { realisationContrat });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, 
                new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            Assert.AreEqual(capitalInitial + miseAPrixMort, prime.Montant);
        }

        [Test]
        public void simple_recompense_condamné_attendu_mort_mais_échappé()
        {
            log.Debug("TEST : simple_recompense_condamné_attendu_mort_mais_échappé ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné = new Condamné("Fredo");
            decimal miseAPrixVivant = 200;
            decimal miseAPrixMort = 100;
            var contrat = new Contrat(condamné, miseAPrixVivant, miseAPrixMort);
            var realisationContrat = new RealisationContrat(contrat, condamné, 
                EtatPersonne.Mort, EtatCondamnation.CondamnéEchappé);
            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax, 
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard,
                new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            Assert.AreEqual(miseAPrixMort * primeCalculator.PourcentageVerséCondamnéEchappé, 
                prime.Montant);
        }


        [Test]
        public void recompense_majorée_car_deux_contrats_satisfaits_en_même_temps()
        {
            log.Debug("TEST : recompense_majorée_car_deux_contrats_satisfaits_en_même_temps ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné1 = new Condamné("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamné1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamné1,
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);

            var condamné2 = new Condamné("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamné2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamné2, 
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);

            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax,
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard, 
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, 
                new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamnésApportés.GetMajoration(2);
            decimal expectedPrime = miseAPrixMort1 + miseAPrixMort2 + majoration;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }


        [Test]
        public void recompense_deux_contrats_satisfaits_en_même_temps_dont_un_echapé()
        {
            log.Debug("TEST : recompense_deux_contrats_satisfaits_en_même_temps_dont_un_echapé ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné1 = new Condamné("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamné1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamné1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamné2 = new Condamné("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamné2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamné2,
                EtatPersonne.Mort, EtatCondamnation.CondamnéEchappé);

            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax,
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamnésApportés.GetMajoration(2);
            decimal expectedPrime = miseAPrixVivant1
                + (miseAPrixMort2 * pourcentageVerséCondamnéEchappé) 
                + majoration;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }


        [Test]
        public void depassement_budget_max_deux_contrats_satisfaits_en_même_temps_dont_un_echapé()
        {
            log.Debug("TEST : depassement_budget_max_deux_contrats_satisfaits_en_même_temps_dont_un_echapé ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 300;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné1 = new Condamné("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamné1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamné1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamné2 = new Condamné("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamné2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamné2,
                EtatPersonne.Mort, EtatCondamnation.CondamnéEchappé);

            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax,
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamnésApportés.GetMajoration(2);
            decimal expectedPrime = miseAPrixVivant1
                + (miseAPrixMort2 * pourcentageVerséCondamnéEchappé)
                + majoration;
            expectedPrime = budgetMax < expectedPrime ? budgetMax : expectedPrime;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }

        [Test]
        public void depassement_budget_max_deux_salopards()
        {
            log.Debug("TEST : depassement_budget_max_deux_salopards ...");
            //Setup
            decimal budgetMax = 300;
            //////////////// salopard 1 : Antonio ////////////////
            decimal capitalInitial1 = 0;
            var salopard1 = new Salopard("Antonio", capitalInitial1);
            var condamné1 = new Condamné("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamné1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamné1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamné2 = new Condamné("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamné2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamné2,
                EtatPersonne.Mort, EtatCondamnation.CondamnéEchappé);

            //////////////// salopard 1 : Antonio ////////////////
            decimal capitalInitial2 = -20;
            var salopard2 = new Salopard("Al Capone ", capitalInitial2);
            var condamné3 = new Condamné("Lucky");
            decimal miseAPrixVivant3 = 100;
            decimal miseAPrixMort3 = 200;
            var contrat3 = new Contrat(condamné3, miseAPrixVivant3, miseAPrixMort3);
            var realisationContrat3 = new RealisationContrat(contrat3, condamné3,
                EtatPersonne.Vivant, EtatCondamnation.CondamnéEchappé);


            //////////////// paramétrage PrimeCalculator ////////////////
            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax,
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard1 = new SalopartRealisationContrat(salopard1,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });
            var realisationContratSalopard2 = new SalopartRealisationContrat(salopard2,
                new List<IRealisationContrat>() { realisationContrat3 });

            //Exercise
            var primeSalopard1 = primeCalculator.CalculatePrime(
                realisationContratSalopard1, 
                new List<ISalopardRealisationContrat>() { realisationContratSalopard1, realisationContratSalopard2 });
            var primeSalopard2 = primeCalculator.CalculatePrime(
                realisationContratSalopard2,
                new List<ISalopardRealisationContrat>() { realisationContratSalopard1, realisationContratSalopard2 });

            //Verify
            //on ne peut pas dépasser le budget maximum auquel cas l'ensemble des primes sont revues à 
            //la baisse au prorata de leur part par rapport à la somme des primes
            //de sorte que la somme fasse exactement le budget maximum
            decimal majorationSalopard1 = tranchesMajorationNombreCondamnésApportés.GetMajoration(2);
            decimal expectedPrimeSalopard1 = capitalInitial1 + miseAPrixVivant1
                + (miseAPrixMort2 * pourcentageVerséCondamnéEchappé)
                + majorationSalopard1;
            decimal expectedPrimeSalopard2 = capitalInitial2 
                + (miseAPrixVivant3 * pourcentageVerséCondamnéEchappé);
            if(expectedPrimeSalopard1 + expectedPrimeSalopard2 > budgetMax)
            {
                decimal sum = expectedPrimeSalopard1 + expectedPrimeSalopard2;
                expectedPrimeSalopard1 = (300 * expectedPrimeSalopard1) / sum;
                expectedPrimeSalopard2 = (300 * expectedPrimeSalopard2) / sum;
            }
            Assert.AreEqual(expectedPrimeSalopard1, primeSalopard1.Montant);
            Assert.AreEqual(expectedPrimeSalopard2, primeSalopard2.Montant);
        }

        [Test]
        public void majoration_tous_vivants_et_non_echapés_prends_le_pas_sur_majoration_nombre_condamnés()
        {
            log.Debug("TEST : majoration_tous_vivants_et_non_echapés_prends_le_pas_sur_majoration_nombre_condamnés ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné1 = new Condamné("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamné1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamné1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamné2 = new Condamné("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamné2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamné2,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax,
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationTousVivants.GetMajoration(2);
            decimal expectedPrime = miseAPrixVivant1 + miseAPrixVivant2 + majoration;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }


        [Test]
        public void majoration_nombre_condamnés_prends_le_pas_sur_tous_vivants_avec_echapés()
        {
            log.Debug("TEST : majoration_nombre_condamnés_prends_le_pas_sur_tous_vivants_avec_echapés ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamné1 = new Condamné("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamné1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamné1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamné2 = new Condamné("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamné2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamné2,
                EtatPersonne.Vivant, EtatCondamnation.CondamnéEchappé);

            decimal pourcentageVerséCondamnéEchappé = 0.70M;
            var tranchesMajorationNombreCondamnésApportés = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamnésApportés.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVerséCondamnéEchappé, budgetMax,
                tranchesMajorationNombreCondamnésApportés, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamnésApportés.GetMajoration(2);
            decimal expectedPrime = miseAPrixVivant1
                + (miseAPrixVivant2 * pourcentageVerséCondamnéEchappé)
                + majoration;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }

    }
}