﻿using System;
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
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class FutureFragment : Android.Support.V4.App.Fragment
    {
        // Evento para notificar al main que hay que unir mesas
        public delegate void UnionPressedEventHandler(Client client);
        public event UnionPressedEventHandler UnionPressed;
        protected virtual void OnUnionPressed(Client client)
        {
            if (UnionPressed != null)
            {
                UnionPressed(client);
            }
        }

        Button newReservation;
        RecyclerView mRecyclerView;

        TextView interval1;
        TextView interval2;

        DateTime dInterval1;
        DateTime dInterval2;

        private Context context;

        //RecyclerView elements
        private RecyclerView.LayoutManager mLayoutManager;
        private FutureListAdapter mAdapter;

        private List<Client> futureList = new List<Client>();
        private List<Reservation> reservations = new List<Reservation>();
        private Restaurant restaurant = new Restaurant();
        private APICaller API = new APICaller();

        private DateTime dateHour;
        private DateTime dateDay;

        public FutureFragment()
        {
            futureList = new List<Client>();
        }

        public FutureFragment(Context context, Restaurant restaurant)
        {
            this.context = context;
            this.restaurant = restaurant;
            reservations = API.GetReservations(restaurant.Idrestaurant);
        }

        private void updateRes()
        {
            futureList.Clear();
            List<Reservation> reservations2 = new List<Reservation>();
            List<AuxiliarReservation>? auxReservations = API.GetAuxReservationsWithoutFilters(restaurant.Idrestaurant).Where(r => r.CheckedFromApp != true && r.Date >= dInterval1 && r.Date <= dInterval2).ToList();
            List<ManualReservations>? manReservations = API.GetManualReservationsWithoutFilters(restaurant.Idrestaurant).Where(r => r.CheckedFromApp != true && r.Date >= dInterval1 && r.Date <= dInterval2).ToList();
            reservations2 = reservations.Where(d => d.Date >= dInterval1 && d.Date <= dInterval2).ToList();
            
            foreach (Reservation res in reservations2)
            {
                Client c = new Client();
                c.name = res.IduserNavigation.FirstName + " " + res.IduserNavigation.LastName;
                c.timeUsed = 0;
                c.reservationDate = res.Date;
                c.seats = res.AmountOfPeople;
                c.floorName = res.IdtableNavigation.FloorName;
                c.tableName = res.IdtableNavigation.tableName;
                c.Phone = "Reservado desde el celular";
                futureList.Add(c);
            }

            foreach (AuxiliarReservation aux in auxReservations)
            {
                Client c = new Client();
                c.name = aux.Name + " " + aux.LastName;
                c.timeUsed = 0;
                c.reservationDate = aux.Date;
                c.seats = aux.AmountOfPeople;
                c.floorName = aux.IdauxiliarTableNavigation.FloorName;
                c.tableName = "Union";
                c.Phone = aux.Phone;
                futureList.Add(c);
            }

            foreach (ManualReservations man in manReservations)
            {
                Client c = new Client();
                c.name = man.Name + " " + man.LastName;
                c.timeUsed = 0;
                c.reservationDate = man.Date;
                c.seats = man.AmountOfPeople;
                c.floorName = man.IdtableNavigation.FloorName;
                c.tableName = man.IdtableNavigation.tableName;
                c.Phone = man.Phone;
                futureList.Add(c);
            }
            futureList.OrderBy(d => d.reservationDate);

            mAdapter = new FutureListAdapter(context, futureList);
            mAdapter.HasStableIds = true;
            mRecyclerView.SetAdapter(mAdapter);
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
            interval1 = view.FindViewById<TextView>(Resource.Id.idInterval1);
            interval2 = view.FindViewById<TextView>(Resource.Id.idInterval2);

            mLayoutManager = new LinearLayoutManager(Application.Context);
            mAdapter = new FutureListAdapter(context, futureList);
            mAdapter.HasStableIds = true;

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            newReservation.Click += delegate { OnAddReservation(); };

            interval1.Click += OnInterval1;
            interval2.Click += OnInterval2;

            initializeIntervals();
            updateRes();

            return view;
        }

        public void OnAddReservation()
        {
            EditText name, tel;
            TextView date, hour, numSeats;
            SeekBar mSeekBar;
            ImageView dismiss;
            Button button;

            View content = LayoutInflater.Inflate(Resource.Layout.layout_manual_reservation, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);
            button = content.FindViewById<Button>(Resource.Id.idButton);
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

            button.Click += delegate 
            {
                DateTime auxDate = new DateTime(dateDay.Year, dateDay.Month, dateDay.Day, dateHour.Hour, dateHour.Minute, 0);
                //string auxName = name.Text;
                //int auxNumSeats =  Int32.Parse(numSeats.Text);

                Client client = new Client(name.Text, "", auxDate, Int32.Parse(numSeats.Hint), tel.Text);
                OnUnionPressed(client);
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
                    dateDay = time;
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            hour.Click += (s, a) =>
            {
                TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
                {
                    hour.Text = time.ToString("hh:mm tt");
                    dateHour = time;
                });
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };
        }

        private void initializeIntervals()
        {
            if (reservations.Any())
            {
                dInterval1 = reservations.Min(d => d.Date);
                dInterval2 = reservations.Max(d => d.Date);


                interval1.Text = dInterval1.ToString("dd/MM/yyyy");
                interval2.Text = dInterval2.ToString("dd/MM/yyyy");
            }
        }

        private void OnInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                interval1.Text = time.ToString("dd/MM/yyyy");
                dInterval1 = time;
                updateRes();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void OnInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                interval2.Text = time.ToString("dd/MM/yyyy");
                dInterval2 = time;
                updateRes();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
    }
}