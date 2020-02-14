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
        public View view;
        public List<WaitList> waitList;
        public Context context;
        // Event handler for item clicks
        public event EventHandler<int> ItemClick;

        public delegate void ModifyClientEventHandler(object source, int position, WaitList element);

        public event ModifyClientEventHandler ModifyClient;

        protected virtual void OnModifyClient(int position, WaitList element)
        {
            if (ModifyClient != null)
            {
                ModifyClient(this, position, element);
            }
        }
        public WaitListAdapter(List<WaitList> waitList, Context context)
        {
            this.waitList = waitList;
            this.context = context;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_waitList, parent, false);
            WaitListViewHolder vh = new WaitListViewHolder(itemView, OnClick);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            WaitListViewHolder vh = holder as WaitListViewHolder;
            vh.personName.Text = waitList.ElementAt(position).personName;
            vh.seats.Text = waitList.ElementAt(position).numSeats.ToString();

            vh.itemView.LongClick += delegate
            {
                onTouch(vh.itemView);
            };

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
                            LayoutInflater inflater = LayoutInflater.From(Application.Context);
                            View content = inflater.Inflate(Resource.Layout.modify_waitList, null);
                            Button add, cancel;
                            EditText nameClient, numSeats;

                            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
                            alertDialog.SetCancelable(true);
                            alertDialog.SetView(content);
                            alertDialog.Show();
                            Toast.MakeText(Application.Context, "Position: " + position + " Opcion 2", ToastLength.Short).Show();
                            break;
                        case Resource.Id.item3:
                            waitList.RemoveAt(position);
                            NotifyDataSetChanged();
                            Toast.MakeText(Application.Context, "Position: " + position + " Opcion 3", ToastLength.Short).Show();
                            break;
                    }
                };

                menu.Show();
            };
        }

        public void onTouch(View itemView)
        {
            var data = ClipData.NewPlainText("name", "Element 1");
            itemView.StartDrag(data, new View.DragShadowBuilder(itemView), null, 0);
            Toast.MakeText(Application.Context, "Se presiono largo", ToastLength.Short).Show();
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
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
            public View itemView;
            public TextView personName, seats, menu;

            public WaitListViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                this.itemView = itemView;

                personName = itemView.FindViewById<TextView>(Resource.Id.idPersonName);
                seats = itemView.FindViewById<TextView>(Resource.Id.idSeats);
                menu = itemView.FindViewById<TextView>(Resource.Id.idViewOptions);

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
            }
        }
    }
}