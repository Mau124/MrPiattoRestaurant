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

using MrPiattoRestaurant.adapters.actualListAdapters;
using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class ActualFragment : Android.Support.V4.App.Fragment
    {
        Button newClient;
        RecyclerView mRecyclerView;
        TextView message;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public ActualListAdapter mAdapter;

        public List<Table> ocupiedTables = new List<Table>();

        public Context context;

        public ActualFragment()
        {
            //Default Constructor
        }
        public ActualFragment(Context context, List<Table> ocupiedTables)
        {
            this.context = context;
            this.ocupiedTables = ocupiedTables;
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

            newClient = view.FindViewById<Button>(Resource.Id.idButton);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);
            message = view.FindViewById<TextView>(Resource.Id.idMessage);

            mLayoutManager = new LinearLayoutManager(Application.Context);
            mAdapter = new ActualListAdapter(context, ocupiedTables);

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            if (ocupiedTables.Any())
            {
                mRecyclerView.Visibility = ViewStates.Visible;
                message.Visibility = ViewStates.Gone;
            } 
            else
            {
                mRecyclerView.Visibility = ViewStates.Gone;
                message.Visibility = ViewStates.Visible;
            }

            return view;
        }

        public void Update(List<Table> ocupiedTables)
        {
            this.ocupiedTables = ocupiedTables;
            mAdapter = new ActualListAdapter(context, ocupiedTables);
            mRecyclerView.SetAdapter(mAdapter);
        }
    }
}