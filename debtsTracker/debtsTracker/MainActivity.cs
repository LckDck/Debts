using Android.App;
using Android.Widget;
using Android.OS;
using com.refractored.fab;
using Android.Support.V7.Widget;
using Plugin.CurrentActivity;
using debtsTracker.Adapters;
using System;
using System.Collections.Generic;
using debtsTracker.Entities;
using Java.Util;
using Android.Graphics;
using debtsTracker.Fragments;
using Microsoft.Practices.ServiceLocation;
using debtsTracker.Utilities;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Support.V4.View;
using debtsTracker.ViewModels;
using Android.Views.InputMethods;
using Android.Content;

namespace debtsTracker
{
    [Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity, Android.Support.V4.App.FragmentManager.IOnBackStackChangedListener, View.IOnClickListener
    {
        IExtendedNavigationService _navigationService;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        private MainViewModel vm;
        public MainViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<MainViewModel> ());

        ActionBarDrawerToggle drawerToggle;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            Bootstrap.RegisterServices (this, Resource.Id.main_content);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
            SetSupportActionBar (toolbar);
            toolbar.SetTitleTextColor (Utils.GetColorFromResource (Resource.Color.primary_dark));

            //Enable support action bar to display hamburger
            SupportActionBar.SetDisplayHomeAsUpEnabled (true);
            SupportActionBar.SetDisplayShowHomeEnabled (true);

            drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView> (Resource.Id.nav_view);

            navigationView.NavigationItemSelected += (sender, e) => {
                e.MenuItem.SetChecked (true);
                //react to click here and swap fragments or navigate
                drawerLayout.CloseDrawers ();
            };
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop) {
                Window window = this.Window;
                window.AddFlags (WindowManagerFlags.TranslucentStatus);
            }

            drawerToggle = new ActionBarDrawerToggle (this, drawerLayout, toolbar, Resource.String.app_name, Resource.String.app_name);
            drawerLayout.AddDrawerListener (drawerToggle);
            drawerLayout.DrawerClosed += OnDrawerClosedTask;
            drawerToggle.ToolbarNavigationClickListener = this;
            drawerToggle.SyncState ();

            SupportFragmentManager.AddOnBackStackChangedListener (this);

            //SetContentView (Resource.Layout.history);
            _navigationService = ServiceLocator.Current.GetInstance<IExtendedNavigationService>();
            _navigationService.NavigateTo (Page.MainPage);

        }

        void OnDrawerClosedTask (object sender, DrawerLayout.DrawerClosedEventArgs e)
        {

        }

        //bool ViewIsAtHome;
        //public void displayView (int viewId)
        //{

        //    BaseFragment fragment = null;

        //    switch (viewId) {
        //    case Resource.Layout.add_transaction:
        //        fragment = new AddPageFragment ();
        //        //title = "Add debt";
        //        ViewIsAtHome = false;
        //        break;
        //    case Resource.Layout.history:
        //        fragment = new HistoryFragment ();
        //        //title = "History";
        //        ViewIsAtHome = false;
        //        break;

        //    case Resource.Id.main_content:
        //        fragment = new HistoryFragment ();
        //        //title = "History";
        //        ViewIsAtHome = true;
        //        break;

        //    }

        //    if (fragment != null) {
        //        var ft = SupportFragmentManager.BeginTransaction ();
        //        ft.Replace (Resource.Id.main_content, fragment);
        //        ft.Commit ();
        //    }

        //    // set the toolbar title
        //    if (SupportActionBar != null) {
        //        SupportActionBar.SetTitle (Resource.String.app_name);
        //    }


        //    drawerLayout.CloseDrawer (GravityCompat.Start);

        //}

        //public override void OnBackPressed ()
        //{

        //    if (drawerLayout.IsDrawerOpen (GravityCompat.Start)) {
        //        drawerLayout.CloseDrawer (GravityCompat.Start);
        //    }
        //    if (!) {

        //        displayView (Resource.Id.main_content);
        //    } else {

        //        MoveTaskToBack (true);
        //    }
        //    base.OnBackPressed ();
        //}


        public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
        {
            if (drawerLayout.IsDrawerOpen (GravityCompat.Start)) {
                drawerLayout.CloseDrawers ();
            } else {
                drawerLayout.OpenDrawer (GravityCompat.Start);
            }
            return true;

        }

        public void OnBackStackChanged ()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
                SetDrawerState (false);
            else

                SetDrawerState (true);
        }

        public void SetDrawerState (bool isEnabled)
        {
            if (isEnabled) {
                drawerLayout.SetDrawerLockMode (DrawerLayout.LockModeUnlocked);
                drawerToggle.OnDrawerStateChanged (DrawerLayout.LockModeUnlocked);
                drawerToggle.DrawerIndicatorEnabled = true;
                drawerToggle.SyncState ();

            } else {
                drawerLayout.SetDrawerLockMode (DrawerLayout.LockModeLockedClosed);
                drawerToggle.OnDrawerStateChanged (DrawerLayout.LockModeLockedClosed);
                drawerToggle.DrawerIndicatorEnabled = false;
                drawerToggle.SyncState ();
            }
        }

        public void OnClick (View v)
        {
            OnBackPressed ();
        }

        public override void OnBackPressed ()
        {
            ToggleSoftInputCustom ();
            if (_navigationService.BackStackCount > 0) {
                _navigationService.GoBack ();
            }
            else
                base.OnBackPressed ();
        }

        private void ToggleSoftInputCustom ()
        {
            Rect rect = new Rect ();
            this.Window.DecorView.GetWindowVisibleDisplayFrame (rect);
            var imm = (InputMethodManager)this.GetSystemService (Context.InputMethodService);
            var currentHeight = rect.Height ();
            var statusBarHeight = rect.Top;
            var screenHeight = Resources.DisplayMetrics.HeightPixels - statusBarHeight;
            if (currentHeight < screenHeight) imm.ToggleSoftInput (ShowFlags.Forced, 0);
        }

    }
}



