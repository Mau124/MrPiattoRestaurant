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

using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.adapters.waitListAdapters
{
    public class WaitListAdapter : RecyclerView.Adapter
    {
        public List<WaitList> waitList;

        public WaitListAdapter(List<WaitList> waitList)
        {
            this.waitList = waitList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_waitList, parent, false);
            WaitListViewHolder vh = new WaitListViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            WaitListViewHolder vh = holder as WaitListViewHolder;
            vh.personName.Text = waitList.ElementAt(position).personName;
            vh.seats.Text = waitList.ElementAt(position).numSeats.ToString();

            vh.menu.Click += (s, arg) =>
            {
                Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(Application.Context, vh.menu);
                menu.Inflate(Resource.Menu.waitListMenu);

                menu.MenuItemClick += (s1, arg1) =>
                {
                    switch (arg1.Item.ItemId)
                    {
                        case Resource.Id.item1:
                            Toast.MakeText(Application.Context, "Posicion: " + position + " Opcion 1", ToastLength.Short).Show();
                            break;
                        case Resource.Id.item2:
                            Toast.MakeText(Application.Context, "Position: " + position + " Opcion 2", ToastLength.Short).Show();
                            break;
                        case Resource.Id.item3:
                            Toast.MakeText(Application.Context, "Position: " + position + " Opcion 3", ToastLength.Short).Show();
                            break;
                    }
                };

                menu.Show();
            };
        }

        public override int ItemCount
        {
            get { return waitList.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public class WaitListViewHolder : RecyclerView.ViewHolder
        {
            public TextView personName, seats, menu;

            public WaitListViewHolder(View itemView) : base(itemView)
            {
                personName = itemView.FindViewById<TextView>(Resource.Id.idPersonName);
                seats = itemView.FindViewById<TextView>(Resource.Id.idSeats);
                menu = itemView.FindViewById<TextView>(Resource.Id.idViewOptions);
            }
        }
    }
}