using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace debtsTracker.Adapters
{
    public class ViewHolder : RecyclerView.ViewHolder
    {
		bool inDeletion;

		public TextView NameTextView;
        public TextView ValueTextView;

        public ViewHolder (View itemView, Action<int> listener, Action<int> delete) : base(itemView) { 
            NameTextView = itemView.FindViewById<TextView> (Resource.Id.name);
            ValueTextView = itemView.FindViewById<TextView> (Resource.Id.count);
            itemView.Click += (sender, e) => listener (AdapterPosition);
            itemView.LongClick += (sender, e) =>
            {
                delete(AdapterPosition);
            };
        }

    }
}
