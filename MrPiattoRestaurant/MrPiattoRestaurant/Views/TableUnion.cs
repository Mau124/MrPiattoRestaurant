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

using MrPiattoRestaurant.adapters;
using MrPiattoRestaurant.Models;

namespace MrPiattoRestaurant.Views
{
    class TableUnion
    {
        private Context context;
        private int floorIndex;
        private int tableIndex;

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private UnionAdapter mAdapter;

        List<GestureRecognizerView> floors;
        Client client = new Client();

        public delegate void ClosePressedEventHandler();
        public event ClosePressedEventHandler ClosePressed;
        protected virtual void OnClosePressed()
        {
            if (ClosePressed != null)
            {
                ClosePressed();
            }
        }

        /// <summary>
        /// Class that creates a tablepropertiesView
        /// </summary>
        /// <param name="context">Context of activity</param>
        /// <param name="floors">List of floors</param>
        /// <param name="floorIndex">Actual floor Index</param>
        /// <param name="tableIndex">Actual table Index in that floor</param>
        public TableUnion(Context context, Client client, List<GestureRecognizerView> floors, int floorIndex, int tableIndex)
        {
            this.context = context;
            this.client = client;
            this.floors = floors;
            this.floorIndex = floorIndex;
            this.tableIndex = tableIndex;

            floors[floorIndex].InitializeUnion(floors[floorIndex].tables[tableIndex]);
        }

        /// <summary>
        /// Creates and shows the table properties View
        /// </summary>
        /// <param name="viewGroup">Container in which the table will be shown</param>
        /// <returns></returns>
        public View CreateView(ViewGroup viewGroup)
        {
            LayoutInflater inflater = LayoutInflater.From(context);
            View tableUnion = inflater.Inflate(Resource.Layout.layout_union_tables, viewGroup, true);

            mRecyclerView = tableUnion.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);
            Button button = tableUnion.FindViewById<Button>(Resource.Id.idButton);


            mLayoutManager = new LinearLayoutManager(context);
            mAdapter = new UnionAdapter(context, floors[floorIndex].auxTables);

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            foreach (Table t in floors[floorIndex].tables)
            {
                if (t.isOcupied || t.nextReservations())
                {
                    t.setSelected(0);
                }
            }

            floors[floorIndex].Draw();

            floors[floorIndex].TableSelected += tableSelected;
            floors[floorIndex].TableDisSelect += tableDisSelected;

            button.Click += delegate
            {
                int seats = 0;
                foreach (Table t in floors[floorIndex].auxTables)
                {
                    seats += t.seats;
                }
                if (seats >= client.Seats && seats < 13)
                {
                    floors[floorIndex].Union(floors[floorIndex].auxTables, client, seats);
                    OnClosePressed();
                    Toast.MakeText(Application.Context, "El numero de sillas es: " + seats, ToastLength.Short).Show();
                } else
                {
                    Toast.MakeText(Application.Context, "El numero de sillas en las mesas seleccionadas debe ser suficiente para el cliente y menor a 13", ToastLength.Short).Show();
                }
            };

            return tableUnion;
        }

        private void tableSelected(Table table)
        {
            mAdapter = new UnionAdapter(context, floors[floorIndex].auxTables);
            mRecyclerView.SetAdapter(mAdapter);
        }

        private void tableDisSelected(Table table)
        {
            mAdapter = new UnionAdapter(context, floors[floorIndex].auxTables);
            mRecyclerView.SetAdapter(mAdapter);
        }
    }
}