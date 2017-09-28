using System;
using System.Collections.Generic;
using System.Linq;
using debtsTracker.Entities;
using debtsTracker.Managers;
using debtsTracker.Utilities;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.ViewModels
{
    public class MainViewModel : BaseVm
    {

        public bool ToMe;

        DebtsManager _debtsManager;
        DebtsManager DebtsManager { 
            get {
                return _debtsManager ?? (_debtsManager = ServiceLocator.Current.GetInstance<DebtsManager>());
            }
        }

        List<Debt> _debts;
        public List<Debt> ResetDebts() {
            _debts = null;
            return _debts = DebtsManager.ReadDebts().Select(item => item.Value).ToList();
        }

        public void ShowDetails (Debt debt)
        {
            NavigationService.NavigateTo (Page.HistoryPage, debt);
        }



        public List<Debt> GetItems (bool myDebts)
        {
            var result = _debts ?? ResetDebts();
            return result.Where(item => item.ToMe == !myDebts).ToList();
        }

        internal void AddPage ()
        {
            NavigationService.NavigateTo (Page.AddPage, ToMe);
        }
    }
}
