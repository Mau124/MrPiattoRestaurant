using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V4.App;

using MrPiattoRestaurant.adapters.waitListAdapters;
using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class WaitFragment : Android.Support.V4.App.Fragment
    {
        Button newWait;
        RecyclerView mRecyclerView;
        Context context;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public WaitListAdapter mAdapter;

        public List<WaitList> waitList = new List<WaitList>();
        public WaitFragment(Context context)
        {
            waitList = new List<WaitList>();
            this.context = context;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.recycler_waitList, container, false);

            newWait = view.FindViewById<Button>(Resource.Id.idButton);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            mLayoutManager = new LinearLayoutManager(Application.Context);
            mAdapter = new WaitListAdapter(context, waitList);

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            newWait.Click += OnAddWait;

            mAdapter.ItemClick += OnItemClick;

            return view;
        }
        void OnItemClick(object sender, int position)
        {
            // Display a toast that briefly shows the enumeration of the selected photo:
            Toast.MakeText(Application.Context, "Se ha presionado un elemento", ToastLength.Long).Show();
        }
        public void OnAddWait(object sender, EventArgs args)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.add_waitList, null);
            Button add;
            ImageView dismiss;
            EditText nameClient, numSeats;
            SeekBar mSeekBar;

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            add = content.FindViewById<Button>(Resource.Id.idAdd);
            dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

            nameClient = content.FindViewById<EditText>(Resource.Id.idName);
            numSeats = content.FindViewById<EditText>(Resource.Id.idSeats);
            mSeekBar = content.FindViewById<SeekBar>(Resource.Id.idSeekBar);

            mSeekBar.Min = 1;
            mSeekBar.Max = 15;

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

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

                if (nameClient.Text.Equals(""))
                {
                    Toast.MakeText(Application.Context, "Coloque un nombre", ToastLength.Long).Show();
                } else if (numSeats.Text.Equals(""))
                {
                    if (validateSeats(numSeats.Hint))
                    {
                        seats = Int32.Parse(numSeats.Hint);
                        if (seats > 0 && seats < 16)
                        {
                            name = nameClient.Text;
                            AddToList(name, seats);
                            alertDialog.Dismiss();
                        }
                    }
                    Toast.MakeText(Application.Context, "Coloque un numero de asientos valido", ToastLength.Long).Show();
                } else
                {
                    if (validateSeats(numSeats.Text))
                    {
                        seats = Int32.Parse(numSeats.Text);
                        if (seats > 0 && seats < 16)
                        {
                            name = nameClient.Text;
                            AddToList(name, seats);
                            alertDialog.Dismiss();
                        }
                    }
                    Toast.MakeText(Application.Context, "Coloque un numero de asientos valido", ToastLength.Long).Show();
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

        public void AddToList(string name, int seats)
        {
            WaitList client = new WaitList(name, seats);
            waitList.Add(client);
            mAdapter = new WaitListAdapter(context, waitList);
            mRecyclerView.SetAdapter(mAdapter);
        }

        public void RemoveFromWaitList(int position)
        {
            waitList.RemoveAt(position);
        }

        public int getDraggedItemPosition()
        {
            return mAdapter.itemDragged;
        }
    }
}