﻿using System;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views.Animations;
using Android.Widget;
using debtsTracker.Adapters;
using debtsTracker.ViewModels;
using Java.Util;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;

namespace debtsTracker.Fragments
{
    public class AddPageFragment : BaseFragment
    {
        AddPageViewModel vm;
        public AddPageViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<AddPageViewModel>());


		public AddPageFragment ()
        {
        }



        TabLayout _tabs;

        ViewPager _pager;
        Button doneButton;
        private EditText amountView;
        private EditText nameView;

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

            amountView = view.FindViewById<EditText> (Resource.Id.amount);
            amountView.TextChanged += (sender, e) => {
                Vm.Amount = string.IsNullOrEmpty(amountView.Text) ? 0 : Convert.ToDouble(amountView.Text);
            };

			nameView = view.FindViewById<EditText>(Resource.Id.name);
            nameView.TextChanged += (sender, e) => Vm.Name = nameView.Text;

			var commentView = view.FindViewById<EditText>(Resource.Id.comment);
            commentView.TextChanged += (sender, e) => Vm.Comment = commentView.Text;


			_tabs = (TabLayout)view.FindViewById (Resource.Id.tabs);

            _pager = (ViewPager)view.FindViewById (Resource.Id.pager);
            Java.Lang.String [] tabNames =
            {
                new Java.Lang.String(Context.Resources.GetString(Resource.String.my_debts)),
                new Java.Lang.String(Context.Resources.GetString(Resource.String.debts_to_me)),
            };
            _pager.Adapter = new OwnerAdapter (ChildFragmentManager, tabNames, true);
           
            _tabs.SetupWithViewPager (_pager);

            HasOptionsMenu = true;


            _tabs.TabSelected += OnTabSelected;


            doneButton = MainActivity.Current.FindViewById<Button>(Resource.Id.done_button);
            doneButton.Visibility = Android.Views.ViewStates.Visible;
            doneButton.Click += (sender, e) => CheckValid();

			return view;
        }

        private void CheckValid()
        {
            var hasError = false;
            if (string.IsNullOrEmpty(nameView.Text)) {
                Shake(nameView);
                hasError = true;
            }
            if (string.IsNullOrEmpty(amountView.Text))
			{
                Shake(amountView);
                hasError = true;
			}

            if (!hasError)
            {
                Vm.SaveCommand.Execute(null);
            }
        }

        private void Shake(EditText view)
        {
			Animation shake = AnimationUtils.LoadAnimation(CrossCurrentActivity.Current.Activity, Resource.Animation.anim_shake);
			view?.StartAnimation(shake);
        }

        public override void OnDestroyView ()
        {
            base.OnDestroyView ();
            doneButton.Visibility = Android.Views.ViewStates.Gone;
            doneButton = null;
            _tabs.TabSelected -= OnTabSelected;
            _tabs = null;
            _pager = null;
        }

        void OnTabSelected (object sender, TabLayout.TabSelectedEventArgs e)
        {

        }



        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {
            Vm.DateTime = e.Date;
        }
    }
}
