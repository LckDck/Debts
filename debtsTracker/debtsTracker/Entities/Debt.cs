using System;
using System.Collections.Generic;
using System.Linq;

namespace debtsTracker.Entities
{
    public class Debt
    {
        public string Name { get; set;}
        public List<Transaction> Transactions { get; set; } = new List<Transaction> ();

        public double Value { 
            get {
                return Transactions.Sum (item => item.Value);
            }
        }
    }
}
