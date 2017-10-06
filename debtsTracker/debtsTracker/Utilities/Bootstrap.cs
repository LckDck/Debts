using System;
using Android.Support.V7.App;
using Autofac;
using debtsTracker.Fragments;
using debtsTracker.Managers;
using Newtonsoft.Json;

namespace debtsTracker.Utilities
{
	public static class Bootstrap
	{
		private static ViewModelLocator locator;
		public static ViewModelLocator Locator => locator ?? (locator = new ViewModelLocator ());

		public static void RegisterServices (AppCompatActivity activity, int container)
		{
			var builder = new ContainerBuilder ();

			// Navigation service
			var nav = new DroidNavigationService ();
			nav.Init (activity, container);

			//charity
            nav.Configure (new PageConfigEntity () { Page = Page.AddPage, Type = typeof (AddPageFragment) });
            nav.Configure (new PageConfigEntity () { Page = Page.AddTransactionPage, Type = typeof (AddTransactionFragment) });
            nav.Configure (new PageConfigEntity () { Page = Page.HistoryPage, Type = typeof (HistoryFragment) });
            nav.Configure (new PageConfigEntity () { Page = Page.MainPage, Type = typeof (MainFragment), IsRoot = true });
			
            builder.RegisterInstance (nav).AsImplementedInterfaces ();

            builder.RegisterInstance (new DroidLocalStorage()).AsSelf ();
            builder.RegisterInstance (new GoogleDriveInteractor()).AsSelf ();
            builder.RegisterInstance (new StorageManager()).AsSelf ();
            builder.RegisterInstance (new DebtsManager()).AsSelf ();
            builder.RegisterInstance (new InterfaceUpdateManager()).AsSelf ();

			// Platform modules registration
			//builder.RegisterModule<PlatformModule> ();
			ViewModelLocator.RegisterServices (builder);
		}
	}
}

