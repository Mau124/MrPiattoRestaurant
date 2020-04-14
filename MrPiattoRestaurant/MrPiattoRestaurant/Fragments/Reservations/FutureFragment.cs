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

using MrPiattoRestaurant.adapters.futureListAdapters;
using MrPiattoRestaurant.Models.Reservations;
using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.Pickers;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class FutureFragment : Android.Support.V4.App.Fragment
    {
        Button newReservation;
        RecyclerView mRecyclerView;

        private Context context;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public FutureListAdapter mAdapter;

        public List<Client> futureList = new List<Client>();

        public FutureFragment()
        {
            futureList = new List<Client>();
        }

        public FutureFragment(Context context, List<Client> futureList)
        {
            this.context = context;
            this.futureList = futureList;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.recycler_futureList, container, false);

            newReservation = view.FindViewById<Button>(Resource.Id.idButton);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            mLayoutManager = new LinearLayoutManager(Application.Context);
            mAdapter = new FutureListAdapter(context, futureList);
            mAdapter.HasStableIds = true;

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            newReservation.Click += delegate { OnAddReservation(); };
            return view;
        }

        public void OnAddReservation()
        {
            Client element = new Client("Mauricio Andres Flores Perez", 3, DateTime.Now, 3);
            futureList.Add(element);
            mAdapter = new FutureListAdapter(context, futureList);
            mRecyclerView.SetAdapter(mAdapter);
            Toast.MakeText(Application.Context, "Se presiono el boton desde future", ToastLength.Long).Show();

            EditText name, tel;
            TextView date, hour, numSeats;
            SeekBar mSeekBar;
            ImageView dismiss;

            View content = LayoutInflater.Inflate(Resource.Layout.layout_manual_reservation, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);
            name = content.FindViewById<EditText>(Resource.Id.idName);
            tel = content.FindViewById<EditText>(Resource.Id.idTel);
            numSeats = content.FindViewById<EditText>(Resource.Id.idNumSeats);
            date = content.FindViewById<TextView>(Resource.Id.idDate);
            hour = content.FindViewById<TextView>(Resource.Id.idHour);
            mSeekBar = content.FindViewById<SeekBar>(Resource.Id.idSeekBar);

            mSeekBar.Min = 1;
            mSeekBar.Max = 15;

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

            numSeats.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
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

            date.Click += (s, a) =>
            {
                Toast.MakeText(Application.Context, "Se esta presionando el seek", ToastLength.Long).Show();
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    date.Text = time.ToLongDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            hour.Click += (s, a) =>
            {
                TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
                {
                    hour.Text = time.ToString("hh:mm tt");
                });
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };
        }
    }
}