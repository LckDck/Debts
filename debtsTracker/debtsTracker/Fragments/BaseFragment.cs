using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views.InputMethods;
using GalaSoft.MvvmLight.Helpers;

namespace debtsTracker.Fragments
{
	public class BaseFragment : Fragment
	{
		protected readonly List<Binding> Bindings = new List<Binding>();
		public BaseFragment ()
		{
		}

        protected void SetTitle(int res) {
            var toolbar = MainActivity.Current.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = Utils.GetStringFromResource(res);
        }

		protected void SetTitle(string title)
		{
			var toolbar = MainActivity.Current.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			toolbar.Title = title;
		}

        protected void HideKeyboard() {
			InputMethodManager inputManager = (InputMethodManager)MainActivity.Current.GetSystemService(Android.Content.Context.InputMethodService);
			var currentFocus = MainActivity.Current.CurrentFocus;
			if (currentFocus != null)
			{
                inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
			}
        }

		protected void ShowKeyboard()
		{
			InputMethodManager inputManager = (InputMethodManager)MainActivity.Current.GetSystemService(Android.Content.Context.InputMethodService);
			var currentFocus = MainActivity.Current.CurrentFocus;
			if (currentFocus != null)
			{
                inputManager.ShowSoftInput(currentFocus, ShowFlags.Forced);
			}
		}

		public void InsertFragmentIntoContainer (BaseFragment fragment, int container)
		{
			var exFragment = ChildFragmentManager.FindFragmentById (container);
			FragmentTransaction childFragTrans = ChildFragmentManager.BeginTransaction ();
			if (exFragment == null) {
				childFragTrans.Add (container, fragment);
			} else {
				childFragTrans.Replace (container, fragment);
			}
			childFragTrans.CommitAllowingStateLoss ();
		}

		public override void OnDestroyView()
		{
			foreach (var binding in Bindings)
			{
				binding.Detach();
			}
			Bindings.Clear();
			base.OnDestroyView();
		}
	}
}

