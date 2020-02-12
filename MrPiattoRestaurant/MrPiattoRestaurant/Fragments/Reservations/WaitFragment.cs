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

using MrPiattoRestaurant.adapters.waitListAdapters;
using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class WaitFragment : Fragment
    {
        Button newWait;
        RecyclerView mRecyclerView;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public WaitListAdapter mAdapter;

        public List<WaitList> waitList = new List<WaitList>();

        public WaitFragment (List<WaitList> waitList)
        {
            this.waitList = waitList;
        }

        public WaitFragment()
        {
            waitList = new List<WaitList>();
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
            mAdapter = new WaitListAdapter(waitList);

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            newWait.Click += delegate { OnAddWait(); };

            return view;
        }

        public void OnAddWait()
        {
            WaitList element = new WaitList("Mauricio Andres Flores Perez", 5);
            waitList.Add(element);
            mAdapter = new WaitListAdapter(waitList);
            mRecyclerView.SetAdapter(mAdapter);
            Toast.MakeText(Application.Context, "Se presiono el boton desde wait", ToastLength.Long).Show();
        }
    }
}