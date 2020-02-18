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
        Context context;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public WaitListAdapter mAdapter;

        public List<WaitList> waitList = new List<WaitList>();

        //We define a delegate for our tablepressed event
        public delegate void AddClientEventHandler(object source, EventArgs args);

        //We define an event based on the tablepressed delegate
        public event AddClientEventHandler AddClient;
        //Raise the event
        protected virtual void OnAddClient()
        {
            if (AddClient != null)
            {
                AddClient(this, EventArgs.Empty);
            }
        }
        public WaitFragment (List<WaitList> waitList)
        {
            this.waitList = waitList;
        }

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
            mAdapter = new WaitListAdapter(waitList, context);

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
            OnAddClient();
            Toast.MakeText(Application.Context, "Se presiono el boton desde wait", ToastLength.Long).Show();
        }

        public void AddToList(string name, int seats)
        {
            WaitList client = new WaitList(name, seats);
            waitList.Add(client);
            mAdapter = new WaitListAdapter(waitList, context);
            mRecyclerView.SetAdapter(mAdapter);
        }
    }
}