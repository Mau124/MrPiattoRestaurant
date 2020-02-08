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

using MrPiattoRestaurant.adapters.actualListAdapters;
using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class ActualFragment : Fragment
    {
        Button newGuest;
        RecyclerView mRecyclerView;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public ActualListAdapter mAdapter;

        public List<ActualList> actualList = new List<ActualList>();

        public ActualFragment(List<ActualList> actualList)
        {
            this.actualList = actualList;
        }
        public ActualFragment()
        {
            actualList = new List<ActualList>();
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.recycler_actualList, container, false);

            newGuest = view.FindViewById<Button>(Resource.Id.idButton);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            mLayoutManager = new LinearLayoutManager(Application.Context);
            mAdapter = new ActualListAdapter(actualList);
            mAdapter.HasStableIds = true;

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            newGuest.Click += delegate { OnAddGuest(); };

            return view;
        }

        public void OnAddGuest()
        {
            ActualList element = new ActualList("Mauricio Andres Flores Perez", "Mesa 2", 3);
            actualList.Add(element);
            mAdapter = new ActualListAdapter(actualList);
            mRecyclerView.SetAdapter(mAdapter);
            Toast.MakeText(Application.Context, "Se presiono el boton desde actual", ToastLength.Long).Show();
        }
    }
}