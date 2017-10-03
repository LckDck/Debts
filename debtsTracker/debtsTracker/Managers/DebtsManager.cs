using System;
using System.Collections.Generic;
using debtsTracker.Entities;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.Managers
{
    public class DebtsManager
    {
        Dictionary<string, Debt> Debts = new Dictionary<string, Debt>();
        StorageManager _storage;
        StorageManager Storage { 
            get {
                return _storage ?? (_storage = ServiceLocator.Current.GetInstance<StorageManager>());
            }
        }

        public Dictionary<string, Debt> ReadDebts () {
            Debts = Storage.ReadDebts();
            return Debts;
        }

        public bool AddDebt (Debt debt) {
            if (Debts.ContainsKey(debt.Name)) return false;

            Debts.Add(debt.Name, debt);
            Storage.SaveDebts(Debts);
            return true;
        }

		public bool MergeDebt(Debt debt)
		{
            if (!Debts.ContainsKey(debt.Name)) return false;

            var foundDebt = Debts[debt.Name];
            foundDebt.Transactions.AddRange(debt.Transactions);
			Storage.SaveDebts(Debts);
			return true;
		}

        public void RemoveDebt(string name) {
            if (!Debts.ContainsKey(name)) return;

            Debts.Remove(name);
            Storage.SaveDebts(Debts);
        }

        public void RemoveTransaction(Transaction trans) { 
            if (!Debts.ContainsKey(trans.Name)) return;

            var debt = Debts[trans.Name];
            var foundDebt = debt.Transactions.Find(item => item.Date == trans.Date
                                                   && item.Value == trans.Value
                                                   && item.Comment == trans.Comment);
            if (foundDebt != null){ 
                debt.Transactions.Remove(foundDebt);
            }
            Storage.SaveDebts(Debts);
        }

        public Debt GetDebt(string name) {
            return Debts.ContainsKey(name) ? Debts[name] : null;
        }
    }
}
