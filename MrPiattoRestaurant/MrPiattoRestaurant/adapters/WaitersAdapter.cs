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

using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.adapters
{
    public class WaitersAdapter : RecyclerView.Adapter
    {
        List<Waiters> waiters = new List<Waiters>();
        private Context context;
        public WaitersAdapter(List<Waiters> waiters, Context context)
        {
            this.waiters = waiters;
            this.context = context;
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
            vh.name.Text = waiters[position].WaiterFirstName + " " + waiters[position].WaiterLasName;

            vh.delete.Click += delegate
            {
                waiters.RemoveAt(position);
                NotifyDataSetChanged();
            };

            vh.modify.Click += delegate
            {
                LayoutInflater mLayout = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService); 
                View content = mLayout.Inflate(Resource.Layout.layout_add_waiter, null);

                TextView textTitle = content.FindViewById<TextView>(Resource.Id.idTextTitle);
                EditText name = content.FindViewById<EditText>(Resource.Id.idName);
                Button button = content.FindViewById<Button>(Resource.Id.idButton);

                textTitle.Text = "Modificar mesero";
                name.Hint = waiters[position].WaiterFirstName + " " + waiters[position].WaiterLasName;
                button.Text = "Modificar"; 


                Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
                alertDialog.SetCancelable(true);
                alertDialog.SetView(content);
                alertDialog.Show();
            };
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