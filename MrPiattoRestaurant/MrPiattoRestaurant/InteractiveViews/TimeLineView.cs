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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MrPiattoRestaurant.InteractiveViews
{
    public class TimeLineView : View
    {
        private const int SPACE_BETWEEN_LINES = 20;
        private const int START_HOUR = 7;

        private static readonly int InvalidPointerId = -1;
        private readonly Drawable _icon;
        private int _activePointerId = InvalidPointerId;
        private float _lastTouchX;
        private int _posX;
        private int center;
        private int lines;
        private int totalMinutes;

        private int hours, minutes;

        private Context context;

        //We define a delegate for our tablepressed event
        public delegate void TimeLinePressedEventHandler(int hours, int minutes);

        //We define an event based on the tablepressed delegate
        public event TimeLinePressedEventHandler TimeLinePressed;

        //Raise the event
        protected virtual void OnTimeLinePressed(int hours, int minutes)
        {
            if (TimeLinePressed != null)
            {
                TimeLinePressed(hours, minutes);
            }
        }

        public TimeLineView (Context context) : base(context, null, 0)
        {
            this.context = context;
            center = (Resources.DisplayMetrics.WidthPixels / 2) - 1;
            lines = 0;
            totalMinutes = 0;

            hours = 0;
            minutes = 0;

            _icon = context.Resources.GetDrawable(Resource.Drawable.line12);
            _icon.SetBounds(center, 0, _icon.IntrinsicWidth + center, _icon.IntrinsicHeight);
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            MotionEventActions action = ev.Action & MotionEventActions.Mask;
            int pointerIndex;

            switch (action)
            {
                case MotionEventActions.Down:
                    _lastTouchX = ev.GetX();
                    _activePointerId = ev.GetPointerId(0);
                    break;

                case MotionEventActions.Move:
                    pointerIndex = ev.FindPointerIndex(_activePointerId);
                    float x = ev.GetX(pointerIndex);

                    // Only move the ScaleGestureDetector isn't already processing a gesture.
                    float deltaX = x - _lastTouchX;
                    _posX += (int)deltaX;

                    lines = _posX;
                    totalMinutes = ((-1) * lines);

                    int auxHours, auxMinutes;

                    auxHours = totalMinutes / 60;
                    auxMinutes = totalMinutes % 60;

                    hours = (7 + auxHours) % 24;
                    minutes = auxMinutes;

                    OnTimeLinePressed(hours, minutes);

                    Toast.MakeText(context, "TotalMinutes: " + lines, ToastLength.Long).Show();

                    if (_posX <= 0 && _posX >= (_icon.IntrinsicWidth * (-1)))
                    {
                        Invalidate();
                    } 
                    else
                    {
                        _posX -= (int)deltaX;
                    }

                    _lastTouchX = x;
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    // This events occur when something cancels the gesture (for example the
                    // activity going in the background) or when the pointer has been lifted up.
                    // We no longer need to keep track of the active pointer.
                    _activePointerId = InvalidPointerId;
                    break;

                case MotionEventActions.PointerUp:
                    // We only want to update the last touch position if the the appropriate pointer
                    // has been lifted off the screen.
                    pointerIndex = (int)(ev.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
                    int pointerId = ev.GetPointerId(pointerIndex);
                    if (pointerId == _activePointerId)
                    {
                        // This was our active pointer going up. Choose a new
                        // action pointer and adjust accordingly
                        int newPointerIndex = pointerIndex == 0 ? 1 : 0;
                        _lastTouchX = ev.GetX(newPointerIndex);
                        _activePointerId = ev.GetPointerId(newPointerIndex);
                    }
                    break;
            }
            return true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.Save();
            canvas.Translate(_posX, 0);
            _icon.Draw(canvas);
            canvas.Restore();
        }

        public void setTime(int hour, int minute)
        {
            int absolutHours = Math.Abs(START_HOUR - hour);

            int totalMinutes = absolutHours * 60;
            totalMinutes += minute;

            _posX = ((-1)*(totalMinutes));
            Toast.MakeText(context, "TotalMinutes: " + totalMinutes, ToastLength.Long).Show();
            Invalidate();

        }
    }
}