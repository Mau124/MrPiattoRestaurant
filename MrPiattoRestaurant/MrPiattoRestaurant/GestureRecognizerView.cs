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
        //Floors name
        public string name;

        //If it is valid to move a table
        public bool moveValid = false;

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
        public GestureRecognizerView(Context context, string name) :
            base(context, null, 0)
        {
            this.name = name;
            moveValid = false;

            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), 30, 30, 250, 183));
            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), 30, 300, 250, 453));
            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), 315, 30, 535, 183));
            tables.Add(new Table(context.Resources.GetDrawable(Resource.Drawable.Table1), 600, 30, 820, 183));
            
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

                    DifferentX = _posX * (-1);
                    DifferentY = _posY * (-1);


                    AbsolutTouchX = DifferentX + _lastTouchX;
                    AbsolutTouchY = DifferentY + _lastTouchY;

                    AbsolutTouchX *= (1 / _scaleFactor);
                    AbsolutTouchY *= (1 / _scaleFactor);
                    //Toast.MakeText(Application.Context, "X: " + _lastTouchX + " Y: " + _lastTouchY, ToastLength.Short).Show();

                    Toast.MakeText(Application.Context, "Factor: " + _scaleFactor + " X: " + AbsolutTouchX + " Y: " + AbsolutTouchY, ToastLength.Short).Show();

                    //We check if we pressed a table
                    bool tablePressed = false;
                    for (int i = 0; i < tables.Count() && !tablePressed; i++)
                    {
                        if (isTable(tables[i], (int)AbsolutTouchX, (int)AbsolutTouchY))
                        {
                            tablePressed = true;
                        }
                    }

                    if (tablePressed)
                    {
                        Toast.MakeText(Application.Context, "Se presiono una mesa", ToastLength.Short).Show();
                        moveValid = true;
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "No se presiono una mesa. Sino el mapa", ToastLength.Short).Show();
                        moveValid = false;
                    }

                    _activePointerId = ev.GetPointerId(0);
                    Invalidate();
                    break;

                case MotionEventActions.Move:
                    pointerIndex = ev.FindPointerIndex(_activePointerId);
                    float x = ev.GetX(pointerIndex);
                    float y = ev.GetY(pointerIndex);
                    if (!_scaleDetector.IsInProgress)
                    {
                        //Only move the ScaleGestureDetector isn't already processing a gesture
                        float deltaX = x - _lastTouchX;
                        float deltaY = y - _lastTouchY;
                        _posX += deltaX;
                        _posY += deltaY;
                        Invalidate();
                    }

                    _lastTouchX = x;
                    _lastTouchY = y;

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
            canvas.Translate(_posX, _posY);
            canvas.Scale(_scaleFactor, _scaleFactor);
            
            foreach (Table t in tables)
            {
                t.getIcon().Draw(canvas);
            }

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