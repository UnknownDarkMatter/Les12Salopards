using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public class Prime : IPrime
    {
        public decimal Montant { get; set; }

        public Prime(decimal montant)
        {
            Montant = montant;
        }
    }
}
