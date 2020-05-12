using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using MrPiattoRestaurant.Models;

using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.adapters
{
    public class NotificationsAdapter : RecyclerView.Adapter
    {
        private Context context;
        public List<Models.Notification> NotificationsList;
        public NotificationsAdapter(Context context, List<Models.Notification> NotificationsList)
        {
            this.context = context;
            this.NotificationsList = NotificationsList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_notifications, parent, false);
            NotificationsListViewHolder vh = new NotificationsListViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            NotificationsListViewHolder vh = holder as NotificationsListViewHolder;
            vh.personName.Text = NotificationsList.ElementAt(position).Name + " " + NotificationsList.ElementAt(position).LastName;
            vh.tableName.Text = NotificationsList.ElementAt(position).TableName;
            vh.date.Text = NotificationsList.ElementAt(position).Date.ToString();
            vh.phone.Text = NotificationsList.ElementAt(position).Phone;
        }

        public override int ItemCount
        {
            get { return NotificationsList.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public void RemoveItem(int position)
        {
            NotificationsList.RemoveAt(position);
        }
        public class NotificationsListViewHolder : RecyclerView.ViewHolder
        {
            public TextView personName, tableName, date, phone;

            public NotificationsListViewHolder(View itemView) : base(itemView)
            {
                personName = itemView.FindViewById<TextView>(Resource.Id.idPersonName);
                tableName = itemView.FindViewById<TextView>(Resource.Id.idtableName);
                date = itemView.FindViewById<TextView>(Resource.Id.idDate);
                phone = itemView.FindViewById<TextView>(Resource.Id.idPhone);
            }
        }
    }
}