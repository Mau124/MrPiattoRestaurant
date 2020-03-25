using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Support.V4.App;
using Android.Support.Design.Widget;

using MrPiattoRestaurant.adapters;
using MrPiattoRestaurant.Models.Reservations;
using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.Pickers;


namespace MrPiattoRestaurant.Fragments
{
    public class NotificationsFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        TextView message;

        Context context;

        public RecyclerView.LayoutManager mLayoutManager;
        public NotificationsAdapter mAdapter;

        public List<Client> NotificationsList = new List<Client>();
        public NotificationsFragment(Context context)
        {
            this.context = context;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            Client client = new Client("Juan", 30, DateTime.Now);

            NotificationsList.Add(client);
            NotificationsList.Add(client);
            NotificationsList.Add(client);
            NotificationsList.Add(client);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.recycler_notifications, container, false);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);
            message = view.FindViewById<TextView>(Resource.Id.idMessage);

            mLayoutManager = new LinearLayoutManager(context);
            mAdapter = new NotificationsAdapter(context, NotificationsList);

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            var swipeHandler = new SwipeToDeleteCallback(0, ItemTouchHelper.Left, context, mAdapter);
            var itemTouchHelper = new ItemTouchHelper(swipeHandler);
            itemTouchHelper.AttachToRecyclerView(mRecyclerView);

            if (NotificationsList.Any())
            {
                mRecyclerView.Visibility = ViewStates.Visible;
                message.Visibility = ViewStates.Gone;
            } else
            {
                mRecyclerView.Visibility = ViewStates.Gone;
                message.Visibility = ViewStates.Visible;
            }

            return view;
        }
    }

    public class SwipeToDeleteCallback : ItemTouchHelper.SimpleCallback
    {
        private NotificationsAdapter mAdapter;
        private Context context;

        public SwipeToDeleteCallback(int dragDirs, int swipeDirs, Context context) : base(dragDirs, swipeDirs)
        {
            this.context = context;
        }
        public SwipeToDeleteCallback(int dragDirs, int swipeDirs, Context context, NotificationsAdapter mRecyclerView) : this(dragDirs, swipeDirs, context)
        {
            this.context = context;
            this.mAdapter = mRecyclerView;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int position = viewHolder.AdapterPosition;
            mAdapter.RemoveItem(position);
            mAdapter.NotifyDataSetChanged();
            Toast.MakeText(Application.Context, "Posicion " + position, ToastLength.Short).Show();
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
        {
            return base.GetMovementFlags(p0, p1);
        }
    }
}