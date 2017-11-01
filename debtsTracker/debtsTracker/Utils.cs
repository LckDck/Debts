using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Widget;
using Plugin.CurrentActivity;

namespace debtsTracker
{
    public static class Utils
    {
        public static string DatePattern = "ddd dd MMMM yyyy";
        public static string GetValueWithPrefix (double value)
        {
            return value.ToString ();
        }

        public static Color Green => GetColorFromResource (Resource.Color.accent);
        public static Color DarkGray => GetColorFromResource (Resource.Color.primary_dark);

        public static Color GetColorFromResource (int res)
        {
            return new Android.Graphics.Color (ContextCompat.GetColor (CrossCurrentActivity.Current.Activity, res));
        }

		public static string GetStringFromResource(int res)
		{
            return MainActivity.Current.GetString(res);
		}

        public static void ShowToast(string message)
        {
			Toast.MakeText(MainActivity.Current, message, ToastLength.Long).Show();
		}

        public static void ShowNothigToRead(Action relogin)
		{
            var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
            alert
                 .SetMessage(Resource.String.no_backup_found)
                .SetNegativeButton(Resource.String.close, (sender, e) => { })
                .SetPositiveButton(Resource.String.yes, (sender, e) => relogin());

            alert.Create().Show();
           
		}

        public static void ShowEmptyFile() 
        {
			var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
			alert
				.SetMessage(Resource.String.empty_backup)
                .SetPositiveButton(Resource.String.ok, (sender, e) => { });

			alert.Create().Show();

		}

		
    }
}
