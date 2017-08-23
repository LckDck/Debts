using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace debtsTracker.Adapters
{
    public class ViewHolder : RecyclerView.ViewHolder
    {
        public TextView NameTextView;
        public TextView ValueTextView;

        public ViewHolder (View itemView) : base(itemView) { 
            NameTextView = itemView.FindViewById<TextView> (Resource.Id.name);
            ValueTextView = itemView.FindViewById<TextView> (Resource.Id.count);
        }
    }
}
