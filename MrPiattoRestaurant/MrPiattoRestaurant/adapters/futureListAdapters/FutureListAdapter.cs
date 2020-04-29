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

using MrPiattoRestaurant.Models;

using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.adapters.futureListAdapters
{
    public class FutureListAdapter : RecyclerView.Adapter
    {
        private Context context;
        public List<Client> futureList;
        public FutureListAdapter(Context context, List<Client> futureList)
        {
            this.context = context;
            this.futureList = futureList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_futureList, parent, false);
            FutureListViewHolder vh = new FutureListViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            FutureListViewHolder vh = holder as FutureListViewHolder;
            vh.personName.Text = futureList.ElementAt(position).name;
            vh.tableName.Text = futureList.ElementAt(position).floorName + ", " + futureList.ElementAt(position).tableName;
            vh.seats.Text = futureList.ElementAt(position).Seats.ToString() + " sillas";
            vh.date.Text = futureList.ElementAt(position).reservationDate.ToString();

            vh.menu.Click += (s, arg) =>
            {
                Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(Application.Context, vh.menu);
                menu.Inflate(Resource.Menu.futureListMenu);

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
                            Toast.MakeText(Application.Context, "Position: " + position + " Opcion 2", ToastLength.Short).Show();
                            break;
                    }
                };

                menu.Show();
            };
        }

        public override int ItemCount
        {
            get { return futureList.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public class FutureListViewHolder : RecyclerView.ViewHolder
        {
            public TextView personName, tableName, seats, date, reservationCode, menu;

            public FutureListViewHolder(View itemView) : base(itemView)
            {
                personName = itemView.FindViewById<TextView>(Resource.Id.idPersonName);
                tableName = itemView.FindViewById<TextView>(Resource.Id.idtableName);
                seats = itemView.FindViewById<TextView>(Resource.Id.idtableSeats);
                date = itemView.FindViewById<TextView>(Resource.Id.idDate);
                //reservationCode = itemView.FindViewById<TextView>(Resource.Id.idReservationCode);
                menu = itemView.FindViewById<TextView>(Resource.Id.idViewOptions);
            }
        }
    }
}