using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using debtsTracker.Entities;
using debtsTracker.Managers;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.Adapters
{
    public class BaseAdapter<TModel> : RecyclerView.Adapter where TModel : IDisposable
    {
        protected List<TModel> Items;


        public override int ItemCount {
            get {
                throw new Exception ("ItemCount method should be overridden");
            }
        }

        public event EventHandler<int> ItemClick;

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            throw new Exception ("OnBindViewHolder method should be overridden");
        }

        public void OnClick (int position)
        {
            if (ItemClick != null) {
                ItemClick (this, position);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            throw new Exception ("OnCreateViewHolder method should be overridden");
        }


		protected void AskDelete(int position)
		{
			var alert = new Android.Support.V7.App.AlertDialog.Builder(MainActivity.Current);
			alert
				 .SetMessage(Resource.String.sure_delete)
				 .SetPositiveButton(Resource.String.yes, (sender, e) => Delete(position))
				 .SetNegativeButton(Resource.String.cancel, (sender, e) => { });

			alert.Create().Show();
		}




		protected void Delete(int position)
		{
            var item = Items[position];
            Items.RemoveAt(position);
            NotifyItemRemoved(position);
            item.Dispose();
        }
    }
}
