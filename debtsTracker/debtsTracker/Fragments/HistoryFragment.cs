﻿using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Widget;
using debtsTracker.Adapters;
using debtsTracker.Entities;
using debtsTracker.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;

namespace debtsTracker.Fragments
{
    public class HistoryFragment : BaseFragment, IDialogInterfaceOnDismissListener
    {
        HistoryViewModel vm;
        public HistoryViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<HistoryViewModel> ());


        public HistoryFragment (Debt debt)
        {
            Vm.Debt = debt;
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetTitle(Vm.Debt.Name);
            var view = inflater.Inflate (Resource.Layout.history, container, false);
            var listView = view.FindViewById<RecyclerView> (Resource.Id.list);
            var total = view.FindViewById<TextView> (Resource.Id.total);

            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);

            listView.SetLayoutManager (linearLayoutManager);
            var items = Vm.GetItems ();
            var adapter = new TransactionsAdapter (items);

            InitActionBarButtons();

            listView.SetAdapter (adapter);

            Bindings.Add(this.SetBinding(() => Vm.TotalText)
              .WhenSourceChanges(() =>
              {
                  total.Text = vm.TotalText;
              }));
            return view;
        }

        ImageButton plusButton;
        ImageButton minusButton;
        ImageButton editButton;

        private void InitActionBarButtons()
        {
			plusButton = MainActivity.Current.FindViewById<ImageButton>(Resource.Id.add_button);
			plusButton.Visibility = Android.Views.ViewStates.Visible;
            plusButton.Click += AddTransaction;

			minusButton = MainActivity.Current.FindViewById<ImageButton>(Resource.Id.remove_button);
			minusButton.Visibility = Android.Views.ViewStates.Visible;
            minusButton.Click += MinusTransaction;


			editButton = MainActivity.Current.FindViewById<ImageButton>(Resource.Id.edit_button);
			editButton.Visibility = Android.Views.ViewStates.Visible;
			editButton.Click += EditName;
        }

		public override void OnDestroyView()
		{
			base.OnDestroyView();

            Vm.Unsubscribe();

			plusButton.Visibility = Android.Views.ViewStates.Gone;
			plusButton.Click -= AddTransaction;
			plusButton = null;

            minusButton.Visibility = Android.Views.ViewStates.Gone;
            minusButton.Click -= MinusTransaction;
            minusButton = null;

			editButton.Visibility = Android.Views.ViewStates.Gone;
			editButton.Click -= EditName;
			editButton = null;

		}


		private void EditName(object sender, EventArgs e)
        {
            var editText = new EditText(MainActivity.Current);
            editText.Text = Vm.Debt.Name;
            editText.Gravity = Android.Views.GravityFlags.Center;
           
			var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
            alert.SetView(editText)
                 .SetPositiveButton(Resource.String.save, (s, ev) =>
                 {
                     Vm.ChangeName(editText.Text);
                     SetTitle(editText.Text);
                     HideKeyboard();
                 })
                 .SetNeutralButton(Resource.String.cancel, (s, ev) =>
                 {
                     HideKeyboard();
                 })
                 .SetOnDismissListener(this);

			alert.Create().Show();
			editText.RequestFocus();
            ShowKeyboard();
        }

        private void MinusTransaction(object sender, EventArgs e)
        {
            Vm.AddPage(false);
        }

        private void AddTransaction(object sender, EventArgs e)
        {
            Vm.AddPage(true);
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            HideKeyboard();
        }
    }
}
