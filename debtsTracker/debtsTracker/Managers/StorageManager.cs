using System;
using System.Collections.Generic;
using debtsTracker.Entities;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace debtsTracker.Managers
{
    public class StorageManager
    {
        string DebtsKey = "DebtsKey";

        DroidLocalStorage _localStorage;
        DroidLocalStorage LocalStorage { 
            get {
                return _localStorage ?? (_localStorage = ServiceLocator.Current.GetInstance<DroidLocalStorage>());
            }
        }


        public void SaveDebts(Dictionary<string, Debt> debts) { 
            var str = JsonConvert.SerializeObject(debts);
            LocalStorage.SetStringValue(DebtsKey, str);
        }

        public Dictionary<string, Debt> ReadDebts() {
            var empty = new Dictionary<string, Debt>();
            var readString = LocalStorage.GetStringValue(DebtsKey);
            if (string.IsNullOrEmpty(readString)) return empty;
            var debts = JsonConvert.DeserializeObject<Dictionary<string,Debt>>(readString);
            return debts ?? empty;
        }
    }
}
