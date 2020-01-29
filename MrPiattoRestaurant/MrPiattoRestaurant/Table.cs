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
        private const int width = 100;
        private const int height = 100;
        private string tableName { get; set; }

        private Drawable image;
        private Drawable border;

        public bool borderOn { get; set; }
        //Blocked List
        //Reservation List
        //Width and heigh
        //Drawable borde rojo
        //Url for the image

        private int firstX, firstY, secondX, secondY;

        public Table(Drawable image, Drawable border, int firstX, int firstY, bool borderOn)
        {
            this.image = image;
            this.border = border;
            this.firstX = firstX - (width / 2);
            this.firstY = firstY - (height / 2);
            this.borderOn = borderOn;

            secondX = this.firstX + width;
            secondY = this.firstY + height;

            borderOn = false;

            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
            border.SetBounds(this.firstX - 5, this.firstY - 5, secondX + 5, secondY + 5);
        }
        public void DrawTable(Canvas canvas)
        {
            image.Draw(canvas);
            if (borderOn)
                border.Draw(canvas);
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

        public void SetCoordinates(float x1, float y1)
        {
            firstX = (int)x1;
            firstY = (int)y1;
            secondX = (int)x1 + width;
            secondY = (int)y1 + height;
            image.SetBounds(firstX, firstY, secondX, secondY);
            border.SetBounds(firstX - 5, firstY - 5, secondX + 5, secondY + 5);
        }

    }
}