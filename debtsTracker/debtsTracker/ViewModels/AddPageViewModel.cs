using System;
using System.Collections.Generic;
using debtsTracker.Entities;
using debtsTracker.Managers;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.ViewModels
{
    public class AddPageViewModel : BaseVm
    {
        
        RelayCommand _saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(Save));
            }
        }

        public int Tab { 
            get {
                return InterfaceUpdateManager.CurrrentTab;
            }

            set { 
                InterfaceUpdateManager.CurrrentTab = value;
            }
        }

       

        public string Name { get; set; }
        public string Comment { get; set; }
        public double Amount { get; set; }
        public DateTime DateTime { get; set; }

		DebtsManager _debtsManager;
		DebtsManager DebtsManager
		{
			get
			{
				return _debtsManager ?? (_debtsManager = ServiceLocator.Current.GetInstance<DebtsManager>());
			}
		}

		InterfaceUpdateManager _interfaceUpdateManager;
        InterfaceUpdateManager InterfaceUpdateManager
        {
            get
            {
                return _interfaceUpdateManager ?? (_interfaceUpdateManager = ServiceLocator.Current.GetInstance<InterfaceUpdateManager>());
            }
        }

        

        public void Save() {
            
            var transaction = new Transaction
            {
                Comment = Comment,
                Date = DateTime,
                Name = Name,
                Value = Amount
            };
            var success = DebtsManager.AddDebt(new Debt{ 
                Name = Name,
                ToMe = InterfaceUpdateManager.IsTabToMe,
                Transactions = new List<Transaction> { 
                    transaction} 
                });
            if (!success) {
                ShowAlert();
                return;
            }
            NavigationService.GoBack();
        }

       

        private void ShowAlert()
        {
			var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
			alert
				 .SetMessage(Resource.String.merge_rename_descr)
                .SetNegativeButton(Resource.String.rename, (sender, e) => Rename())
                .SetPositiveButton(Resource.String.merge, (sender, e) => Merge());

			alert.Create().Show();
        }

        void Rename() {
            InterfaceUpdateManager.InvokeNameFocus();
        }

        void Merge() {
			var transaction = new Transaction
			{
				Comment = Comment,
				Date = DateTime,
				Value = Amount,
                Name = Name
			};
            var success = DebtsManager.MergeDebt(new Debt
			{
				Name = Name,
				ToMe = InterfaceUpdateManager.IsTabToMe,
				Transactions = new List<Transaction> {
					transaction}
			});
			if (!success)
			{
				ShowMergeFailedAlert();
				return;
			}
			NavigationService.GoBack();
        }

        private void ShowMergeFailedAlert()
        {
			var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
			alert
				 .SetMessage(Resource.String.merge_failed)
                .SetPositiveButton(Resource.String.rename, (sender, e) => { });

			alert.Create().Show();
        }
    }
       
}
