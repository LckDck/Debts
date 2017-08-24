using System;
using Android.App;
using Android.Support.V4.App;
using Android.Widget;
using debtsTracker.ViewModels;
using Java.Util;
using Plugin.CurrentActivity;

namespace debtsTracker.Fragments
{
    public class AddPageFragment : BaseFragment
    {
        public AddPageViewModel Vm { get; private set; }

        public AddPageFragment ()
        {
        }

        public AddPageFragment (AddPageViewModel vm)
        {
            Vm = vm;
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.add_transaction, container, false);
            var dateView = view.FindViewById<EditText> (Resource.Id.date);
            dateView.Text = DateTime.Now.ToString (Utils.DatePattern);
            dateView.Click += (sender, e) => {
                Calendar calendar = Calendar.GetInstance (Java.Util.TimeZone.Default);
                DatePickerDialog dialog = new DatePickerDialog (CrossCurrentActivity.Current.Activity, ChangeText,
                                                                calendar.Get (CalendarField.Year), calendar.Get (CalendarField.Month),
                                                                calendar.Get (CalendarField.DayOfMonth));
                dialog.Show ();
            };

            var amountView = view.FindViewById<EditText> (Resource.Id.amount);
            amountView.TextChanged += (sender, e) => {
                var color = (e.Text.ToString ().StartsWith ("-")) ? Utils.DarkGray : Utils.Green;
                amountView.SetTextColor (color);
            };
            return view;
        }

        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {

        }
    }
}
