using System;
using System.Collections.Generic;
using System.Linq;
using debtsTracker.Managers;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.Entities
{
    public class Debt : IDisposable
    {
        public string Name { get; set;}
        public List<Transaction> Transactions { get; set; } = new List<Transaction> ();

        public bool ToMe = false;
        public double Value { 
            get {
                return Transactions.Sum (item => item.Value);
            }
        }

		DebtsManager _debtsManager;
		DebtsManager DebtsManager
		{
			get
			{
				return _debtsManager ?? (_debtsManager = ServiceLocator.Current.GetInstance<DebtsManager>());
			}
		}


        public void Dispose()
        {
            DebtsManager.RemoveDebt(Name);
        }
    }
}
