using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;

namespace debtsTracker.Utilities
{
	public class DroidNavigationService : IExtendedNavigationService
	{
		private AppCompatActivity mainActivity;
		private int container;
		private FragmentManager fragmentManager;

		private readonly Dictionary<Page, PageConfigEntity> pagesByKey = new Dictionary<Page, PageConfigEntity> ();

		public void Init (AppCompatActivity activity, int container)
		{
			mainActivity = activity;
			this.container = container;
			fragmentManager = activity.SupportFragmentManager;
		}

		public void Configure (PageConfigEntity config)
		{
			lock (pagesByKey) {
				if (pagesByKey.ContainsKey (config.Page)) {
					pagesByKey [config.Page] = config;
				} else {
					pagesByKey.Add (config.Page, config);
				}
			}
		}

		private Stack<Page> pageStack = new Stack<Page> ();

		public event EventHandler<Page> BeforeNavigateEvent;

		public Page? CurrentPage {
			get {
				if (pageStack.Count == 0) return null;
				return pageStack.Peek ();
			}
		}

		public int BackStackCount => pageStack.Count;

		public void GoBack ()
		{
			try {
				pageStack.Pop ();
				fragmentManager.PopBackStack ();
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine ($"failed to backstack {ex.Message}");
			}
		}

		private Fragment CreateFragment (Page page, object parameter)
		{
			object [] parameters = parameter != null ? new [] { parameter } : null;
			var fragment = Activator.CreateInstance (pagesByKey [page].Type, parameters) as Fragment;
			return fragment;
		}

		public void NavigateTo (Page page, object parameter)
		{
			mainActivity.RunOnUiThread (() => {
				if (page == CurrentPage && parameter == null) return;
				BeforeNavigateEvent?.Invoke (this, page);
				if (mainActivity == null)
					throw new InvalidOperationException ("No MainActivity found");

				lock (pagesByKey) {
					if (!pagesByKey.ContainsKey (page))
						throw new ArgumentException ($"No such page: {page.ToString ()}. Did you forget to call NavigationService.Configure?");

					var fragment = CreateFragment (page, parameter);

					var transaction = fragmentManager
						.BeginTransaction ()
						.SetCustomAnimations (Resource.Animation.anim_slide_in_right, Resource.Animation.anim_slide_out_right)
						.Replace (container, fragment, page.ToString ());

					if (!pagesByKey[page].IsRoot)
                    {

                        transaction.AddToBackStack(page.ToString());
                    }
                    else if (pageStack.Count > 0)
                    {
                        pageStack.Clear();
                        fragmentManager.PopBackStackImmediate(null, FragmentManager.PopBackStackInclusive);
                    }

					transaction.CommitAllowingStateLoss ();
					pageStack.Push (page);
				}
			});
		}

		public void NavigateUrl (string url)
		{
			if (string.IsNullOrEmpty (url)) return;
			var uri = Android.Net.Uri.Parse (url);
			var intent = new Intent (Intent.ActionView, uri);
			intent.AddFlags (ActivityFlags.NewTask);
			CrossCurrentActivity.Current.Activity.ApplicationContext.StartActivity (intent);
		}

		public void NavigateTo (Page page)
		{
			NavigateTo (page, null);
		}

	}
}
