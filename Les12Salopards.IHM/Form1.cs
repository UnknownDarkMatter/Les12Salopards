using Les12Salopards.Business;
using Les12Salopards.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Les12Salopards.IHM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lblMontantPrime.Text = "";
        }

        private void btnCalculerPrime_Click(object sender, EventArgs e)
        {
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

            var prime = primeCalculator.CalculatePrime(realisationContratSalopard, new List<ISalopardRealisationContrat>() { realisationContratSalopard });
            lblMontantPrime.Text = prime.Montant.ToString("0.##");
        }
    }
}
