﻿using System;
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
        //We define a delegate for our tablepressed event
        public delegate void ChangeItemEventHandler(object sender, EventArgs e);

        //We define an event based on the tablepressed delegate
        public event ChangeItemEventHandler ChangeItem;

        //Raise the event
        protected virtual void OnChangeItem()
        {
            if (ChangeItem != null)
            {
                ChangeItem(this, EventArgs.Empty);
            }
        }

        private List<Waiters> waiters = new List<Waiters>();
        private Context context;

        private APIUpdate APIupdate = new APIUpdate();
        private APIDelete APIdelete = new APIDelete();
        public WaitersAdapter(Context context, List<Waiters> waiters)
        {
            this.context = context;
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
            vh.name.Text = waiters[position].WaiterFirstName + " " + waiters[position].WaiterLasName;

            vh.delete.Click += async delegate
            {
                var response = await APIdelete.DeleteWaiter(waiters[position]);
                waiters.RemoveAt(position);
                Toast.MakeText(context, response, ToastLength.Long).Show();
                NotifyDataSetChanged();
            };

            vh.modify.Click += delegate
            {
                LayoutInflater mLayout = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService); 
                View content = mLayout.Inflate(Resource.Layout.layout_add_waiter, null);

                TextView textTitle = content.FindViewById<TextView>(Resource.Id.idTextTitle);
                EditText name = content.FindViewById<EditText>(Resource.Id.idName);
                EditText lastName = content.FindViewById<EditText>(Resource.Id.idLastName);
                Button button = content.FindViewById<Button>(Resource.Id.idButton);
                ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

                textTitle.Text = "Modificar mesero";
                name.Hint = waiters[position].WaiterFirstName;
                lastName.Hint = waiters[position].WaiterLasName;
                button.Text = "Modificar"; 

                Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
                alertDialog.SetCancelable(true);
                alertDialog.SetView(content);
                alertDialog.Show();

                dismiss.Click += delegate
                {
                    alertDialog.Dismiss();
                };

                button.Click += async delegate
                {
                    if (name.Text.Equals("") || lastName.Text.Equals(""))
                    {
                        Toast.MakeText(context, "Falta algun campo" , ToastLength.Long).Show();
                    } else
                    {
                        Waiters waiter = new Waiters();

                        waiter.Idwaiter = waiters[position].Idwaiter;
                        waiter.Idrestaurant = waiters[position].Idrestaurant;
                        waiter.WaiterFirstName = name.Text;
                        waiter.WaiterLasName = lastName.Text;

                        var response = await APIupdate.UpdateWaiters(waiter);
                        Toast.MakeText(context, response, ToastLength.Long).Show();

                        waiters[position].WaiterFirstName = name.Text;
                        waiters[position].WaiterLasName = lastName.Text;

                        OnChangeItem();

                        alertDialog.Dismiss();
                    }
                };
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