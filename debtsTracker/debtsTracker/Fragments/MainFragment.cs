using System;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Adapters;
using debtsTracker.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.Fragments
{
    public class MainFragment : BaseFragment
    {
        MainViewModel vm;
        public MainViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<MainViewModel> ());

        public MainFragment ()
        {

        }

        com.refractored.fab.FloatingActionButton _fab;

        ViewPager _pager;

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.start_layout, container, false);
            var tabs = (TabLayout)view.FindViewById (Resource.Id.tabs);
            _pager = (ViewPager)view.FindViewById (Resource.Id.pager);
            Java.Lang.String [] tabNames =
            {
                new Java.Lang.String(Context.Resources.GetString(Resource.String.my_debts)),
                new Java.Lang.String(Context.Resources.GetString(Resource.String.debts_to_me)),
            };


            _pager.Adapter = new OwnerAdapter (ChildFragmentManager, tabNames);
            tabs.SetupWithViewPager (_pager);
            _pager.PageSelected += OnPageSelected;
            _pager.LayoutChange += OnLayoutChange;

            _fab = view.FindViewById<com.refractored.fab.FloatingActionButton> (Resource.Id.fab);

            _fab.Click += (sender, e) => {
                Vm.AddPage ();
            };

            return view;
        }

        void OnPageSelected (object sender, ViewPager.PageSelectedEventArgs e)
        {
            AttachFab ();
        }

        void OnLayoutChange (object sender, View.LayoutChangeEventArgs e)
        {
            AttachFab ();
        }


        void AttachFab ()
        {
            ListTabFragment page = (ListTabFragment)_pager.Adapter.InstantiateItem (_pager, _pager.CurrentItem);
            if (page != null && page.List != null) {
                System.Diagnostics.Debug.WriteLine (_pager.CurrentItem);
                _fab.Show ();
                _fab.AttachToRecyclerView (page.List);
            }
        }

        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {

        }
    }
}
