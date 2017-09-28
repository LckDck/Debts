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

        public bool AddDebt (Debt debt) {
            if (Debts.ContainsKey(debt.Name)) return false;

            Debts.Add(debt.Name, debt);
            Storage.SaveDebts(Debts);
            return true;
        }

        public void RemoveDebt(string name) {
            if (!Debts.ContainsKey(name)) return;

            Debts.Remove(name);
            Storage.SaveDebts(Debts);
        }
    }
}
