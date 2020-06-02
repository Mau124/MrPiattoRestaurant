using System.Collections.Generic;
using System.Linq;
using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Graphics.Drawables;

using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.InteractiveViews;
using MrPiattoRestaurant.Resources.utilities;
using MrPiattoRestaurant.ModelsDB;

namespace MrPiattoRestaurant
{
    public class GestureRecognizerView : View
    {
        public const float screenSizeX = 978;
        public const float screenSizeY = 526;
        public Context context;
        public TimeLineView timeLineView;

        float n1, n2;
        //Floors name
        public string name;

        //If it is valid to move a table
        public bool moveValid = false;
        public bool isTablePressed = false;

        // Menciona si se estan seleccionando mesas
        // En caso de que se esten seleccionando mesas, no se podran mover ni modificar las actuales
        public bool isSelecting { get; set; }

        // Indexes for floor and table
        public int tableIndex = new int();
        public int floorIndex = new int();

        public float DifferentX = 0, DifferentY = 0, AbsolutTouchX = 0, AbsolutTouchY = 0;
        public float centerX = screenSizeX / 2, centerY = screenSizeY / 2;

        private static readonly int InvalidPointerId = -1;

        public List<Table> tables = new List<Table>();
        public List<Table> ocupiedTables = new List<Table>();
        public List<Table> auxTables = new List<Table>();

        private readonly ScaleGestureDetector _scaleDetector;

        private int _activePointerId = InvalidPointerId;
        private float _lastTouchX;
        private float _lastTouchY;
        private float _posX;
        private float _posY;
        private float _scaleFactor = 1.0f;

        private Restaurant restaurant;

        private APIUpdate APIupdate = new APIUpdate();
        private APICaller API = new APICaller();

        //We define a delegate for our tablepressed event
        public delegate void TablePressedEventHandler(int floorIndex, int tableIndex);

        //We define an event based on the tablepressed delegate
        public event TablePressedEventHandler TablePressed;

        //Raise the event
        protected virtual void OnTablePressed(int floorIndex, int tableIndex)
        {
            if (TablePressed != null)
            {
                TablePressed(floorIndex, tableIndex);
            }
        }

        // Evento para definir cuando una mesa se selecciono
        public delegate void TableSelectedEventHandler(Table table);
        public event TableSelectedEventHandler TableSelected;
        protected virtual void OnTableSelected(Table table)
        {
            if (TableSelected != null)
            {
                TableSelected(table);
            }
        }

        // Evento para definir que una mesa se deselecciono
        public delegate void TableDisSelectEventHandler(Table table);
        public event TableDisSelectEventHandler TableDisSelect;
        protected virtual void OnTableDisSelectPressed(Table table)
        {
            if (TableDisSelect != null)
            {
                TableDisSelect(table);
            }
        }

        public delegate void ClosePressedEventHandler();
        public event ClosePressedEventHandler ClosePressed;
        protected virtual void OnClosePressed()
        {
            if (ClosePressed != null)
            {
                ClosePressed();
            }
        }

        public void InitializeUnion(Table t)
        {
            isSelecting = true;
            auxTables.Clear();
            auxTables.Add(t);
            t.setSelected(1);
            Invalidate();
        }

        public void InitializeUnion()
        {
            isSelecting = true;
            auxTables.Clear();
            Invalidate();
        }

        public void Union(List<Table> tablesToJoin, Client client, int seats)
        {
            Table auxTable = new Table(context, tablesToJoin, client, seats);

            // Iteramos sobre la lista de mesas que se van a unir
            foreach (Table t in tablesToJoin)
            {
                // Removemos de la lista de mesas las que se uninar
                t.Selected = 2;
                tables.Remove(t);
            }


            auxTable.TableUnJoined += unJoined;
            tables.Add(auxTable);
            ocupiedTables.Add(auxTable);
            setDisSelect();
            isSelecting = false;

            Invalidate();
        }

