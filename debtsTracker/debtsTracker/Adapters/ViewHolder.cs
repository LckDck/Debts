using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace debtsTracker.Adapters
{
    public class ViewHolder : RecyclerView.ViewHolder
    {
		
		public TextView NameTextView;
        public TextView ValueTextView;
        public ImageButton PlusButton;
        public ImageButton MinusButton;

        public ViewHolder (View itemView, Action<int> listener, Action<int> delete, Action <int, bool> addTransaction) : base(itemView) { 
            NameTextView = itemView.FindViewById<TextView> (Resource.Id.name);
            ValueTextView = itemView.FindViewById<TextView> (Resource.Id.count);
            PlusButton = itemView.FindViewById<ImageButton> (Resource.Id.plus_button);
            MinusButton = itemView.FindViewById<ImageButton> (Resource.Id.minus_button);
            itemView.LongClick += (sender, e) => delete(AdapterPosition);
            itemView.Click += (sender, e) => listener (AdapterPosition);
            PlusButton.Click += (sender, e) => addTransaction (AdapterPosition, true);
            MinusButton.Click += (sender, e) => addTransaction (AdapterPosition, false);
        }

    }
}
