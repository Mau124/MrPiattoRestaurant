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

using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.adapters
{
    public class AssigmentAdapter : RecyclerView.Adapter
    {
        private List<GestureRecognizerView> floors = new List<GestureRecognizerView>();
        private List<Models.Notification> reservations = new List<Models.Notification>();
        private Context context;

        private APIUpdate APIupdate = new APIUpdate();
        private APICaller API = new APICaller();
        private Restaurant restaurant = new Restaurant();

        public delegate void FinishSelectionHandler();
        public event FinishSelectionHandler FinishSelection;
        protected virtual void OnFinishSelection()
        {
            if (FinishSelection != null)
            {
                FinishSelection();
            }
        }

        public AssigmentAdapter(Context context, List<Models.Notification> reservations, List<GestureRecognizerView> floors, Restaurant restaurant)
        {
            this.context = context;
            this.reservations = reservations;
            this.floors = floors;
            this.restaurant = restaurant;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_notifications, parent, false);
            ReservationsViewHolder vh = new ReservationsViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            holder.IsRecyclable = false;
            ReservationsViewHolder vh = holder as ReservationsViewHolder;

            // Load the photo image resource from the photo album:

            // Load the photo caption from the photo album:
            vh.personName.Text = reservations.ElementAt(position).Name + " " + reservations.ElementAt(position).LastName;
            vh.tableName.Text = reservations.ElementAt(position).TableName;
            vh.date.Text = reservations.ElementAt(position).Date.ToString();
            vh.phone.Text = reservations.ElementAt(position).Phone;

            // Select table
            vh.ItemView.Click += delegate
            {
                if (reservations[position].type == 0)
                {
                    int tableIndex = floors[reservations[position].floorIndex].tables.FindIndex(t => t.Id == reservations[position].tableIndex);
                    Client client = new Client(reservations[position].Name + " " + reservations[position].LastName, 0, DateTime.Now, floors[reservations[position].floorIndex].tables[tableIndex].seats);

                    floors.ElementAt(reservations[position].floorIndex).tables.ElementAt(tableIndex).setOcupied(true);
                    floors.ElementAt(reservations[position].floorIndex).Draw();

                    floors.ElementAt(reservations[position].floorIndex).setActualClientOnTable(client, tableIndex);

                    OnFinishSelection();
                } else if (reservations[position].type == 1)
                {
                    Client client = new Client(reservations[position].Name + " " + reservations[position].LastName, 0, DateTime.Now, reservations[position].seats);

                    List<RestaurantTables> tables = API.GetTables(restaurant.Idrestaurant);
                    List<Table> tablesToJoin = new List<Table>();

                    int floorIndex = tables.Where(t => t.Idtables == reservations[position].tablesID[0]).Select(t => t.floorIndex).First();
                    int seats = new int();

                    foreach (int id in reservations[position].tablesID)
                    {
                        tablesToJoin.Add(floors[floorIndex].tables.Where(t => t.Id == id).First());
                    }
                    
                    foreach (Table t in tablesToJoin)
                    {
                        seats += t.seats;
                    }

                    floors[floorIndex].Union(tablesToJoin, client, seats);

                    OnFinishSelection();
                }
            };
        }

        public override int ItemCount
        {
            get { return reservations.Count; }
        }

        public class ReservationsViewHolder : RecyclerView.ViewHolder
        {
            public TextView personName, tableName, date, phone;

            public ReservationsViewHolder(View itemView) : base(itemView)
            {
                personName = itemView.FindViewById<TextView>(Resource.Id.idPersonName);
                tableName = itemView.FindViewById<TextView>(Resource.Id.idtableName);
                date = itemView.FindViewById<TextView>(Resource.Id.idDate);
                phone = itemView.FindViewById<TextView>(Resource.Id.idPhone);
            }
        }
    }
}