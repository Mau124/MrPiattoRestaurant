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

        public void AddCoordinates(int x, int y)
        {
            firstX += x;
            secondX += x;
            firstY += y;
            secondY += y;
        }

    }
}