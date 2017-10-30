using System;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using debtsTracker.Adapters;
using debtsTracker.Managers;
using debtsTracker.ViewModels;
using Java.Util;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;
using static Android.Resource;
using static Android.Support.Design.Widget.TabLayout;

namespace debtsTracker.Fragments
{
    public class AddTransactionFragment : BaseFragment
    {
        protected AddPageViewModel vm;
        public virtual AddPageViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<AddTransactionPageViewModel>());

        public AddTransactionFragment(string name, bool positive)
        {
            Vm.Positive = positive;
            Vm.Name = name;
        }

        InterfaceUpdateManager _interfaceUpdateManager;
        protected InterfaceUpdateManager InterfaceUpdateManager
        {
            get
            {
                return _interfaceUpdateManager ?? (_interfaceUpdateManager = ServiceLocator.Current.GetInstance<InterfaceUpdateManager>());
            }
        }


        ImageButton doneButton;
        protected EditText amountView;
        protected View _view;
        LinearLayout _form;
        EditText dateView;
        private DatePickerDialog dialog;


        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {

            InitTitle();
            InitView(inflater, container);
            amountView = _view.FindViewById<EditText>(Resource.Id.amount);
            var hintResource = Vm.Positive ? Resource.String.amount : Resource.String.amount_refund;
            amountView.Hint = Utils.GetStringFromResource(hintResource);
            amountView.TextChanged += (sender, e) =>
            {
                Vm.Amount = string.IsNullOrEmpty(amountView.Text) ? 0 : Convert.ToDouble(amountView.Text);
            };

            amountView.SetTextColor(Vm.Positive ? Utils.Green : Utils.DarkGray);

            var commentView = _view.FindViewById<EditText>(Resource.Id.comment);
            commentView.TextChanged += (sender, e) => Vm.Comment = commentView.Text;

            _form = _view.FindViewById<LinearLayout>(Resource.Id.form);

            dateView = _view.FindViewById<EditText>(Resource.Id.date);

            dateView.Text = DateTime.Now.ToString(Utils.DatePattern);
            dateView.Click += OnDateClick;

            doneButton = MainActivity.Current.FindViewById<ImageButton>(Resource.Id.done_button);
            doneButton.Visibility = Android.Views.ViewStates.Visible;
            doneButton.Click += CheckValid;
            return _view;
        }

        protected virtual void InitView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container)
        {
            _view = inflater.Inflate(Resource.Layout.add_transaction, container, false);
        }

        private void OnDateClick(object sender, EventArgs e)
		{
			_form.RequestFocus();
			Calendar calendar = Calendar.GetInstance(Java.Util.TimeZone.Default);
			dialog = new DatePickerDialog(CrossCurrentActivity.Current.Activity, ChangeText,
															calendar.Get(CalendarField.Year), calendar.Get(CalendarField.Month),
															calendar.Get(CalendarField.DayOfMonth));
			dialog.Show();
		}

		protected virtual void InitTitle()
		{
			var resource = Vm.Positive ? Resource.String.add_transaction : Resource.String.return_transaction;
            SetTitle($"{Utils.GetStringFromResource(resource)} | {Vm.Name}");
		}


		public override void OnDestroyView()
		{
			base.OnDestroyView();
			
			doneButton.Visibility = Android.Views.ViewStates.Gone;
			doneButton.Click -= CheckValid;
			doneButton = null;
			
		}


        protected virtual void CheckValid(object sender, EventArgs e)
		{
			var hasError = false;
			
			if (string.IsNullOrEmpty(amountView.Text))
			{
				Shake(amountView);
				hasError = true;
			}

			if (!hasError)
			{
				HideKeyboard();
				Vm.SaveCommand.Execute(null);
			}
		}

		protected void Shake(EditText view)
		{
            Android.Views.Animations.Animation shake = AnimationUtils.LoadAnimation(CrossCurrentActivity.Current.Activity, Resource.Animation.anim_shake);
			view?.StartAnimation(shake);
		}


		void ChangeText(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			dateView.Text = e.Date.ToString(Utils.DatePattern);
			Vm.DateTime = e.Date;
		}
	}
}
