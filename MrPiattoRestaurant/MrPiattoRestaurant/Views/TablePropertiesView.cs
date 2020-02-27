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

using MrPiattoRestaurant.adapters.futureListAdapters;

namespace MrPiattoRestaurant.Views
{
    /// <summary>
    /// Class that creates and manages a table properties view
    /// </summary>
    public class TablePropertiesView
    {
        private Context context;

        private int floorIndex;
        private int tableIndex;

        List<GestureRecognizerView> floors;

        /// <summary>
        /// Class that creates a tablepropertiesView
        /// </summary>
        /// <param name="context">Context of activity</param>
        /// <param name="floors">List of floors</param>
        /// <param name="floorIndex">Actual floor Index</param>
        /// <param name="tableIndex">Actual table Index in that floor</param>
        public TablePropertiesView(Context context, List<GestureRecognizerView> floors, int floorIndex, int tableIndex)
        {
            this.context = context;
            this.floors = floors;
            this.floorIndex = floorIndex;
            this.tableIndex = tableIndex;
        }

        /// <summary>
        /// Creates and shows the table properties View
        /// </summary>
        /// <param name="viewGroup">Container in which the table will be shown</param>
        /// <returns></returns>
        public View CreateView(ViewGroup viewGroup)
        {
            EditText tableName;
            TextView tableSeats;
            RecyclerView mRecyclerView;

            Table table = floors[floorIndex].getTableProperties(tableIndex);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context, LinearLayoutManager.Horizontal, false);
            FutureListAdapter mAdapter = new FutureListAdapter(context, floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).reservations);

            LayoutInflater inflater = LayoutInflater.From(context);
            View tablePropertiesView = inflater.Inflate(Resource.Layout.table_properties, viewGroup, true);

            tableName = tablePropertiesView.FindViewById<EditText>(Resource.Id.idTableName);
            tableSeats = tablePropertiesView.FindViewById<TextView>(Resource.Id.idTableSeats);
            mRecyclerView = tablePropertiesView.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            tableName.Hint = table.TableName;
            tableSeats.Text = table.seats.ToString();

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            tableName.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).TableName = e.Text.ToString();
                floors.ElementAt(floorIndex).Draw();
                tableSeats.Text = e.Text.ToString();
            };
            //We identify the buttons
            Button buttonSave = tablePropertiesView.FindViewById<Button>(Resource.Id.idSaveButton);
            Button buttonDelete = tablePropertiesView.FindViewById<Button>(Resource.Id.idDeleteButton);

            return tablePropertiesView;
        }
    }
}