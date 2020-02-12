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

using MrPiattoRestaurant.adapters.futureListAdapters;
using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class FutureFragment : Fragment
    {
        Button newReservation;
        RecyclerView mRecyclerView;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public FutureListAdapter mAdapter;

        public List<FutureList> futureList = new List<FutureList>();

        public FutureFragment(List<FutureList> futureList)
        {
            this.futureList = futureList;
        }
        public FutureFragment()
        {
            futureList = new List<FutureList>();
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
            mAdapter = new FutureListAdapter(futureList);
            mAdapter.HasStableIds = true;

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            newReservation.Click += delegate { OnAddReservation(); };
            return view;
        }

        public void OnAddReservation()
        {
            FutureList element = new FutureList("Mauricio Andres Flores Perez", "Mesa 2", 3);
            futureList.Add(element);
            mAdapter = new FutureListAdapter(futureList);
            mRecyclerView.SetAdapter(mAdapter);
            Toast.MakeText(Application.Context, "Se presiono el boton desde future", ToastLength.Long).Show();
        }
    }
}