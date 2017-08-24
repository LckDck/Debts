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

namespace debtsTracker
{
    [Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity
    {
        IExtendedNavigationService _navigationService;
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            Bootstrap.RegisterServices (this, Resource.Id.main_content);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
            SetSupportActionBar (toolbar);


            //Enable support action bar to display hamburger
            SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_menu_black_24dp);
            SupportActionBar.SetDisplayHomeAsUpEnabled (true);

            drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView> (Resource.Id.nav_view);

            navigationView.NavigationItemSelected += (sender, e) => {
                e.MenuItem.SetChecked (true);
                //react to click here and swap fragments or navigate
                drawerLayout.CloseDrawers ();
            };
            //drawerLayout.SetStatusBarBackgroundColor (Utils.GetColorFromResource(Resource.Color.primary_dark));
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop) {
                Window window = this.Window;
                window.AddFlags (WindowManagerFlags.TranslucentStatus);
            }


            //SetContentView (Resource.Layout.history);

            var listView = FindViewById<RecyclerView> (Resource.Id.list);


            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);

            listView.SetLayoutManager (linearLayoutManager);
            var items = GetItems ();
            var adapter = new DebtsAdapter (items);
            //var items = GetItems () [0].Transactions;
            //var adapter = new TransactionsAdapter (items);
            listView.SetAdapter (adapter);
            var fab = FindViewById<com.refractored.fab.FloatingActionButton> (Resource.Id.fab);
            fab.AttachToRecyclerView (listView);

            _navigationService = ServiceLocator.Current.GetInstance<IExtendedNavigationService> ();
            fab.Click += (sender, e) => {
                _navigationService.NavigateTo (Page.AddPage);
            };

        }


        public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
        {
            if (drawerLayout.IsDrawerOpen (GravityCompat.Start)) {
                drawerLayout.CloseDrawers ();
            } else {
                drawerLayout.OpenDrawer (GravityCompat.Start);
            }
            return true;

        }



        List<Debt> GetItems ()
        {
            var result = new List<Debt> () {

                new Debt { Name = "Anna",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 1100, Date = DateTime.Now, Comment="Какой-то коммент"},
                        new Transaction { Value = -300, Date = DateTime.Now, Comment=""},
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" }
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                }
            };

            return result;
        }

    }
}



