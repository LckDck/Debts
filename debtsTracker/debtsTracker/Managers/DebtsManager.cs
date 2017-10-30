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

        public bool AddTransaction(Transaction trans)
		{
            if (!Debts.ContainsKey(trans.Name)) return false;

            Debts[trans.Name].Transactions.Add(trans);
			Storage.SaveDebts(Debts);
			return true;
		}


		public bool MergeDebt(Debt debt)
		{
            if (!Debts.ContainsKey(debt.Name)) return false;

            var foundDebt = Debts[debt.Name];

            if (foundDebt.ToMe != debt.ToMe) {
                foreach (var transaction in debt.Transactions) {
                    transaction.Value = - transaction.Value;
                }
            }
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
            var foundTransaction = debt.Transactions.Find(item => item.Date == trans.Date
                                                   && Math.Abs(item.Value - trans.Value) < double.Epsilon
                                                   && item.Comment == trans.Comment);
            if (foundTransaction != null){ 
                debt.Transactions.Remove(foundTransaction);
            }
            Storage.SaveDebts(Debts);
        }

        public Debt GetDebt(string name) {
            return Debts.ContainsKey(name) ? Debts[name] : null;
        }

        public void RenameDebt(string oldName, string newName) { 
            if (!Debts.ContainsKey(oldName)) return;
            var newDebts = new Dictionary<string, Debt>();
            foreach (var debt in Debts) {
                if (debt.Key != oldName)
                {
                    newDebts.Add(debt.Key, debt.Value);
                }
                else {
                    newDebts.Add(newName, debt.Value);
                    debt.Value.Name = newName;
                    foreach (var transaction in debt.Value.Transactions) {
                        transaction.Name = newName;
                    }
                }
            }
            Debts = newDebts;
            Storage.SaveDebts(Debts);
        }
    }
}
