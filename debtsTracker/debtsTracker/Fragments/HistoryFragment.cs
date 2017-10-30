using System;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Widget;
using debtsTracker.Adapters;
using debtsTracker.Entities;
using debtsTracker.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using Plugin.CurrentActivity;

namespace debtsTracker.Fragments
{
    public class HistoryFragment : BaseFragment, IDialogInterfaceOnDismissListener
    {
        HistoryViewModel vm;
        public HistoryViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<HistoryViewModel> ());


        public HistoryFragment (Debt debt)
        {
            Vm.Debt = debt;
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetTitle(Vm.Debt.Name);
            var view = inflater.Inflate (Resource.Layout.history, container, false);
            var listView = view.FindViewById<RecyclerView> (Resource.Id.list);
            var total = view.FindViewById<TextView> (Resource.Id.total);

            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);

            listView.SetLayoutManager (linearLayoutManager);
            var items = Vm.GetItems ();
            var adapter = new TransactionsAdapter (items);

            InitActionBarButtons();

            listView.SetAdapter (adapter);

            Bindings.Add(this.SetBinding(() => Vm.TotalText)
              .WhenSourceChanges(() =>
              {
                  total.Text = vm.TotalText;
              }));
            return view;
        }

        ImageButton plusButton;
        ImageButton minusButton;
        ImageButton editButton;
        private AlertDialog _dialog;
        private Button _dialogPositiveButton;
        private Button _dialogNegativeButton;
        private TextInputLayout _editText;

        private void InitActionBarButtons()
        {
			plusButton = MainActivity.Current.FindViewById<ImageButton>(Resource.Id.add_button);
			plusButton.Visibility = Android.Views.ViewStates.Visible;
            plusButton.Click += AddTransaction;

			minusButton = MainActivity.Current.FindViewById<ImageButton>(Resource.Id.remove_button);
			minusButton.Visibility = Android.Views.ViewStates.Visible;
            minusButton.Click += MinusTransaction;


			editButton = MainActivity.Current.FindViewById<ImageButton>(Resource.Id.edit_button);
			editButton.Visibility = Android.Views.ViewStates.Visible;
			editButton.Click += EditName;
        }

		public override void OnDestroyView()
		{
			base.OnDestroyView();

            Vm.Unsubscribe();

			plusButton.Visibility = Android.Views.ViewStates.Gone;
			plusButton.Click -= AddTransaction;
			plusButton = null;

            minusButton.Visibility = Android.Views.ViewStates.Gone;
            minusButton.Click -= MinusTransaction;
            minusButton = null;

			editButton.Visibility = Android.Views.ViewStates.Gone;
			editButton.Click -= EditName;
			editButton = null;

		}


		private void EditName(object sender, EventArgs e)
        {
            
            _editText = (TextInputLayout) LayoutInflater.Inflate(Resource.Layout.edit_name, null);

            _editText.EditText.Text = Vm.Debt.Name;
            _editText.EditText.Gravity = Android.Views.GravityFlags.Center;

            using (var alert = new AlertDialog.Builder(MainActivity.Current))
            {
                alert.SetView(_editText)
                     .SetPositiveButton(Resource.String.save, (s, ev) =>
                     {
                         
                     })
                     .SetNeutralButton(Resource.String.cancel, (s, ev) =>
                     {
                         HideKeyboard();
                     })
                     .SetOnDismissListener(this);

                _dialog = alert.Create();
            }

            _dialog.Show();

			_dialogPositiveButton = _dialog.GetButton((int)DialogButtonType.Positive);
            _dialogNegativeButton = _dialog.GetButton((int)DialogButtonType.Neutral);

			_dialogPositiveButton.Click += HandleDialogPositiveButtonClick;
			_dialogNegativeButton.Click += HandleDialogNegativeButtonClick;
            _editText.EditText.TextChanged += OnTextChanged;

			
			_editText.RequestFocus();
			ShowKeyboard();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _editText.Error = string.Empty;
        }

        private void HandleDialogPositiveButtonClick(object sender, EventArgs e)
		{
            var success = Vm.ChangeName(_editText.EditText.Text);
			if (success)
			{
				SetTitle(_editText.EditText.Text);
				HideKeyboard();
                DismissDialog();
			}
			else
			{
				_editText.Error = "Name exists";
			}
			
		}

		private void HandleDialogNegativeButtonClick(object sender, EventArgs e)
		{
			DismissDialog();
		}

		private void DismissDialog()
		{
			_dialogPositiveButton.Click -= HandleDialogPositiveButtonClick;
			_dialogNegativeButton.Click -= HandleDialogNegativeButtonClick;
            _editText.EditText.TextChanged -= OnTextChanged;
			_dialog.Dismiss();
            _dialog = null;
            _editText = null;
            _dialogNegativeButton = null;
            _dialogPositiveButton = null;
		}

        private void MinusTransaction(object sender, EventArgs e)
        {
            Vm.AddPage(false);
        }

        private void AddTransaction(object sender, EventArgs e)
        {
            Vm.AddPage(true);
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            HideKeyboard();
        }
    }
}
