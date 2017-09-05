using System;
using System.Diagnostics.Contracts;
using Android.Support.V4.App;
using debtsTracker.Fragments;
using JavaString = Java.Lang.String;

namespace debtsTracker.Adapters
{
    public class OwnerAdapter : FragmentPagerAdapter
    {
        private readonly JavaString [] tabsNames;
        readonly bool empty;

        public OwnerAdapter (FragmentManager fm, JavaString [] tabNames, bool empty = false) : base (fm)
        {
            this.empty = empty;
            tabsNames = tabNames;
        }

        Fragment EmptyFragment => new Fragment ();

        public override int Count => tabsNames.Length;

        public override Fragment GetItem (int position)
        {
            if (empty) return EmptyFragment;

            if (position == 0) {
                return new ListTabFragment (true);
            } else {
                return new ListTabFragment (false);
            }
        }


        public override Java.Lang.ICharSequence GetPageTitleFormatted (int position)
        {
            return tabsNames [position % tabsNames.Length];
        }
    }
}

