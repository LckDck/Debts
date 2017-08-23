using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Entities;

namespace debtsTracker.Adapters
{
    public class DebtsAdapter : RecyclerView.Adapter
    {
        List<Debt> _items;
        public DebtsAdapter (List<Debt> items) {
            _items = items;
        }

        public override int ItemCount {
            get {
                return _items.Count;
            }
        }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            var mholder = (ViewHolder)holder;
            mholder.NameTextView.SetText(_items[position].Name, Android.Widget.TextView.BufferType.Normal);
            mholder.ValueTextView.SetText(_items[position].Value, Android.Widget.TextView.BufferType.Normal);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            
            var itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.debt_item, parent, false);
            var vh = new ViewHolder (itemView);
            return vh;
          
        }
    }

}
