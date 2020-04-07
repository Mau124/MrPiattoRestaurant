using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace MrPiattoRestaurant.adapters
{
    public class WaitersAdapter : RecyclerView.Adapter
    {
        List<string> waiters = new List<string>();

    public WaitersAdapter(List<string> waiters)
    {
        this.waiters = waiters;
    }
    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.layout_item_waiter, parent, false);
        WaitersViewHolder vh = new WaitersViewHolder(itemView);
        return vh;
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        holder.IsRecyclable = false;
        WaitersViewHolder vh = holder as WaitersViewHolder;

        // Load the photo image resource from the photo album:

        // Load the photo caption from the photo album:
        vh.name.Text = waiters[position];
    }

    public override int ItemCount
    {
        get { return waiters.Count; }
    }

    public class WaitersViewHolder : RecyclerView.ViewHolder
    {
        public TextView name;
        public Button delete, modify;

        public WaitersViewHolder(View itemView) : base(itemView)
        {
            name = itemView.FindViewById<TextView>(Resource.Id.idName);
            delete = itemView.FindViewById<Button>(Resource.Id.idDelete);
            modify = itemView.FindViewById<Button>(Resource.Id.idModify);
        }
    }
}
}