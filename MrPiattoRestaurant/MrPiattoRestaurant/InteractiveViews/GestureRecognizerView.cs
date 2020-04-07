using System.Collections.Generic;
using System.Linq;
using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.InteractiveViews;

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

        // Indexes for floor and table
        public int tableIndex = new int();
        public int floorIndex = new int();

        public float DifferentX = 0, DifferentY = 0, AbsolutTouchX = 0, AbsolutTouchY = 0;
        public float centerX = screenSizeX / 2, centerY = screenSizeY / 2;

        private static readonly int InvalidPointerId = -1;

        public List<Table> tables = new List<Table>();
        public List<Table> ocupiedTables = new List<Table>();

        private readonly ScaleGestureDetector _scaleDetector;

        private int _activePointerId = InvalidPointerId;
        private float _lastTouchX;
        private float _lastTouchY;
        private float _posX;
        private float _posY;
        private float _scaleFactor = 1.0f;

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

        public Table getTableProperties(int iterator)
        {
            return tables[iterator];
        }

        public void DeleteTable(int iterator)
        {
            tables.RemoveAt(iterator);
            tableIndex = 0;
        }

        public GestureRecognizerView(Context context, string name, int floorIndex, TimeLineView timeLineView) :
            base(context, null, 0)
        {
            this.context = context;
            this.name = name;
            this.floorIndex = floorIndex;
            this.timeLineView = timeLineView;
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

                    if (tables.Count() > 0)
                        tables[tableIndex].borderOn = false;

                    if (isOnTable() != -1)
                    {
                        OnTablePressed(floorIndex, tableIndex);
                        moveValid = true;
                        isTablePressed = true;

                        tables[tableIndex].borderOn = true;

                        n1 = AbsolutTouchX - tables[tableIndex].firstX;
                        n2 = AbsolutTouchY - tables[tableIndex].firstY;
                    }
                    else
                    { 
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

                    if (moveValid)
                    {
                        tables[tableIndex].SetCoordinates(AbsolutTouchX - n1, AbsolutTouchY - n2);
                        Invalidate();
                    }

                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    //We no longer need to keep track of the active pointer
                    bool tablePressed = false;
                    int auxTableIndex = new int();
                    for (int i = 0; i < tables.Count() && !tablePressed; i++)
                    {
                        if ((isTable(tables[i], tables[tableIndex].firstX, tables[tableIndex].firstY)
                            || isTable(tables[i], tables[tableIndex].firstX, tables[tableIndex].firstY + tables[tableIndex].getHeight())
                            || isTable(tables[i], tables[tableIndex].firstX + tables[tableIndex].getWidth(), tables[tableIndex].firstY)
                            || isTable(tables[i], tables[tableIndex].firstX + tables[tableIndex].getWidth(), tables[tableIndex].firstY + tables[tableIndex].getHeight()))
                            && i != tableIndex)
                        {
                            tablePressed = true;
                            auxTableIndex = i;
                        }
                    }
                    if (tablePressed)
                    {
                        JoinTables(tableIndex, auxTableIndex);
                        Toast.MakeText(Application.Context, "Esta sobre una mesa: " + auxTableIndex, ToastLength.Long).Show();
                    }
                    else
                    {
                        Point point; 

                        ////////////////////////////////////
                        //if (tables.Count() > 0 && isTable(tables[tableIndex], tables[tableIndex].firstX, tables[tableIndex].firstY))
                        //{
                        //    int hours = timeLineView.GetHour();
                        //    int minutes = timeLineView.GetMinutes();
                        //    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, 0);

                        //    point = new Point(tables[tableIndex].firstX, tables[tableIndex].firstY);
                        //    tables.ElementAt(tableIndex).AddDistribution(point, date);

                        //    Toast.MakeText(Application.Context, "Size: " + tables[tableIndex].TableDistributions.Count(), ToastLength.Long).Show();

                        //    Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////" + minutes);
                        //    Console.WriteLine("Hora: " + hours + "Minutes " + minutes);

                        //    for (int i = 0; i < tables[tableIndex].TableDistributions.Count(); ++i)
                        //    {
                        //        Console.WriteLine("Puntos: " + tables[tableIndex].TableDistributions.ElementAt(i).Key + " Fecha: " + tables[tableIndex].TableDistributions.ElementAt(i).Value);
                        //    }
                        //    Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////" + minutes);
                        //}
                    }
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
                if (isTable(tables[i], (int) AbsolutTouchX, (int) AbsolutTouchY))
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

        public void AddTable(string type, int seats)
        {
            if (tables.Count() > 0)
                tables.ElementAt(tableIndex).borderOn = false;
            tables.Add(new Table(context, type, seats, (int)centerX, (int)centerY, true));
            OnTablePressed(floorIndex, tables.Count() - 1);
            Invalidate();
        }

        public void JoinTables(int indexTable1, int indexTable2)
        {
            int auxSeats = tables.ElementAt(indexTable1).seats + tables.ElementAt(indexTable2).seats;
            string auxType = tables.ElementAt(indexTable1).type;
            int x = tables.ElementAt(indexTable1).firstX;
            int y = tables.ElementAt(indexTable1).firstY;

            ShowJoinTablesDialog(indexTable1, indexTable2);

            tables.ElementAt(indexTable1).type = auxType;
            tables.ElementAt(indexTable1).seats = auxSeats;
            tables.ElementAt(indexTable1).setDrawable(auxType, auxSeats);
            tables.ElementAt(indexTable1).SetCoordinates(x, y);

            tables.RemoveAt(indexTable2);
            tableIndex = (indexTable1 > indexTable2) ? indexTable1 - 1 : indexTable1;
            Invalidate();
        }

        public void setActualClientOnTable(Client actualClient, int tableIterator)
        {
            tables.ElementAt(tableIterator).setActualClient(actualClient);
            ocupiedTables.Add(tables.ElementAt(tableIterator));
        }

        public void updateTableDistributions(int hours, int minutes)
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, 0);
            foreach (Table t in tables)
            {
                if (t.TableDistributions.Count() > 0)
                {
                    t.ChangeTableDistribution(date);
                    Invalidate();
                }
            }
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
            if (tables.Count() > 0) 
                tables[tableIndex].DrawTable(canvas);
            canvas.Restore();
        }

        public void ShowJoinTablesDialog(int indexTable1, int indexTable2)
        {
            LayoutInflater inflater = LayoutInflater.From(context);
            View content = inflater.Inflate(Resource.Layout.dialog_join_tables, null);

            TextView nameTable1, nameTable2, seatsTable1, seatsTable2;
            ImageView imageTable1, imageTable2;

            nameTable1 = content.FindViewById<TextView>(Resource.Id.idNameTable1);
            nameTable2 = content.FindViewById<TextView>(Resource.Id.idNameTable2);
            seatsTable1 = content.FindViewById<TextView>(Resource.Id.idSeatsTable1);
            seatsTable2 = content.FindViewById<TextView>(Resource.Id.idSeatsTable2);
            imageTable1 = content.FindViewById<ImageView>(Resource.Id.idImageTable1);
            imageTable2 = content.FindViewById<ImageView>(Resource.Id.idImageTable2);

            nameTable1.Text = tables.ElementAt(indexTable1).TableName;
            seatsTable1.Text = tables.ElementAt(indexTable1).seats.ToString();

            nameTable2.Text = tables.ElementAt(indexTable2).TableName;
            seatsTable2.Text = tables.ElementAt(indexTable2).seats.ToString();

            imageTable1.SetImageResource(context.Resources.GetIdentifier(tables.ElementAt(indexTable1).tableDrawable, "drawable", context.PackageName));
            imageTable2.SetImageResource(context.Resources.GetIdentifier(tables.ElementAt(indexTable2).tableDrawable, "drawable", context.PackageName));

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();
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