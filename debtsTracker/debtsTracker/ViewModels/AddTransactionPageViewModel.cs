using System;
using System.Collections.Generic;
using debtsTracker.Entities;

namespace debtsTracker.ViewModels
{
    public class AddTransactionPageViewModel : AddPageViewModel
    {

        protected override void Save()
		{

			var transaction = new Transaction
			{
				Comment = Comment,
				Date = DateTime,
				Name = Name,
				Value = Amount
			};
            var success = DebtsManager.AddTransaction(transaction);
			if (!success)
			{
				ShowAlert();
				return;
			}
			NavigationService.GoBack();
		}

        private void ShowAlert()
        {
			var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
			alert
				 .SetMessage(Resource.String.add_transaction_failed)
				.SetPositiveButton(Resource.String.close, (sender, e) => { });

			alert.Create().Show();
        }
    }
}
