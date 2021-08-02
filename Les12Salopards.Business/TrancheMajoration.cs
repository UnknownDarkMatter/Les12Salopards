using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Les12Salopards.Business
{
    public class TrancheMajoration<Key> where Key : IComparable
    {
        public List<Tuple<Key, decimal>> Tranches { get; set; }

        public TrancheMajoration()
        {
            Tranches = new List<Tuple<Key, decimal>>();
        }

        public void AjouteMajoration(Key tranche, decimal majoration)
        {
            Tranches.Add(new Tuple<Key, decimal>(tranche, majoration));
        }

        public decimal GetMajoration(Key key)
        {
            var tranche = Tranches.OrderByDescending(m => m.Item1)
                .Where(m => IsKey1GreaterThanKey2(key, m.Item1))
                .FirstOrDefault();
            if (tranche == null) return 0;

            return tranche.Item2;
        }

        private bool IsKey1GreaterThanKey2(Key key1, Key key2)
        {
            return key1.CompareTo(key2) >= 0;
        }
    }
}
