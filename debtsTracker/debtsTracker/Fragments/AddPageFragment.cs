using System;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Widget;
using debtsTracker.Adapters;
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

        TabLayout _tabs;

        ViewPager _pager;


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

            _tabs = (TabLayout)view.FindViewById (Resource.Id.tabs);

            _pager = (ViewPager)view.FindViewById (Resource.Id.pager);
            Java.Lang.String [] tabNames =
            {
                new Java.Lang.String(Context.Resources.GetString(Resource.String.my_debts)),
                new Java.Lang.String(Context.Resources.GetString(Resource.String.debts_to_me)),
            };
            _pager.Adapter = new OwnerAdapter (ChildFragmentManager, tabNames, true);
           
            _tabs.SetupWithViewPager (_pager);
    

            _tabs.TabSelected += OnTabSelected;

            return view;
        }

        public override void OnDestroyView ()
        {
            base.OnDestroyView ();
            _tabs.TabSelected -= OnTabSelected;
            _tabs = null;
            _pager = null;
        }

        void OnTabSelected (object sender, TabLayout.TabSelectedEventArgs e)
        {

        }



        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {

        }
    }
}
