using System;
using Android.Graphics;
using Android.Support.V4.Content;
using Plugin.CurrentActivity;

namespace debtsTracker
{
    public static class Utils
    {
        public static string DatePattern = "ddd dd MMMM yy";
        public static string GetValueWithPrefix (int value)
        {
            return value >= 0 ? "+" + value : value.ToString ();
        }

        public static Color Green => GetColorFromResource (Resource.Color.accent);
        public static Color DarkGray => GetColorFromResource (Resource.Color.primary_dark);

        static Color GetColorFromResource (int res)
        {
            return new Android.Graphics.Color (ContextCompat.GetColor (CrossCurrentActivity.Current.Activity, res));
        }
    }
}
