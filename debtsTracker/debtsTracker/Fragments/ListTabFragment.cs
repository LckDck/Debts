using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Adapters;
using debtsTracker.Entities;
using debtsTracker.Managers;
using debtsTracker.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;

namespace debtsTracker.Fragments
{
    public class ListTabFragment : BaseFragment
    {
        MainViewModel vm;
        public MainViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<MainViewModel> ());
        List<Debt> _items;
        bool _myDebts;
        RecyclerView _listView;

		InterfaceUpdateManager _interfaceUpdateManager;
		InterfaceUpdateManager InterfaceUpdateManager
		{
			get
			{
				return _interfaceUpdateManager ?? (_interfaceUpdateManager = ServiceLocator.Current.GetInstance<InterfaceUpdateManager>());
			}
		}

        public ListTabFragment (bool myDebts)
        {
            _myDebts = myDebts;
            InterfaceUpdateManager.UpdateMainScreen += UpdateScreen;
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            _items.Clear();
            Vm.ResetDebts();
            _items.AddRange(Vm.GetItems(_myDebts));
            _listView.GetAdapter().NotifyDataSetChanged();
        }

        public RecyclerView List {
            get {
                return _listView;
            }
        }


        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.debts_list, container, false);

            _listView = view.FindViewById<RecyclerView> (Resource.Id.list);

            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);

            _listView.SetLayoutManager (linearLayoutManager);
            _items = Vm.GetItems (_myDebts);
            var adapter = new DebtsAdapter (_items, AddTransactionAction);
            adapter.ItemClick += OnItemClick;
            _listView.SetAdapter (adapter);


            return view;
        }

        Action<int> AddTransactionAction => (pos) => { Vm.AddTransaction(pos); };

        void OnItemClick (object sender, int e)
        {
            Vm.ShowDetails (_items [e]);
        }
    }
}
