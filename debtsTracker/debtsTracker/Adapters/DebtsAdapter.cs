using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Entities;
using Java.Lang;

namespace debtsTracker.Adapters
{
    public class DebtsAdapter : BaseAdapter<Debt>
    {
		private static int TYPE_ITEM = 1;
		private static int TYPE_FOOTER = 2;

        public Action<int, bool> AddTransaction { get; }

        public DebtsAdapter (List<Debt> items, Action<int, bool> addTransaction)
        {
            AddTransaction = addTransaction;
            Items = items;
        }

        public override int ItemCount {
            get {
                return Items.Count + 1;
            }
        }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            if (position == ItemCount - 1) {
                return;
            }
            var mholder = (ViewHolder)holder;
            mholder.NameTextView.SetText (Items [position].Name, Android.Widget.TextView.BufferType.Normal);
            var count = Items [position].Value;
            var val = Utils.GetValueWithPrefix (count);
            var color = (count >= 0) ? Utils.Green : Utils.DarkGray;
            mholder.ValueTextView.SetTextColor (color);
            mholder.ValueTextView.SetText (val, Android.Widget.TextView.BufferType.Normal);
        }

        public override int GetItemViewType(int position)
        {
            if (position == ItemCount - 1)
            {
                return TYPE_FOOTER;
            }
            return TYPE_ITEM;
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            if (viewType == TYPE_FOOTER) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.footer, parent, false);
                var holder = new FooterViewHolder(view);
                return holder;
            }

            var itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.debt_item, parent, false);
            var vh = new ViewHolder (itemView, OnClick, AskDelete, AddTransaction);
            return vh;
        }
    }

}
