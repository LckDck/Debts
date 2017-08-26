using System;
using Android.Support.V7.Widget;
using debtsTracker.Adapters;
using debtsTracker.Entities;
using debtsTracker.ViewModels;
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
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.history, container, false);
            var listView = view.FindViewById<RecyclerView> (Resource.Id.list);


            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);

            listView.SetLayoutManager (linearLayoutManager);
            var items = Vm.GetItems ();
            var adapter = new TransactionsAdapter (items);

            listView.SetAdapter (adapter);

            var fab = view.FindViewById<com.refractored.fab.FloatingActionButton> (Resource.Id.fab);
            fab.AttachToRecyclerView (listView);

            fab.Click += (sender, e) => {
                Vm.AddPage ();
            };


            return view;
        }
    }
}
