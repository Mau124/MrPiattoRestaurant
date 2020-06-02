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
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

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
        private APICaller API = new APICaller();
        private APIUpdate APIupdate = new APIUpdate();
        private Restaurant restaurant = new Restaurant();

        List<GestureRecognizerView> floors;
        Client client = new Client();

        private bool isRes;

        public delegate void ClosePressedEventHandler();
        public event ClosePressedEventHandler ClosePressed;
        protected virtual void OnClosePressed()
        {
            if (ClosePressed != null)
            {
                ClosePressed();
            }
        }

        public delegate void UpdateEventHandler();
        public event UpdateEventHandler Update;
        protected virtual void OnUpdate()
        {
            if (Update != null)
            {
                Update();
            }
        }

        public delegate void DismissEventHandler();
        public event DismissEventHandler Dismiss;
        protected virtual void OnDismiss()
        {
            if (Dismiss != null)
            {
                Dismiss();
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
            isRes = false;
            this.context = context;
            this.client = client;
            this.floors = floors;
            this.floorIndex = floorIndex;
            this.tableIndex = tableIndex;

            floors[floorIndex].InitializeUnion(floors[floorIndex].tables[tableIndex]);
        }

        /// <summary>
        /// Class that creates a tablepropertiesView
        /// </summary>
        /// <param name="context">Context of activity</param>
        /// <param name="floors">List of floors</param>
        /// <param name="floorIndex">Actual floor Index</param>
        public TableUnion(Context context, Restaurant restaurant, Client client, List<GestureRecognizerView> floors, int floorIndex)
        {
            isRes = true;
            this.context = context;
            this.restaurant = restaurant;
            this.client = client;
            this.floors = floors;
            this.floorIndex = floorIndex;

            floors[floorIndex].InitializeUnion();
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
            ImageView dismiss = tableUnion.FindViewById<ImageView>(Resource.Id.idDismiss);


            mLayoutManager = new LinearLayoutManager(context);
            mAdapter = new UnionAdapter(context, floors[floorIndex].auxTables);

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            if (!isRes)
            {
                foreach (Table t in floors[floorIndex].tables)
                {
                    if (t.isOcupied || t.nextReservations())
                    {
                        t.setSelected(0);
                    }
                }
            } else
            {
                DateTime auxDate1 = client.reservationDate.AddHours(2);
                DateTime auxDate2 = client.reservationDate.AddHours(-2);

                List<AuxiliarReservation>? auxReservations = API.GetAllAuxReservations(restaurant.Idrestaurant).Where(c => c.Date >= auxDate2 && c.Date <= auxDate1).ToList();

                // Revisamos si existe alguna reservacion existente
                foreach (Table t in floors[floorIndex].tables)
                {
                    if (t.nextReservations(client.reservationDate))
                    {
                        t.setSelected(0);
                    }
                }

                // Revisamos si existe alguna reservacion para las mesas unidas
                if (auxReservations != null)
                {
                    foreach(AuxiliarReservation aux in auxReservations)
                    {
                        string[] div = aux.IdauxiliarTableNavigation.StringIdtables.Split(' ');

                        foreach(string s in div)
                        {
                            try 
                            {
                                int idTable = Int32.Parse(s);
                                int tableIndex = floors[floorIndex].tables.FindIndex(t => t.Id == idTable);

                                floors[floorIndex].tables[tableIndex].setSelected(0);
                            } catch (FormatException e)
                            {}
                        }
                    }
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
                if (seats >= client.seats && seats < 13)
                {
                    if (!isRes)
                    {
                        floors[floorIndex].Union(floors[floorIndex].auxTables, client, seats);
                        OnClosePressed();
                    } else
                    {
                        if (floors[floorIndex].auxTables.Count == 1)
                        {
                            // En este caso guardamos la reservacion en la tabla de reservacion manual
                            ManualReservations reservation = new ManualReservations();

                            reservation.IDTable = floors[floorIndex].auxTables.First().Id;
                            reservation.Date = client.reservationDate;
                            reservation.AmountOfPeople = client.seats;
                            reservation.Checked = false;
                            reservation.Name = client.name;
                            reservation.LastName = client.lastName;

                            var response = APIupdate.AddManReservation(reservation).Result;
                            Toast.MakeText(context, response, ToastLength.Long).Show();
                            OnClosePressed();
                        } else 
                        {
                            // En este caso guardariamos la reservacion en la tabla auxiliar, realizando en primer lugar la creacion de la mesa
                            AuxiliarTables table = new AuxiliarTables();

                            table.Idrestaurant = restaurant.Idrestaurant;
                            table.FloorName = floors[floorIndex].name;
                            table.CoordenateX = floors[floorIndex].auxTables.First().firstX;
                            table.CoordenateY = floors[floorIndex].auxTables.First().firstY;
                            table.AvarageUse = 0;

                            foreach (Table t in floors[floorIndex].auxTables)
                            {
                                table.StringIdtables += " " + t.Id;
                            }

                            var response = APIupdate.AddAuxiliarTable(table).Result;
                            Toast.MakeText(context, response, ToastLength.Long).Show();

                            AuxiliarReservation reservation = new AuxiliarReservation();

                            reservation.IdauxiliarTable = API.GetAuxTables(restaurant.Idrestaurant).Last().IdauxiliarTable;
                            reservation.Date = client.reservationDate;
                            reservation.AmountOfPeople = client.seats;
                            reservation.ReservationStatus = 1;
                            reservation.Phone = client.Phone;
                            reservation.Checked = false;
                            reservation.Name = client.name;
                            reservation.LastName = client.lastName;

                            response = APIupdate.AddAuxReservation(reservation).Result;
                            Toast.MakeText(context, response, ToastLength.Long).Show();
                            OnClosePressed();
                        }

                        OnUpdate();
                        floors[floorIndex].backGrid();
                    }
                } else
                {
                    Toast.MakeText(Application.Context, "El numero de sillas en las mesas seleccionadas debe ser suficiente para el cliente y menor a 13", ToastLength.Short).Show();
                }
            };

            dismiss.Click += delegate
            {
                floors[floorIndex].auxTables.Clear();
                OnDismiss();
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