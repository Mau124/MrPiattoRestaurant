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
using MrPiattoRestaurant.Resources.utilities;
using MrPiattoRestaurant.ModelsDB;

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
        private APIUpdate APIupdate = new APIUpdate();


        List<GestureRecognizerView> floors;

        //We define a delegate for our tablepressed event
        public delegate void ClosePressedEventHandler();

        //We define an event based on the tablepressed delegate
        public event ClosePressedEventHandler ClosePressed;

        //Raise the event
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
            ImageView dismiss;
            RecyclerView mRecyclerView;
            SeekBar mSeekBar;

            Table table = floors[floorIndex].getTableProperties(tableIndex);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context, LinearLayoutManager.Horizontal, false);
            FutureListAdapter mAdapter = new FutureListAdapter(context, floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).reservations);

            LayoutInflater inflater = LayoutInflater.From(context);
            View tablePropertiesView = inflater.Inflate(Resource.Layout.table_properties, viewGroup, true);

            tableName = tablePropertiesView.FindViewById<EditText>(Resource.Id.idTableName);
            tableSeats = tablePropertiesView.FindViewById<TextView>(Resource.Id.idTableSeats);
            mRecyclerView = tablePropertiesView.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);
            mSeekBar = tablePropertiesView.FindViewById<SeekBar>(Resource.Id.idSeekBar);
            dismiss = tablePropertiesView.FindViewById<ImageView>(Resource.Id.idDismiss);

            tableName.Hint = table.TableName;
            tableSeats.Hint = table.seats.ToString();

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            dismiss.Click += onDismiss;

            mSeekBar.Min = 1;
            mSeekBar.Max = 12;
            mSeekBar.Progress = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).seats;

            tableName.TextChanged += async (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                RestaurantTables restaurant = new RestaurantTables();
                floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).TableName = e.Text.ToString();
                floors.ElementAt(floorIndex).Draw();

                restaurant.Idtables = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).Id;
                restaurant.CoordenateX = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).firstX;
                restaurant.CoordenateY = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).firstY;
                restaurant.Seats = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).seats;
                restaurant.tableName = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).TableName;

                var response = await APIupdate.UpdateTable(restaurant);
                Toast.MakeText(context, response, ToastLength.Long).Show();
            };

            tableSeats.TextChanged += async (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                try
                {
                    int seats;
                    string type = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).type;
                    if (e.Text.ToString().Equals("") || Int32.Parse(e.Text.ToString()) > 12)
                    {
                        seats =  Int32.Parse(tableSeats.Hint);
                        mSeekBar.Progress = Int32.Parse(tableSeats.Hint);
                    } else
                    {
                        seats = Int32.Parse(e.Text.ToString());
                        mSeekBar.Progress = Int32.Parse(e.Text.ToString());
                    }

                    try
                    {
                        floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).setDrawable(type, seats);
                        floors.ElementAt(floorIndex).Invalidate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    RestaurantTables restaurant = new RestaurantTables();

                    restaurant.Idtables = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).Id;
                    restaurant.CoordenateX = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).firstX;
                    restaurant.CoordenateY = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).firstY;
                    restaurant.Seats = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).seats;
                    restaurant.tableName = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).TableName;

                    var response = await APIupdate.UpdateTable(restaurant);
                    Toast.MakeText(context, response, ToastLength.Long).Show();
                } catch
                {
                    Toast.MakeText(context, "Valores Incorrectos", ToastLength.Long).Show();
                }
            };

            mSeekBar.ProgressChanged += async (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    string type = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).type;
                    tableSeats.Text = e.Progress.ToString();

                    RestaurantTables restaurant = new RestaurantTables();

                    restaurant.Idtables = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).Id;
                    restaurant.CoordenateX = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).firstX;
                    restaurant.CoordenateY = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).firstY;
                    restaurant.Seats = e.Progress;
                    restaurant.tableName = floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).TableName;

                    var response = await APIupdate.UpdateTable(restaurant);
                    Toast.MakeText(context, response, ToastLength.Long).Show();

                    try
                    {
                        floors.ElementAt(floorIndex).tables.ElementAt(tableIndex).setDrawable(type, e.Progress);
                        floors.ElementAt(floorIndex).Invalidate();
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            };
            //We identify the buttons
            Button buttonSave = tablePropertiesView.FindViewById<Button>(Resource.Id.idSaveButton);
            Button buttonDelete = tablePropertiesView.FindViewById<Button>(Resource.Id.idDeleteButton);

            return tablePropertiesView;
        }

        private void onDismiss(object sender, EventArgs e)
        {
            OnClosePressed();
        }
    }
}