using System;
using debtsTracker.ViewModels;

namespace debtsTracker.Fragments
{
    public class HistoryFragment : BaseFragment
    {
        public HistoryViewModel Vm { get; private set; }

        public HistoryFragment ()
        {
        }

        public HistoryFragment (HistoryViewModel vm)
        {
            Vm = vm;
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.history, container, false);

            return view;
        }
    }
}
