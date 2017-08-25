using System;
using debtsTracker.Entities;
using debtsTracker.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.Fragments
{
    public class HistoryFragment : BaseFragment
    {
        HistoryViewModel vm;
        public HistoryViewModel Vm => vm ?? (vm = ServiceLocator.Current.GetInstance<HistoryViewModel> ());

        private Debt _debt;



        public HistoryFragment (Debt debt)
        {
            _debt = debt;
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.history, container, false);

            return view;
        }
    }
}
