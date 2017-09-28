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

		

        public void Save() {
            
            var transaction = new Transaction
            {
                Comment = Comment,
                Date = DateTime,
                Value = Amount
            };
            var success = DebtsManager.AddDebt(new Debt{ Name = Name, Transactions = new List<Transaction> { transaction} } );
            if (!success) {
                ShowAlert();
                return;
            }
            UpdateMainScreen();
            NavigationService.GoBack();
        }

        private void UpdateMainScreen()
        {
            throw new NotImplementedException();
        }

        private void ShowAlert()
        {
			var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
			alert
				 .SetMessage(Resource.String.merge_rename_descr)
                .SetPositiveButton(Resource.String.rename, (sender, e) => Rename())
                .SetNegativeButton(Resource.String.merge, (sender, e) => Merge());

			alert.Create().Show();
        }

        void Rename() { 
            
        }

        void Merge() { 
            
        }
    }
       
}
