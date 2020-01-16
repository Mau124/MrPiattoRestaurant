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
        float n1, n2, n3, n4;
        //Floors name
        public string name;

        //If it is valid to move a table
        public bool moveValid = false;
        public int iterator = 0;

        public float DifferentX = 0, DifferentY = 0, AbsolutTouchX = 0, AbsolutTouchY = 0;

        private static readonly int InvalidPointerId = -1;

        private Drawable border, delete, options;
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

            border = context.Resources.GetDrawable(Resource.Drawable.border);
            delete = context.Resources.GetDrawable(Resource.Drawable.delete);
            options = context.Resources.GetDrawable(Resource.Drawable.more);

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
                    
                    DifferentX = (_posX * (-1)) * (_scaleFactor);
                    DifferentY = (_posY * (-1)) * (_scaleFactor);

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
                            iterator = i;
                        }
                    }

                    if (tablePressed)
                    {
                        border.SetBounds(tables[iterator].getFirstX() - 5, tables[iterator].getFistY() - 5, tables[iterator].getSecondX() + 5, tables[iterator].getSecondY() + 5);
                        delete.SetBounds(tables[iterator].getSecondX() + 10, tables[iterator].getFistY() - 5, tables[iterator].getSecondX() + 34, tables[iterator].getFistY() -5 + 24);
                        options.SetBounds(tables[iterator].getSecondX() + 10, tables[iterator].getFistY() + 24, tables[iterator].getSecondX() + 34, tables[iterator].getFistY() + 48);

                        Toast.MakeText(Application.Context, "Se presiono una mesa", ToastLength.Short).Show();
                        moveValid = true;

                        n1 = AbsolutTouchX - tables[iterator].getFirstX();
                        n2 = AbsolutTouchY - tables[iterator].getFistY();
                        n3 = tables[iterator].getSecondX() - AbsolutTouchX;
                        n4 = tables[iterator].getSecondY() - AbsolutTouchY;

                        Invalidate();
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "No se presiono una mesa. Sino el mapa", ToastLength.Short).Show();
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
                        tables[iterator].SetCoordinates(AbsolutTouchX - n1, AbsolutTouchY - n2, AbsolutTouchX + n3, AbsolutTouchY + n4);
                        border.SetBounds((int)AbsolutTouchX - (int)n1 - 5, (int)AbsolutTouchY - (int)n2 - 5, (int)AbsolutTouchX + (int)n3 + 5, (int)AbsolutTouchY + (int)n4 + 5);
                        delete.SetBounds((int)AbsolutTouchX + (int)n3 + 10, (int)AbsolutTouchY - (int)n2 - 5, (int)AbsolutTouchX + (int)n3 + 34, (int)AbsolutTouchY - (int)n2 -5 + 24);
                        options.SetBounds((int)AbsolutTouchX + (int)n3 + 10, (int)AbsolutTouchY - (int)n2 + 24, (int)AbsolutTouchX + (int)n3 + 34, (int)AbsolutTouchY - (int)n2 + 48);
                        Toast.MakeText(Application.Context, "X: " + tables[iterator].getFirstX() + " Y: " + tables[iterator].getFistY() + " Border x,y = " + (AbsolutTouchX - 110) + "," + (AbsolutTouchY - 76), ToastLength.Short).Show();
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
                t.getIcon().Draw(canvas);
            }
            tables[iterator].getIcon().Draw(canvas);
            if (moveValid)
            {
                border.Draw(canvas);
                delete.Draw(canvas);
                options.Draw(canvas);
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