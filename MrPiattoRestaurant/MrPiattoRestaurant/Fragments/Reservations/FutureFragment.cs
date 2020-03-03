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
            Client element = new Client("Mauricio Andres Flores Perez", 3, DateTime.Now);
            futureList.Add(element);
            mAdapter = new FutureListAdapter(context, futureList);
            mRecyclerView.SetAdapter(mAdapter);
            Toast.MakeText(Application.Context, "Se presiono el boton desde future", ToastLength.Long).Show();
        }
    }
}