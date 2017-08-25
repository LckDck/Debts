
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using debtsTracker.Adapters;
using debtsTracker.Entities;
using debtsTracker.Utilities;
using debtsTracker.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;

namespace debtsTracker.Fragments
{
    public class MainFragment : BaseFragment
    {
        MainViewModel vm;
        public MainViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<MainViewModel>());

        public MainFragment ()
        {
           
        }

        IExtendedNavigationService _navigationService;


        List<Debt> _items;

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.start_layout, container, false);
            var listView = view.FindViewById<RecyclerView> (Resource.Id.list);


            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);

            listView.SetLayoutManager (linearLayoutManager);
            _items = Vm.GetItems ();
            var adapter = new DebtsAdapter (_items);
            adapter.ItemClick += OnItemClick;
            //var items = GetItems () [0].Transactions;
            //var adapter = new TransactionsAdapter (items);
            listView.SetAdapter (adapter);
           
            var fab = view.FindViewById<com.refractored.fab.FloatingActionButton> (Resource.Id.fab);
            fab.AttachToRecyclerView (listView);

            _navigationService = ServiceLocator.Current.GetInstance<IExtendedNavigationService> ();
            fab.Click += (sender, e) => {
                _navigationService.NavigateTo (Page.AddPage);

            };

            return view;
        }

        void OnItemClick (object sender, int e)
        {
            Vm.ShowDetails (_items[e]);
        }

        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {

        }
    }
}
