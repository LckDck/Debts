﻿//using Android.App;
//using Android.Widget;
//using Android.OS;
//using com.refractored.fab;
//using Android.Support.V7.Widget;
//using Plugin.CurrentActivity;
//using debtsTracker.Adapters;
//using System;
//using System.Collections.Generic;
//using debtsTracker.Entities;
//using Java.Util;
//using Android.Graphics;
//using debtsTracker.Fragments;
//using Microsoft.Practices.ServiceLocation;
//using debtsTracker.Utilities;
//using Android.Support.V7.App;
//using Android.Support.V4.Widget;
//using Android.Support.Design.Widget;
//using Android.Views;
//using Android.Support.V4.View;
//using debtsTracker.ViewModels;
//using Android.Views.InputMethods;
//using Android.Content;
//using Android.Animation;
//using Android.Gms.Common.Apis;
//using Android.Gms.Drive;
//using Android.Gms.Common;

//namespace debtsTracker
//{
//	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
//	public class MainActivity : AppCompatActivity, Android.Support.V4.App.FragmentManager.IOnBackStackChangedListener, View.IOnClickListener, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
//	{
//		IExtendedNavigationService _navigationService;
//		DrawerLayout drawerLayout;
//		NavigationView navigationView;
//		private BaseVm vm;
//		public BaseVm Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<BaseVm>());

//		ActionBarDrawerToggle drawerToggle;
//		private GoogleApiClient _googleApiClient;

//		protected override void OnCreate(Bundle savedInstanceState)
//		{
//			base.OnCreate(savedInstanceState);
//			Bootstrap.RegisterServices(this, Resource.Id.main_content);
//			// Set our view from the "main" layout resource
//			SetContentView(Resource.Layout.Main);
//			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


//			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
//			drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.app_name, Resource.String.app_name);
//			drawerLayout.AddDrawerListener(drawerToggle);
//			drawerLayout.DrawerClosed += OnDrawerClosedTask;
//			drawerToggle.SyncState();
//			drawerToggle.DrawerSlideAnimationEnabled = false;

//			drawerToggle.ToolbarNavigationClickListener = this;
//			SetSupportActionBar(toolbar);
//			//Enable support action bar to display hamburger
//			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
//			SupportActionBar.SetDisplayShowHomeEnabled(true);


//			navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

//			navigationView.NavigationItemSelected += (sender, e) => {
//				e.MenuItem.SetChecked(true);
//				//react to click here and swap fragments or navigate
//				drawerLayout.CloseDrawers();
//			};
//			if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
//			{
//				Window window = this.Window;
//				window.AddFlags(WindowManagerFlags.TranslucentStatus);
//			}



//			SupportFragmentManager.AddOnBackStackChangedListener(this);

//			//SetContentView (Resource.Layout.history);
//			_navigationService = ServiceLocator.Current.GetInstance<IExtendedNavigationService>();
//			_navigationService.NavigateTo(Page.MainPage);


//			_googleApiClient = new GoogleApiClient.Builder(this)
//			   .AddApi(DriveClass.API)
//			   .AddScope(DriveClass.ScopeFile)
//			   .AddScope(DriveClass.ScopeAppfolder)
//			   .AddConnectionCallbacks(this)
//			   .AddOnConnectionFailedListener(this)
//			   .Build();

//		}



//		void OnDrawerClosedTask(object sender, DrawerLayout.DrawerClosedEventArgs e)
//		{

//		}




//		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
//		{
//			if (SupportFragmentManager.BackStackEntryCount > 0)
//			{
//				OnBackPressed();
//			}
//			else
//			{
//				if (drawerLayout.IsDrawerOpen(GravityCompat.Start))
//				{
//					drawerLayout.CloseDrawers();
//				}
//				else
//				{
//					drawerLayout.OpenDrawer(GravityCompat.Start);
//				}
//			}
//			return true;
//		}

//		public void OnBackStackChanged()
//		{
//			if (SupportFragmentManager.BackStackEntryCount > 0)
//			{
//				SetDrawerState(false);
//				//to revert, comment
//				ObjectAnimator.OfFloat(drawerToggle.DrawerArrowDrawable, "progress", 1).Start();
//			}
//			else
//			{

//				SetDrawerState(true);
//				//to revert, comment
//				ObjectAnimator.OfFloat(drawerToggle.DrawerArrowDrawable, "progress", 0).Start();
//			}
//		}

//		public void SetDrawerState(bool isEnabled)
//		{
//			if (isEnabled)
//			{
//				drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
//			}
//			else
//			{
//				drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
//			}
//		}

//		public void OnClick(View v)
//		{
//			OnBackPressed();
//		}

//		public override void OnBackPressed()
//		{
//			ToggleSoftInputCustom();
//			if (_navigationService.BackStackCount > 0)
//			{
//				_navigationService.GoBack();
//			}
//			else
//				base.OnBackPressed();
//		}

//		private void ToggleSoftInputCustom()
//		{
//			Rect rect = new Rect();
//			this.Window.DecorView.GetWindowVisibleDisplayFrame(rect);
//			var imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
//			var currentHeight = rect.Height();
//			var statusBarHeight = rect.Top;
//			var screenHeight = Resources.DisplayMetrics.HeightPixels - statusBarHeight;
//			if (currentHeight < screenHeight) imm.ToggleSoftInput(ShowFlags.Forced, 0);
//		}

//		public void OnConnectionFailed(ConnectionResult result)
//		{
//			if (result.HasResolution)
//			{
//				try
//				{
//					result.StartResolutionForResult(this, 2);
//				}
//				catch (IntentSender.SendIntentException e)
//				{
//					// Unable to resolve, message user appropriately
//				}
//			}
//			else
//			{
//				GooglePlayServicesUtil.ShowErrorDialogFragment(result.ErrorCode, this, 0);
//			}
//		}

//		public void OnConnected(Bundle connectionHint)
//		{
//			System.Diagnostics.Debug.WriteLine("Google API connected!");
//		}

//		public void OnConnectionSuspended(int cause)
//		{
//			System.Diagnostics.Debug.WriteLine("Google API connection suspended");
//		}

//		protected override void OnStart()
//		{
//			base.OnStart();
//			_googleApiClient.Connect();
//		}

//		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
//		{
//			base.OnActivityResult(requestCode, resultCode, data);

//			switch (requestCode)
//			{
//				case 101: // Something may have been resolved, try connecting again
//					if (resultCode == Result.Ok)
//						_googleApiClient.Connect();
//					break;
//			}
//		}
//	}
//}



