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

namespace MrPiattoRestaurant
{
    public class Table
    {
        private int firstX, firstY, secondX, secondY;
        private Drawable icon;

        public Table(Drawable icon, int firstX, int firstY, int secondX, int secondY)
        {
            this.firstX = firstX;
            this.firstY = firstY;
            this.secondX = secondX;
            this.secondY = secondY;
            this.icon = icon;

            icon.SetBounds(firstX, firstY, secondX, secondY);
        }

        public Drawable getIcon()
        {
            return icon;
        }

        public int getFirstX()
        {
            return firstX;
        }

        public int getFistY()
        {
            return firstY;
        }

        public int getSecondX()
        {
            return secondX;
        }

        public int getSecondY()
        {
            return secondY;
        }

        public void SetCoordinates(float x1, float y1, float x2, float y2)
        {
            icon.SetBounds((int)x1, (int)y1, (int)x2, (int)y2);
            firstX = (int)x1;
            firstY = (int)y1;
            secondX = (int)x2;
            secondY = (int)y2;
        }

    }
}