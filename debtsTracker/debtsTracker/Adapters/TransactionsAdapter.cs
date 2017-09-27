using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Entities;
using Plugin.CurrentActivity;

namespace debtsTracker.Adapters
{
    public class TransactionsAdapter : BaseAdapter<Transaction> 
    {
        public TransactionsAdapter (List<Transaction> items)
        {
            Items = items;
        }

        public override int ItemCount {
            get {
                return Items.Count;
            }
        }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            var mholder = (TransactionsViewHolder)holder;
            var comment = Items [position].Comment;
            mholder.CommentTextView.SetText (comment, Android.Widget.TextView.BufferType.Normal);
            mholder.CommentTextView.Visibility = string.IsNullOrEmpty (comment) ? ViewStates.Gone : ViewStates.Visible;
            var count = Items [position].Value;
            var value = Utils.GetValueWithPrefix (count);
            var color = (count >= 0) ? Utils.Green : Utils.DarkGray;
            mholder.ValueTextView.SetTextColor (color);
            mholder.ValueTextView.SetText (value, Android.Widget.TextView.BufferType.Normal);
            mholder.DateTextView.SetText (Items [position].Date.ToString(Utils.DatePattern), Android.Widget.TextView.BufferType.Normal);
        }



        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.history_item, parent, false);
            return new TransactionsViewHolder (itemView, AskDelete);
        }
    }
}
