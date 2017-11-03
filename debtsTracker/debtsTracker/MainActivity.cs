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
using Android.Animation;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Common;
using System.Threading.Tasks;
using System.Threading;
using Android.Media;
using System.IO;
using Android.Gms.Drive.Query;
using Java.IO;
using debtsTracker.Managers;
using Debts.Interfaces;
using Android.Gms.Ads;

namespace debtsTracker
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity, Android.Support.V4.App.FragmentManager.IOnBackStackChangedListener, View.IOnClickListener
    {
        IExtendedNavigationService _navigationService;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        private BaseVm vm;
        public BaseVm Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<BaseVm>());

        ActionBarDrawerToggle drawerToggle;
        private GoogleDriveInteractor _driveManager;
        public static Activity Current;
        const int REQUESTCODE_PICK_TEXT = 23424;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Current = this;
            Bootstrap.RegisterServices(this, Resource.Id.main_content);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.app_name, Resource.String.app_name);
            drawerLayout.AddDrawerListener(drawerToggle);
            drawerLayout.DrawerClosed += OnDrawerClosedTask;
            drawerToggle.SyncState();
            drawerToggle.DrawerSlideAnimationEnabled = false;

            drawerToggle.ToolbarNavigationClickListener = this;
            SetSupportActionBar(toolbar);
            //Enable support action bar to display hamburger
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);


            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            navigationView.NavigationItemSelected += (sender, e) =>
            {
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_import:
                        AskImportSource();
                        break;
                    case Resource.Id.nav_export:
                        _driveManager.SaveFile();
                        break;
                    case Resource.Id.nav_backup:
                        break;

                    case Resource.Id.nav_upgrade:
                        Buy();
                        break;
                    default:
                        break;

                }
                //react to click here and swap fragments or navigate
                drawerLayout.CloseDrawers();
            };
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window window = this.Window;
                window.AddFlags(WindowManagerFlags.TranslucentStatus);
            }



            SupportFragmentManager.AddOnBackStackChangedListener(this);

            //SetContentView (Resource.Layout.history);
            _navigationService = ServiceLocator.Current.GetInstance<IExtendedNavigationService>();
            _navigationService.NavigateTo(Page.MainPage);

			_driveManager = ServiceLocator.Current.GetInstance<GoogleDriveInteractor>();
            _driveManager.Init (Utils.GetStringFromResource(Resource.String.app_name));

			_inapp = ServiceLocator.Current.GetInstance<IInAppPurchase>();
            LoadProducts();
            MobileAds.Initialize(this, Constants.AdMobId);
			mAdView = FindViewById<Android.Gms.Ads.AdView>(Resource.Id.adView);

			var builder = new AdRequest.Builder();
			var adRequest = builder.Build();

			mAdView.LoadAd(adRequest);
        }

		async void LoadProducts()
		{
			var product = await _inapp.GetProdutctInfo(_inapp.PaidItem);
			if (product != null)
			{
				//Paid = product.Bought;
				//_storage.Store(Constants.Paid, product.Bought);
			}
		}

        IInAppPurchase _inapp;
        private AdView mAdView;

        private async Task Buy()
        {
            await _inapp.BuyProduct(_inapp.PaidItem);
        }

        private void AskImportSource()
        {
			var alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            var items = new string[] { 
                Utils.GetStringFromResource(Resource.String.from_drive), 
                Utils.GetStringFromResource(Resource.String.from_device)
            };
            alert.SetItems(items, OnImportSourceSelected);
			alert.Create().Show();
		}

        private void OnImportSourceSelected(object sender, DialogClickEventArgs e)
        {
            switch (e.Which) { 
                case 0:
                    _driveManager.ReadFile();
                    break;
                case 1:
                    
    		         Intent mediaIntent = new Intent(Intent.ActionGetContent);
    		         mediaIntent.SetType("text/plain"); //set mime type as per requirement
					StartActivityForResult(mediaIntent, REQUESTCODE_PICK_TEXT);
					break;
            }

        }

        void OnDrawerClosedTask(object sender, DrawerLayout.DrawerClosedEventArgs e)
        {

        }


        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                OnBackPressed();
            }
            else
            {
                if (drawerLayout.IsDrawerOpen(GravityCompat.Start))
                {
                    drawerLayout.CloseDrawers();
                }
                else
                {
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                }
            }
            return true;
        }

        public void OnBackStackChanged()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SetDrawerState(false);
                //to revert, comment
                ObjectAnimator.OfFloat(drawerToggle.DrawerArrowDrawable, "progress", 1).Start();
            }
            else
            {

                SetDrawerState(true);
                //to revert, comment
                ObjectAnimator.OfFloat(drawerToggle.DrawerArrowDrawable, "progress", 0).Start();
            }
        }

        public void SetDrawerState(bool isEnabled)
        {
            if (isEnabled)
            {
                drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
            }
            else
            {
                drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
            }
        }

        public void OnClick(View v)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            ToggleSoftInputCustom();
            if (_navigationService.BackStackCount > 0)
            {
                _navigationService.GoBack();
            }
            else
                base.OnBackPressed();
        }

        private void ToggleSoftInputCustom()
        {
            Rect rect = new Rect();
            this.Window.DecorView.GetWindowVisibleDisplayFrame(rect);
            var imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            var currentHeight = rect.Height();
            var statusBarHeight = rect.Top;
            var screenHeight = Resources.DisplayMetrics.HeightPixels - statusBarHeight;
            if (currentHeight < screenHeight) imm.ToggleSoftInput(ShowFlags.Forced, 0);
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            System.Diagnostics.Debug.WriteLine("ActivityResult");
            switch (requestCode)
            {
                case GoogleDriveInteractor.RESULT_CODE: // Something may have been resolved, try connecting again
                    if (resultCode == Result.Ok)
                    {
                        _driveManager.GoogleDriveAction();
                    }
                    break;

                case REQUESTCODE_PICK_TEXT:
					if (resultCode == Result.Ok)
					{
                        string uri = data.DataString;
                        Android.Net.Uri uris = Android.Net.Uri.FromParts(data.Data.Scheme, data.Data.SchemeSpecificPart, data.Data.Fragment);
                        System.IO.Stream input = ContentResolver.OpenInputStream(data.Data);
                        _driveManager.ReadDriveFile(input);

					}
                    break;
            }
        }
    }
}



