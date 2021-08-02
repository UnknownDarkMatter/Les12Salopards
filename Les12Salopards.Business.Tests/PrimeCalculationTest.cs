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
            log.Debug("D�marrage des tests ...");
        }

        [Test]
        public void simple_recompense_condamn�_mort()
        {
            log.Debug("TEST : simple_recompense_condamn�_mort ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn� = new Condamn�("Fredo");
            decimal miseAPrixVivant = 200;
            decimal miseAPrixMort = 100;
            var contrat = new Contrat(condamn�, miseAPrixVivant, miseAPrixMort);
            var realisationContrat = new RealisationContrat(contrat, condamn�, 
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);
            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax, 
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);
            
            var realisationContratSalopard = new SalopartRealisationContrat(salopard, 
                new List<IRealisationContrat>() { realisationContrat });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            Assert.AreEqual(miseAPrixMort, prime.Montant);
        }

        [Test]
        public void simple_recompense_condamn�_mort_avec_capital_initial_negatif()
        {
            log.Debug("TEST : simple_recompense_condamn�_mort_avec_capital_initial_negatif ...");
            //Setup
            decimal capitalInitial = -100;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn� = new Condamn�("Fredo");
            decimal miseAPrixVivant = 200;
            decimal miseAPrixMort = 100;
            var contrat = new Contrat(condamn�, miseAPrixVivant, miseAPrixMort);
            var realisationContrat = new RealisationContrat(contrat, condamn�,
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);
            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax, tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard, 
                new List<IRealisationContrat>() { realisationContrat });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, 
                new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            Assert.AreEqual(capitalInitial + miseAPrixMort, prime.Montant);
        }

        [Test]
        public void simple_recompense_condamn�_attendu_mort_mais_�chapp�()
        {
            log.Debug("TEST : simple_recompense_condamn�_attendu_mort_mais_�chapp� ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn� = new Condamn�("Fredo");
            decimal miseAPrixVivant = 200;
            decimal miseAPrixMort = 100;
            var contrat = new Contrat(condamn�, miseAPrixVivant, miseAPrixMort);
            var realisationContrat = new RealisationContrat(contrat, condamn�, 
                EtatPersonne.Mort, EtatCondamnation.Condamn�Echapp�);
            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax, 
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard,
                new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            Assert.AreEqual(miseAPrixMort * primeCalculator.PourcentageVers�Condamn�Echapp�, 
                prime.Montant);
        }


        [Test]
        public void recompense_major�e_car_deux_contrats_satisfaits_en_m�me_temps()
        {
            log.Debug("TEST : recompense_major�e_car_deux_contrats_satisfaits_en_m�me_temps ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn�1 = new Condamn�("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamn�1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamn�1,
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);

            var condamn�2 = new Condamn�("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamn�2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamn�2, 
                EtatPersonne.Mort, EtatCondamnation.PeineApplicable);

            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax,
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard, 
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, 
                new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamn�sApport�s.GetMajoration(2);
            decimal expectedPrime = miseAPrixMort1 + miseAPrixMort2 + majoration;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }


        [Test]
        public void recompense_deux_contrats_satisfaits_en_m�me_temps_dont_un_echap�()
        {
            log.Debug("TEST : recompense_deux_contrats_satisfaits_en_m�me_temps_dont_un_echap� ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn�1 = new Condamn�("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamn�1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamn�1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamn�2 = new Condamn�("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamn�2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamn�2,
                EtatPersonne.Mort, EtatCondamnation.Condamn�Echapp�);

            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax,
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamn�sApport�s.GetMajoration(2);
            decimal expectedPrime = miseAPrixVivant1
                + (miseAPrixMort2 * pourcentageVers�Condamn�Echapp�) 
                + majoration;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }


        [Test]
        public void depassement_budget_max_deux_contrats_satisfaits_en_m�me_temps_dont_un_echap�()
        {
            log.Debug("TEST : depassement_budget_max_deux_contrats_satisfaits_en_m�me_temps_dont_un_echap� ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 300;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn�1 = new Condamn�("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamn�1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamn�1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamn�2 = new Condamn�("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamn�2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamn�2,
                EtatPersonne.Mort, EtatCondamnation.Condamn�Echapp�);

            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax,
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamn�sApport�s.GetMajoration(2);
            decimal expectedPrime = miseAPrixVivant1
                + (miseAPrixMort2 * pourcentageVers�Condamn�Echapp�)
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
            var condamn�1 = new Condamn�("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamn�1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamn�1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamn�2 = new Condamn�("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamn�2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamn�2,
                EtatPersonne.Mort, EtatCondamnation.Condamn�Echapp�);

            //////////////// salopard 1 : Antonio ////////////////
            decimal capitalInitial2 = -20;
            var salopard2 = new Salopard("Al Capone ", capitalInitial2);
            var condamn�3 = new Condamn�("Lucky");
            decimal miseAPrixVivant3 = 100;
            decimal miseAPrixMort3 = 200;
            var contrat3 = new Contrat(condamn�3, miseAPrixVivant3, miseAPrixMort3);
            var realisationContrat3 = new RealisationContrat(contrat3, condamn�3,
                EtatPersonne.Vivant, EtatCondamnation.Condamn�Echapp�);


            //////////////// param�trage PrimeCalculator ////////////////
            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax,
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

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
            //on ne peut pas d�passer le budget maximum auquel cas l'ensemble des primes sont revues � 
            //la baisse au prorata de leur part par rapport � la somme des primes
            //de sorte que la somme fasse exactement le budget maximum
            decimal majorationSalopard1 = tranchesMajorationNombreCondamn�sApport�s.GetMajoration(2);
            decimal expectedPrimeSalopard1 = capitalInitial1 + miseAPrixVivant1
                + (miseAPrixMort2 * pourcentageVers�Condamn�Echapp�)
                + majorationSalopard1;
            decimal expectedPrimeSalopard2 = capitalInitial2 
                + (miseAPrixVivant3 * pourcentageVers�Condamn�Echapp�);
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
        public void majoration_tous_vivants_et_non_echap�s_prends_le_pas_sur_majoration_nombre_condamn�s()
        {
            log.Debug("TEST : majoration_tous_vivants_et_non_echap�s_prends_le_pas_sur_majoration_nombre_condamn�s ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn�1 = new Condamn�("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamn�1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamn�1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamn�2 = new Condamn�("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamn�2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamn�2,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax,
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

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
        public void majoration_nombre_condamn�s_prends_le_pas_sur_tous_vivants_avec_echap�s()
        {
            log.Debug("TEST : majoration_nombre_condamn�s_prends_le_pas_sur_tous_vivants_avec_echap�s ...");
            //Setup
            decimal capitalInitial = 0;
            decimal budgetMax = 99999;
            var salopard = new Salopard("Antonio", capitalInitial);
            var condamn�1 = new Condamn�("Fredo");
            decimal miseAPrixVivant1 = 200;
            decimal miseAPrixMort1 = 100;
            var contrat1 = new Contrat(condamn�1, miseAPrixVivant1, miseAPrixMort1);
            var realisationContrat1 = new RealisationContrat(contrat1, condamn�1,
                EtatPersonne.Vivant, EtatCondamnation.PeineApplicable);

            var condamn�2 = new Condamn�("Toni");
            decimal miseAPrixVivant2 = 300;
            decimal miseAPrixMort2 = 200;
            var contrat2 = new Contrat(condamn�2, miseAPrixVivant2, miseAPrixMort2);
            var realisationContrat2 = new RealisationContrat(contrat2, condamn�2,
                EtatPersonne.Vivant, EtatCondamnation.Condamn�Echapp�);

            decimal pourcentageVers�Condamn�Echapp� = 0.70M;
            var tranchesMajorationNombreCondamn�sApport�s = new TrancheMajoration<int>();
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(2, 300);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(3, 200);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(4, 100);
            tranchesMajorationNombreCondamn�sApport�s.AjouteMajoration(5, 0);
            var tranchesMajorationTousVivants = new TrancheMajoration<int>();
            tranchesMajorationTousVivants.AjouteMajoration(2, 400);
            tranchesMajorationTousVivants.AjouteMajoration(3, 300);
            tranchesMajorationTousVivants.AjouteMajoration(4, 200);
            tranchesMajorationTousVivants.AjouteMajoration(5, 100);
            tranchesMajorationTousVivants.AjouteMajoration(6, 0);
            var primeCalculator = new PrimeCalculator(pourcentageVers�Condamn�Echapp�, budgetMax,
                tranchesMajorationNombreCondamn�sApport�s, tranchesMajorationTousVivants);

            var realisationContratSalopard = new SalopartRealisationContrat(salopard,
                new List<IRealisationContrat>() { realisationContrat1, realisationContrat2 });

            //Exercise
            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });

            //Verify
            decimal majoration = tranchesMajorationNombreCondamn�sApport�s.GetMajoration(2);
            decimal expectedPrime = miseAPrixVivant1
                + (miseAPrixVivant2 * pourcentageVers�Condamn�Echapp�)
                + majoration;
            Assert.AreEqual(expectedPrime, prime.Montant);
        }

    }
}