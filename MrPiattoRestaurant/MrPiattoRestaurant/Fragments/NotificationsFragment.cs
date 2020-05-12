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
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.Resources.utilities;


namespace MrPiattoRestaurant.Fragments
{
    public class NotificationsFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        TextView message;

        private Context context;
        private Restaurant restaurant;
        private APICaller API = new APICaller();

        public RecyclerView.LayoutManager mLayoutManager;
        public NotificationsAdapter mAdapter;

        public List<Models.Notification> NotificationsList = new List<Models.Notification>();
        public NotificationsFragment(Context context, Restaurant restaurant)
        {
            this.context = context;
            this.restaurant = restaurant;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            InitializeNotifications();
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

        private void InitializeNotifications()
        {
            List<Reservation>? reservations = API.GetNotReservations(restaurant.Idrestaurant);
            List<AuxiliarReservation>? auxReservations = API.GetNotAuxReservations(restaurant.Idrestaurant);
            List<ManualReservation>? manReservations = API.GetNotManReservations(restaurant.Idrestaurant);

            foreach (Reservation r in reservations)
            {
                NotificationsList.Add(new Models.Notification(r.IduserNavigation.FirstName, r.IduserNavigation.LastName, r.IdtableNavigation.tableName, r.Date, "Reservado desde celular"));
            }

            foreach (AuxiliarReservation r in auxReservations)
            {
                NotificationsList.Add(new Models.Notification(r.Name, r.LastName, "Union", r.Date, r.Phone));
            }

            foreach (ManualReservation r in manReservations)
            {
                NotificationsList.Add(new Models.Notification(r.Name, r.LastName, r.IdtableNavigation.tableName, r.Date, r.Phone));
            }
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