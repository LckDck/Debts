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
        public MainViewModel() {
            
        }

        DebtsManager _debtsManager;
        DebtsManager DebtsManager { 
            get {
                return _debtsManager ?? (_debtsManager = ServiceLocator.Current.GetInstance<DebtsManager>());
            }
        }


        public void ShowDetails (Debt debt)
        {
            NavigationService.NavigateTo (Page.HistoryPage, debt);
        }

		InterfaceUpdateManager _interfaceUpdateManager;
        private List<Debt> _debts;

        InterfaceUpdateManager InterfaceUpdateManager
		{
			get
			{
				return _interfaceUpdateManager ?? (_interfaceUpdateManager = ServiceLocator.Current.GetInstance<InterfaceUpdateManager>());
			}
		}

        public List<Debt> GetItems (bool myDebts)
        {
            _debts = DebtsManager.ReadDebts().Select(item => item.Value).ToList();
            return _debts.Where(item => item.ToMe == !myDebts).ToList();
        }

		public int Tab
		{
			get
			{
				return InterfaceUpdateManager.CurrrentTab;
			}
			set
			{
				InterfaceUpdateManager.CurrrentTab = value;
			}
		}


		internal void AddPage ()
        {
            NavigationService.NavigateTo (Page.AddPage);
        }

        public void AddTransaction(int position, bool positive) {
            NavigationService.NavigateTo(Page.AddTransactionPage, _debts[position].Name, positive);
        }
    }
}
