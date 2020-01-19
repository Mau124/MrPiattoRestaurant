using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MrPiattoRestaurant
{
    public class GestureRecognizerView : View
    {
        float n1, n2;
        //Floors name
        public string name;

        //If it is valid to move a table
        public bool moveValid = false;
        public int iterator = 0;

        TablePressedEventArgs args =  new TablePressedEventArgs();

        public float DifferentX = 0, DifferentY = 0, AbsolutTouchX = 0, AbsolutTouchY = 0;

        private static readonly int InvalidPointerId = -1;

        private readonly List<Table> tables = new List<Table>();
        private readonly ScaleGestureDetector _scaleDetector;

        private int _activePointerId = InvalidPointerId;
        private float _lastTouchX;
        private float _lastTouchY;
        private float _posX;
        private float _posY;
        private float _scaleFactor = 1.0f;

        //We define a delegate for our tablepressed event
        public delegate void TablePressedEventHandler(object source, TablePressedEventArgs args);

        //We define an event based on the tablepressed delegate
        public event TablePressedEventHandler TablePressed;

        //Raise the event
        protected virtual void OnTablePressed()
        {
            if (TablePressed != null)
            {
                TablePressed(this, args);
            }
        }

        public Table getTableProperties(int iterator)
        {
            return tables[iterator];
        }

        public GestureRecognizerView(Context context, string name) :
            base(context, null, 0)
        {

            this.name = name;
            moveValid = false;

            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), context.Resources.GetDrawable(Resource.Drawable.border), 30, 30));
            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), context.Resources.GetDrawable(Resource.Drawable.border), 30, 300));
            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), context.Resources.GetDrawable(Resource.Drawable.border), 315, 30));
            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), context.Resources.GetDrawable(Resource.Drawable.border), 600, 30));
            
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

                    tables[iterator].borderOn = false;
                    //Toast.MakeText(Application.Context, "X: " + _lastTouchX + " Y: " + _lastTouchY, ToastLength.Short).Show();

                    Toast.MakeText(Application.Context, "Factor: " + _scaleFactor + " X: " + AbsolutTouchX + " Y: " + AbsolutTouchY, ToastLength.Short).Show();

                    //We check if we pressed a table
                    bool tablePressed = false;
                    for (int i = 0; i < tables.Count() && !tablePressed; i++)
                    {
                        if (isTable(tables[i], (int)AbsolutTouchX, (int)AbsolutTouchY))
                        {
                            tablePressed = true;
                            iterator = i;
                        }
                    }

                    if (tablePressed)
                    {
                        args.floorIterator = 0;
                        args.tableIterator = iterator;
                        OnTablePressed();
                        moveValid = true;

                        tables[iterator].borderOn = true;

                        n1 = AbsolutTouchX - tables[iterator].getFirstX();
                        n2 = AbsolutTouchY - tables[iterator].getFistY();

                        Invalidate();
                    }
                    else
                    { 
                        moveValid = false;
                        Invalidate();
                    }

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

                    AbsolutTouchX *= (1 / _scaleFactor);
                    AbsolutTouchY *= (1 / _scaleFactor);

                    if (moveValid)
                    {
                        tables[iterator].SetCoordinates(AbsolutTouchX - n1, AbsolutTouchY - n2);
                        Invalidate();
                    }

                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    //We no longer need to keep track of the active pointer
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
        //Return true if the event presses a table
        public bool isTable(Table t, int x, int y)
        {
            if ((x >= t.getFirstX() && x <= t.getSecondX()) &&
                (y >= t.getFistY() && y <= t.getSecondY()))
            {
                return true;
            }
            return false;
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
            tables[iterator].DrawTable(canvas);
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