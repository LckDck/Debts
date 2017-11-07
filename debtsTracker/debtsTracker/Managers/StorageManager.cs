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
        string BoughtKey = "Bought";
        string Tab = "Tab";

        DroidLocalStorage _localStorage;
        DroidLocalStorage LocalStorage { 
            get {
                return _localStorage ?? (_localStorage = ServiceLocator.Current.GetInstance<DroidLocalStorage>());
            }
        }

        bool? _bought = null;
        public bool Bought
        {
            set
            {
                _bought = value;
                LocalStorage.SetBoolValue(BoughtKey, _bought.Value);
            }

            get {
                if (!_bought.HasValue) {
                    _bought = LocalStorage.GetBoolValue(BoughtKey);
                }
                return _bought.Value;
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

        public void SaveTab(int tab) { 
            LocalStorage.SetIntValue(Tab, tab);
        }

        public int GetTab()
        {
            return LocalStorage.GetIntValue(Tab);
        }
    }
}
