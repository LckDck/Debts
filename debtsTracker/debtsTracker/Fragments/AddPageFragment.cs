using System;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using debtsTracker.Adapters;
using debtsTracker.ViewModels;
using Java.Util;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;
using static Android.Support.Design.Widget.TabLayout;

namespace debtsTracker.Fragments
{
    public class AddPageFragment : BaseFragment, IOnTabSelectedListener
    {
        AddPageViewModel vm;
        public AddPageViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<AddPageViewModel>());


		public AddPageFragment (bool toMe)
        {
            Vm.ToMe = toMe;
        }



        TabLayout _tabs;

        ViewPager _pager;
        Button doneButton;
        private EditText amountView;
        private EditText nameView;

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
			SetTitle(Resource.String.add_debt);
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


			

            _pager = (ViewPager)view.FindViewById (Resource.Id.pager);
            Java.Lang.String [] tabNames =
            {
                new Java.Lang.String(Context.Resources.GetString(Resource.String.my_debts)),
                new Java.Lang.String(Context.Resources.GetString(Resource.String.debts_to_me)),
            };
            _pager.Adapter = new OwnerAdapter (ChildFragmentManager, tabNames, true);


			_tabs = (TabLayout)view.FindViewById(Resource.Id.tabs);
			
			_tabs.SetupWithViewPager(_pager);

            var tab = _tabs.GetTabAt(Vm.ToMe ? 1 : 0);
            tab.Select();
            _tabs.AddOnTabSelectedListener(this);

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
                InputMethodManager inputManager = (InputMethodManager)MainActivity.Current.GetSystemService(Android.Content.Context.InputMethodService);
				var currentFocus = Activity.CurrentFocus;
				if (currentFocus != null)
				{
                    inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
				}
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
            _tabs = null;
            _pager = null;
        }



        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {
            Vm.DateTime = e.Date;
        }

        public void OnTabReselected(Tab tab)
        {
           
        }

        public void OnTabSelected(Tab tab)
        {
            Vm.ToMe = (tab.Text == Utils.GetStringFromResource(Resource.String.debts_to_me));
        }

        public void OnTabUnselected(Tab tab)
        {
            
        }
    }
}
