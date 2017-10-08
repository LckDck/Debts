using System;
using Android.App;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using debtsTracker.Adapters;
using debtsTracker.Managers;
using debtsTracker.ViewModels;
using Java.Util;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;
using static Android.App.DatePickerDialog;
using static Android.Support.Design.Widget.TabLayout;

namespace debtsTracker.Fragments
{
    public class AddPageFragment : AddTransactionFragment, IOnTabSelectedListener
    {
        public override AddPageViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<AddPageViewModel>());

		TabLayout _tabs;

		ViewPager _pager;

        public AddPageFragment () : base (string.Empty, true)
        {
            InterfaceUpdateManager.NameFocus += OnNameFocus;
        }

        private void OnNameFocus(object sender, EventArgs e)
        {
            nameView.RequestFocus();
            ShowKeyboard();
        }


        private EditText nameView;

        protected override void InitView(LayoutInflater inflater, ViewGroup container)
        {
            _view = inflater.Inflate(Resource.Layout.add_debt, container, false);
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetTitle(Resource.String.add_debt);
			if (_view != null)
			{
				return _view;
			}
            base.OnCreateView(inflater, container, savedInstanceState);
			


			_pager = (ViewPager)_view.FindViewById(Resource.Id.pager);
			Java.Lang.String[] tabNames =
			{
				new Java.Lang.String(Context.Resources.GetString(Resource.String.my_debts)),
				new Java.Lang.String(Context.Resources.GetString(Resource.String.debts_to_me)),
			};
			_pager.Adapter = new OwnerAdapter(ChildFragmentManager, tabNames, true);




			_tabs = (TabLayout)_view.FindViewById(Resource.Id.tabs);

			_tabs.SetupWithViewPager(_pager);

			var tab = _tabs.GetTabAt(Vm.Tab);
			tab.Select();

			_tabs.AddOnTabSelectedListener(this);

			nameView = _view.FindViewById<EditText>(Resource.Id.name);
			nameView.TextChanged += (sender, e) => Vm.Name = nameView.Text;


			return _view;
        }



        protected override void CheckValid(object sender, EventArgs e)
        {
            bool hasError = false;
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
                HideKeyboard();
                Vm.SaveCommand.Execute(null);
            }
        }


        public override void OnDestroyView()
        {
            base.OnDestroyView();
            _tabs.RemoveOnTabSelectedListener(this);
			_tabs = null;
			_pager = null;
        }

		public void OnTabReselected(Tab tab)
		{

		}

		public void OnTabSelected(Tab tab)
		{
			Vm.Tab = tab.Position;
		}

		public void OnTabUnselected(Tab tab)
		{

		}

    }
}