        private void setDisSelect()
        {
            foreach (Table t in tables)
            {
                t.InitializeImages();
            }
        }

        public void backGrid()
        {
            foreach (Table t in tables)
            {
                t.Selected = 2;
            }
            isSelecting = false;
            setDisSelect();
            Invalidate();
        }

        private void unJoined(Table table)
        {
            ocupiedTables.Remove(table);

            foreach (Table t in table.tables)
            {
                tables.Add(t);
            }

            setDisSelect();
            tables.Remove(table);
            Invalidate();
        }

        public Table getTableProperties(int iterator)
        {
            return tables[iterator];
        }

        public void DeleteTable(int iterator)
        {
            tables.RemoveAt(iterator);
            tableIndex = 0;
        }

        public GestureRecognizerView(Context context, string name, int floorIndex, TimeLineView timeLineView, Restaurant restaurant) :
            base(context, null, 0)
        {
            this.context = context;
            this.name = name;
            this.floorIndex = floorIndex;
            this.timeLineView = timeLineView;
            this.restaurant = restaurant;
            moveValid = false;

            _scaleDetector = new ScaleGestureDetector(context, new MyScaleListener(this));
        }
        public override bool OnTouchEvent(MotionEvent ev)
        {
            _scaleDetector.OnTouchEvent(ev);

            MotionEventActions action = ev.Action & MotionEventActions.Mask;
            int pointerIndex;

            switch (action)
            {
                case MotionEventActions.Down:
                    _lastTouchX = ev.GetX();
                    _lastTouchY = ev.GetY();

                    DifferentX = (_posX * (-1)) * (_scaleFactor);
                    DifferentY = (_posY * (-1)) * (_scaleFactor);

                    AbsolutTouchX = DifferentX + _lastTouchX;
                    AbsolutTouchY = DifferentY + _lastTouchY;

                    AbsolutTouchX *= (1 / _scaleFactor);
                    AbsolutTouchY *= (1 / _scaleFactor);

                    foreach (Table t in tables) {
                        t.borderOn = false;
                    }

                    if (isOnTable() != -1)
                    {
                        if (isSelecting)
                        {
                            switch (tables[tableIndex].Selected)
                            {
                                case 0:
                                    Toast.MakeText(Application.Context, "No se puede seleccionar esta mesa", ToastLength.Short).Show();
                                    break;
                                case 1:
                                    auxTables.Remove(tables[tableIndex]);
                                    TableDisSelect(tables[tableIndex]);
                                    tables[tableIndex].setSelected(2);
                                    break;
                                case 2:
                                    auxTables.Add(tables[tableIndex]);
                                    TableSelected(tables[tableIndex]);
                                    tables[tableIndex].setSelected(1);
                                    break;
                            }
                            Invalidate();
                        } else
                        {
                            OnTablePressed(floorIndex, tableIndex);
                            moveValid = true;
                            isTablePressed = true;

                            tables[tableIndex].borderOn = true;

                            n1 = AbsolutTouchX - tables[tableIndex].firstX;
                            n2 = AbsolutTouchY - tables[tableIndex].firstY;
                        }
                    }
                    else
                    {
                        OnClosePressed();
                        moveValid = false;
                    }

                    Invalidate();
                    _activePointerId = ev.GetPointerId(0);
                    break;

                case MotionEventActions.Move:
                    pointerIndex = ev.FindPointerIndex(_activePointerId);
                    float x = ev.GetX(pointerIndex);
                    float y = ev.GetY(pointerIndex);
                    if (!_scaleDetector.IsInProgress)
                    {
                        float deltaX = x - _lastTouchX;
                        float deltaY = y - _lastTouchY;
                        //Only move the ScaleGestureDetector isn't already processing a gesture
                        if (!moveValid)
                        {
                            _posX += deltaX;
                            _posY += deltaY;

                        }
                        Invalidate();
                    }

                    _lastTouchX = x;
                    _lastTouchY = y;

                    DifferentX = (_posX * (-1) * (_scaleFactor));
                    DifferentY = (_posY * (-1) * (_scaleFactor));

                    AbsolutTouchX = DifferentX + _lastTouchX;
                    AbsolutTouchY = DifferentY + _lastTouchY;

                    centerX = (AbsolutTouchX - _lastTouchX) + (screenSizeX / 2);
                    centerY = (AbsolutTouchY - _lastTouchY) + (screenSizeY / 2);

                    AbsolutTouchX *= (1 / _scaleFactor);
                    AbsolutTouchY *= (1 / _scaleFactor);

                    centerX *= (1 / _scaleFactor);
                    centerY *= (1 / _scaleFactor);

                    if (moveValid && !isSelecting)
                    {
                        tables[tableIndex].SetCoordinates(AbsolutTouchX - n1, AbsolutTouchY - n2);
                        // Actualizamos en la base de datos con esas nuevas coordenadas
                        var response = APIupdate.UpdateTable(new RestaurantTables(tables[tableIndex].Id, tables[tableIndex].firstX, tables[tableIndex].firstY, tables[tableIndex].seats, tables[tableIndex].TableName)).Result;
                        Toast.MakeText(context, response, ToastLength.Long).Show();

                        Invalidate();
                    }

                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    //We no longer need to keep track of the active pointer
                    isTablePressed = false;
                    _activePointerId = InvalidPointerId;
                    break;

                case MotionEventActions.PointerUp:
                    //check to make sure that the pointer that went up is for the gesture we're tracking
                    pointerIndex = (int)(ev.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
                    int pointerId = ev.GetPointerId(pointerIndex);
                    if (pointerId == _activePointerId)
                    {
                        //This was out active pointer going up. Choose a new
                        //action pointer and adjust accordingly
                        int newPointerIndex = pointerIndex == 0 ? 1 : 0;
                        _lastTouchX = ev.GetX(newPointerIndex);
                        _lastTouchY = ev.GetY(newPointerIndex);
                        _activePointerId = ev.GetPointerId(newPointerIndex);
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Function that returns all the reservatinos for that day
        /// </summary>
        /// <param name="date">Day of reservations</param>
        /// <returns>List of clients that have a reservation on that date</returns>
        public List<Client> GetReservations(DateTime date)
        {
            List<Client> reservations = new List<Client>();

            foreach (Table table in tables)
            {
                foreach (Client client in table.reservations)
                {
                    if ((client.reservationDate.Month == date.Month)
                        && (client.reservationDate.Day == date.Day))
                    {
                        reservations.Add(client);
                    }
                }
            }

            return reservations;
        }
        //Return true if the event presses a table
        public bool isTable(Table t, int x, int y)
        {
            if ((x >= t.firstX && x <= t.secondX) &&
                (y >= t.firstY && y <= t.secondY))
            {
                return true;
            }
            return false;
        }

        public int isOnTable()
        {
            bool tablePressed = false;
            for (int i = 0; i < tables.Count() && !tablePressed; i++)
            {
                if (isTable(tables[i], (int)AbsolutTouchX, (int)AbsolutTouchY))
                {
                    tablePressed = true;
                    tableIndex = i;
                }
            }
            return (tablePressed) ? tableIndex : -1;
        }

        public int IsOnTable(float x, float y)
        {
            int table = 0;

            DifferentX = (_posX * (-1)) * (_scaleFactor);
            DifferentY = (_posY * (-1)) * (_scaleFactor);

            AbsolutTouchX = DifferentX + x;
            AbsolutTouchY = DifferentY + y;

            AbsolutTouchX *= (1 / _scaleFactor);
            AbsolutTouchY *= (1 / _scaleFactor);

            bool tablePressed = false;
            for (int i = 0; i < tables.Count() && !tablePressed; i++)
            {
                if (isTable(tables[i], (int)AbsolutTouchX, (int)AbsolutTouchY))
                {
                    tablePressed = true;
                    table = i;
                }
            }
            return (tablePressed) ? table : -1;
        }

        public void Draw()
        {
            Invalidate();
        }

        public void AddOneTable(Table table)
        {
            foreach (Table auxTable in tables)
            {
                auxTable.borderOn = false;
            }

            Table t = new Table(context, table.Id, table.TableName, table.type, table.seats, table.firstX, table.firstY, true);
            t.Draw += DrawTable;
            tables.Add(t);

            OnTablePressed(floorIndex, tables.Count() - 1);
            Invalidate();
        }

        public void AddTable(Table table)
        {
            Table t = new Table(context, table.Id, table.TableName, table.type, table.seats, table.firstX, table.firstY, table.borderOn);
            t.Draw += DrawTable;
            tables.Add(t);

            Invalidate();
        }

        public void updateStates(DateTime date)
        {
            List<AuxiliarReservation> auxRes = API.GetAllAuxReservations(restaurant.Idrestaurant);
            foreach (Table t in tables)
            {
                bool isReservation = false;
                foreach (Client c in t.reservations)
                {
                    DateTime limit = new DateTime(c.reservationDate.Year, c.reservationDate.Month, c.reservationDate.Day, c.reservationDate.Hour, c.reservationDate.Minute, c.reservationDate.Second);
                    limit = limit.AddHours(2);

                    if ((date >= c.reservationDate && date <= limit) && !isReservation)
                    {
                        isReservation = true;
                        t.setImageOcupied(true);
                    }
                }
                if (!isReservation)
                {
                    t.setImageOcupied(false);
                }
            }

            // Revisamos para las mesas unidas
            if (auxRes != null)
            {
                foreach (AuxiliarReservation aux in auxRes)
                {
                    string[] div = aux.IdauxiliarTableNavigation.StringIdtables.Split(' ');

                    foreach (string s in div)
                    {
                        try
                        {
                            int idTable = Int32.Parse(s);
                            int tableIndex = tables.FindIndex(t => t.Id == idTable);

                            DateTime limit = new DateTime(aux.Date.Year, aux.Date.Month, aux.Date.Day, aux.Date.Hour, aux.Date.Minute, aux.Date.Second);
                            limit = limit.AddHours(2);

                            if (date >= aux.Date && date <= limit)
                            {
                                tables[tableIndex].setImageOcupied(true);
                            } 
                        }
                        catch (FormatException e)
                        { }
                    }
                }
            }
        }

        public void returnActualTime()
        {
            foreach (Table t in tables)
            {
                t.InitializeTable();
            }
        }

        public void DrawTable() 
        { 
            Invalidate();
        }

        public void setActualClientOnTable(Client actualClient, int tableIterator)
        {
            tables.ElementAt(tableIterator).setActualClient(actualClient);
            ocupiedTables.Add(tables.ElementAt(tableIterator));
        }
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.Save();
            canvas.Scale(_scaleFactor, _scaleFactor);
            canvas.Translate(_posX, _posY);
            foreach (Table t in tables)
            {
                t.DrawTable(canvas);
            }
            if (tableIndex < tables.Count) 
                tables[tableIndex].DrawTable(canvas);
            canvas.Restore();
        }

        private class MyScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            private readonly GestureRecognizerView _view;

            public MyScaleListener(GestureRecognizerView view)
            {
                _view = view;
            }

            public override bool OnScale(ScaleGestureDetector detector)
            {
                _view._scaleFactor *= detector.ScaleFactor;

                if (_view._scaleFactor > 5.0f)
                {
                    _view._scaleFactor = 5.0f;
                }
                if (_view._scaleFactor < 0.1f)
                {
                    _view._scaleFactor = 0.1f;
                }

                _view.Invalidate();
                return true;
            }
        }

    }
}