using System;
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
    public class HistoryFragment : BaseFragment
    {
        HistoryViewModel vm;
        public HistoryViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<HistoryViewModel> ());


        public HistoryFragment (Debt debt)
        {
            Vm.Debt = debt;
            SetTitle (debt.Name);
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.history, container, false);
            var listView = view.FindViewById<RecyclerView> (Resource.Id.list);
            var total = view.FindViewById<TextView> (Resource.Id.total);

            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);

            listView.SetLayoutManager (linearLayoutManager);
            var items = Vm.GetItems ();
            var adapter = new TransactionsAdapter (items);

            listView.SetAdapter (adapter);

            Bindings.Add(this.SetBinding(() => Vm.TotalText)
              .WhenSourceChanges(() =>
              {
                  total.Text = vm.TotalText;
              }));
            return view;
        }

        public override void OnDestroyView()
        {
            Vm.Unsubscribe();
            base.OnDestroyView();
        }
    }
}
