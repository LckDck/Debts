using System;
using Android.Support.V7.Widget;
using Android.Views;

namespace debtsTracker.Adapters
{
    public class BaseAdapter : RecyclerView.Adapter
    {
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
    }
}
