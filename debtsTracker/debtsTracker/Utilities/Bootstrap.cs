﻿using System;
using Android.Support.V7.App;
using Autofac;
using debtsTracker.Fragments;

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
            nav.Configure (new PageConfigEntity () { Page = Page.HistoryPage, Type = typeof (HistoryFragment) });
			
			builder.RegisterInstance (nav).AsImplementedInterfaces ();

			// Platform modules registration
			//builder.RegisterModule<PlatformModule> ();
			ViewModelLocator.RegisterServices (builder);
		}
	}
}
