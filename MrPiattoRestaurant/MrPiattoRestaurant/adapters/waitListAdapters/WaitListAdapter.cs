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
        public int itemDragged { get; set; }
        // Event handler for item clicks
        public event EventHandler<int> ItemClick;
        public WaitListAdapter(Context context, List<WaitList> waitList)
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
            holder.IsRecyclable = false;
            WaitListViewHolder vh = holder as WaitListViewHolder;
            vh.personName.Text = waitList.ElementAt(position).personName;
            vh.seats.Text = waitList.ElementAt(position).numSeats.ToString();

            vh.itemView.LongClick += delegate
            {
                onTouch(vh.itemView);
                itemDragged = position;
            };

            vh.menu.Click += delegate { OnMenuClick(position, vh);  };
        }

        public void OnMenuClick(int position, WaitListViewHolder vh)
        {
            Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(Application.Context, vh.menu);
            menu.Inflate(Resource.Menu.waitListMenu);

            menu.MenuItemClick += (s1, arg1) =>
            {
                switch (arg1.Item.ItemId)
                {
                    case Resource.Id.item2:
                        ModifyClient(position);
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

        public void ModifyClient(int position)
        {
            LayoutInflater inflater = LayoutInflater.From(Application.Context);
            View content = inflater.Inflate(Resource.Layout.add_waitList, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            Button add, cancel;
            EditText nameClient, numSeats;
            SeekBar mSeekBar;

            add = content.FindViewById<Button>(Resource.Id.idAdd);

            nameClient = content.FindViewById<EditText>(Resource.Id.idName);
            numSeats = content.FindViewById<EditText>(Resource.Id.idSeats);
            mSeekBar = content.FindViewById<SeekBar>(Resource.Id.idSeekBar);

            add.Text = "Modificar";

            mSeekBar.Min = 1;
            mSeekBar.Max = 15;

            nameClient.Hint = waitList.ElementAt(position).personName;
            numSeats.Hint = waitList.ElementAt(position).numSeats.ToString();
            mSeekBar.Progress = Int32.Parse(numSeats.Hint);

            numSeats.TextChanged += (object s, Android.Text.TextChangedEventArgs e) =>
            {
                if (validateSeats(numSeats.Text))
                    mSeekBar.Progress = Int32.Parse(numSeats.Text);
            };

            mSeekBar.ProgressChanged += (object senderProgresBar, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    numSeats.Hint = e.Progress.ToString();
                    Toast.MakeText(Application.Context, "Se esta presionando el seek", ToastLength.Long).Show();
                }
            };

            add.Click += (s, a) => {
                string name;
                int seats;

                try
                {
                    if (!nameClient.Text.Equals(""))
                    {
                        name = nameClient.Text;
                        waitList.ElementAt(position).personName = name;
                    }
                    if (!numSeats.Text.Equals("") && validateSeats(numSeats.Text))
                    {
                        seats = Int32.Parse(numSeats.Text);
                        waitList.ElementAt(position).numSeats = seats;
                    } else if (!numSeats.Hint.Equals("") && validateSeats(numSeats.Hint))
                    {
                        seats = Int32.Parse(numSeats.Hint);
                        waitList.ElementAt(position).numSeats = seats;
                    }
                    alertDialog.Dismiss();

                    List<WaitList> auxList = new List<WaitList>(waitList);
                    NotifyDataSetChanged();
                } catch
                {
                    Toast.MakeText(Application.Context, "Error", ToastLength.Long).Show();
                }
            };
        }

        private bool validateSeats(string s)
        {
            if (s.Count() == 0) return false;
            foreach (char c in s)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
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