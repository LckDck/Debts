using System;
using System.Collections.Generic;
using Android.Support.V4.App;

namespace debtsTracker.Fragments
{
	public class BaseFragment : Fragment
	{
		
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

		public override void OnDestroyView ()
		{
			
			base.OnDestroyView ();
		}
	}
}

