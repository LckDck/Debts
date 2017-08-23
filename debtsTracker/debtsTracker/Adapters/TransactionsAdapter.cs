using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Entities;

namespace debtsTracker.Adapters
{
    public class TransactionsAdapter : RecyclerView.Adapter
    {
        List<Transaction> _items;
        public TransactionsAdapter (List<Transaction> items)
        {
            _items = items;
        }

        public override int ItemCount {
            get {
                return _items.Count;
            }
        }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            var mholder = (TransactionsViewHolder)holder;
            var comment = _items [position].Comment;
            mholder.CommentTextView.SetText (comment, Android.Widget.TextView.BufferType.Normal);
            if (string.IsNullOrEmpty (comment)) mholder.CommentTextView.Visibility = ViewStates.Gone;
            var value = Utils.GetValueWithPrefix (_items [position].Value);
            mholder.ValueTextView.SetText (value, Android.Widget.TextView.BufferType.Normal);
            mholder.DateTextView.SetText (_items [position].Date.ToString("dddd dd MMMM yy"), Android.Widget.TextView.BufferType.Normal);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.history_item, parent, false);
            return new TransactionsViewHolder (itemView);
        }
    }
}
