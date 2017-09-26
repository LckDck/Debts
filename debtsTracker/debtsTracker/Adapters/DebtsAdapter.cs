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
    public class DebtsAdapter : BaseAdapter
    {
        List<Debt> _items;


        public DebtsAdapter (List<Debt> items)
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
            var mholder = (ViewHolder)holder;
            mholder.NameTextView.SetText (_items [position].Name, Android.Widget.TextView.BufferType.Normal);
            var count = _items [position].Value;
            var value = Utils.GetValueWithPrefix (count);
            var color = (count >= 0) ? Utils.Green : Utils.DarkGray;
            mholder.ValueTextView.SetTextColor (color);
            mholder.ValueTextView.SetText (value, Android.Widget.TextView.BufferType.Normal);
            
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {

            var itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.debt_item, parent, false);
            var vh = new ViewHolder (itemView, OnClick, AskDelete);
            return vh;
        }

        private void AskDelete(int position)
        {
            var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
            alert
                 .SetMessage(Resource.String.sure_delete)
                 .SetPositiveButton(Resource.String.yes, (sender, e) => Delete(position))
                 .SetNegativeButton(Resource.String.cancel, (sender, e) => { });

           alert.Create().Show();
        }



        public void Delete(int position)
		{
			_items.RemoveAt(position);
            NotifyItemRemoved(position);
		}


    }

}
