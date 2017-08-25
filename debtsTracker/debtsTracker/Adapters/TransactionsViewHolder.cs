using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace debtsTracker.Adapters
{
    public class TransactionsViewHolder : RecyclerView.ViewHolder
    {
        public TextView CommentTextView;
        public TextView DateTextView;
        public TextView ValueTextView;

       

		public TransactionsViewHolder (View itemView) : base (itemView)
        {
            DateTextView = itemView.FindViewById<TextView> (Resource.Id.date);
            CommentTextView = itemView.FindViewById<TextView> (Resource.Id.comment);
            ValueTextView = itemView.FindViewById<TextView> (Resource.Id.count);
        }
    }
}
