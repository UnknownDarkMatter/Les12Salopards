using Les12Salopards.Business;
using Les12Salopards.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace Les12Salopards.Business.Tests
{
    public class TrancheMajorationTest
    {

        [Test]
        public void avant_majoration()
        {
            //Setup
            var tranchesajoration = new TrancheMajoration<int>();
            tranchesajoration.AjouteMajoration(2, 200);
            tranchesajoration.AjouteMajoration(3, 300);
            tranchesajoration.AjouteMajoration(7, 700);

            //Exercise
            var majoration = tranchesajoration.GetMajoration(1);

            //Verify
            Assert.AreEqual(0, majoration);
        }

        [Test]
        public void egal_tranche_majoration_1()
        {
            //Setup
            var tranchesajoration = new TrancheMajoration<int>();
            tranchesajoration.AjouteMajoration(2, 200);
            tranchesajoration.AjouteMajoration(3, 300);
            tranchesajoration.AjouteMajoration(7, 700);

            //Exercise
            var majoration = tranchesajoration.GetMajoration(2);

            //Verify
            Assert.AreEqual(200, majoration);
        }

        [Test]
        public void egal_tranche_majoration_2()
        {
            //Setup
            var tranchesajoration = new TrancheMajoration<int>();
            tranchesajoration.AjouteMajoration(2, 200);
            tranchesajoration.AjouteMajoration(3, 300);
            tranchesajoration.AjouteMajoration(7, 700);

            //Exercise
            var majoration = tranchesajoration.GetMajoration(7);

            //Verify
            Assert.AreEqual(700, majoration);
        }

        [Test]
        public void entre_tranche_majoration()
        {
            //Setup
            var tranchesajoration = new TrancheMajoration<int>();
            tranchesajoration.AjouteMajoration(2, 200);
            tranchesajoration.AjouteMajoration(3, 300);
            tranchesajoration.AjouteMajoration(7, 700);

            //Exercise
            var majoration = tranchesajoration.GetMajoration(5);

            //Verify
            Assert.AreEqual(300, majoration);
        }


        [Test]
        public void apres_tranche_majoration()
        {
            //Setup
            var tranchesajoration = new TrancheMajoration<int>();
            tranchesajoration.AjouteMajoration(2, 200);
            tranchesajoration.AjouteMajoration(3, 300);
            tranchesajoration.AjouteMajoration(7, 700);

            //Exercise
            var majoration = tranchesajoration.GetMajoration(8);

            //Verify
            Assert.AreEqual(700, majoration);
        }


    }
}