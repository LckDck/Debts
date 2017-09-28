using System;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Adapters;
using debtsTracker.Managers;
using debtsTracker.ViewModels;
using Microsoft.Practices.ServiceLocation;
using static Android.Support.Design.Widget.TabLayout;

namespace debtsTracker.Fragments
{
    public class MainFragment : BaseFragment, IOnTabSelectedListener
    {
        MainViewModel vm;
        public MainViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<MainViewModel> ());


        com.refractored.fab.FloatingActionButton _fab;

        ViewPager _pager;
        private TabLayout _tabs;

        public override void OnStart()
        {
            base.OnStart();
            SetTitle(Resource.String.app_name);
            var tab = _tabs.GetTabAt(oldValue);
			tab.Select();
        }

        View _view;
        int oldValue;

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            oldValue = Vm.Tab;
            if (_view != null) {
                return _view;
            }
            _view = inflater.Inflate (Resource.Layout.start_layout, container, false);
            _tabs = (TabLayout)_view.FindViewById (Resource.Id.tabs);
            _pager = (ViewPager)_view.FindViewById (Resource.Id.pager);
            Java.Lang.String [] tabNames =
            {
                new Java.Lang.String(Context.Resources.GetString(Resource.String.my_debts)),
                new Java.Lang.String(Context.Resources.GetString(Resource.String.debts_to_me)),
            };


            _pager.Adapter = new OwnerAdapter (ChildFragmentManager, tabNames);

            _tabs.SetupWithViewPager (_pager);
            _pager.PageSelected += OnPageSelected;
            _pager.LayoutChange += OnLayoutChange;

            _tabs.AddOnTabSelectedListener(this);

			_fab = _view.FindViewById<com.refractored.fab.FloatingActionButton> (Resource.Id.fab);

            _fab.Click += (sender, e) => {
                Vm.AddPage ();
            };

            return _view;
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
                _fab.Show ();
                _fab.AttachToRecyclerView (page.List);
            }
        }

        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {

        }

        public void OnTabReselected(Tab tab)
        {
            
        }

        public void OnTabSelected(Tab tab)
        {
            Vm.Tab = tab.Position;
        }

        public void OnTabUnselected(Tab tab)
        {
            
        }
    }
}
