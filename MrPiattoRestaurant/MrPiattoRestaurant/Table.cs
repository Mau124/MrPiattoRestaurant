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

using MrPiattoRestaurant.Models;

namespace MrPiattoRestaurant
{
    public class Table
    {
        private const int width = 100;
        private const int height = 100;
        private const int textSize = 20;
        public string tableName { get; set; }
        public string tableDrawable { get; set; }
        public int seats { get; set; }

        private Drawable image;
        private Drawable border;
        private Paint paint;

        public Context context;

        public Client actualClient { get; set; }
        public bool borderOn { get; set; }
        public bool isOcupied { get; set; }
        //Blocked List
        //Reservation List
        //Width and heigh
        //Drawable borde rojo
        //Url for the image

        public int firstX { get; set; }
        public int firstY { get; set; }
        public int secondX { get; set; }
        public int secondY { get; set; }

        public Table(Context context, string tableDrawable, int firstX, int firstY, bool borderOn)
        {
            paint = new Paint();
            paint.Color = Color.ParseColor("#B0ADE8");
            paint.SetStyle(Paint.Style.Fill);
            paint.TextSize = textSize;
            paint.TextAlign = Paint.Align.Center;
            paint.SetTypeface(Typeface.SansSerif);

            this.context = context;
            this.tableDrawable = tableDrawable;
            this.firstX = firstX - (width / 2);
            this.firstY = firstY - (height / 2);
            this.borderOn = borderOn;

            secondX = this.firstX + width;
            secondY = this.firstY + height;

            image = context.Resources.GetDrawable(context.Resources.GetIdentifier(tableDrawable, "drawable", context.PackageName));
            border = context.Resources.GetDrawable(Resource.Drawable.border);

            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
            border.SetBounds(this.firstX - 5, this.firstY - 5, secondX + 5, secondY + 5);

            isOcupied = false;
        }

        public void setOcupied()
        {
            string auxDrawable = tableDrawable + "ocupied";
            image = context.Resources.GetDrawable(context.Resources.GetIdentifier(auxDrawable, "drawable", context.PackageName));
            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
        }
        public void DrawTable(Canvas canvas)
        {
            image.Draw(canvas);
            canvas.DrawText("Mesa 1", firstX + (width / 2), firstY + (height / 2) + (textSize / 2), paint);
            if (borderOn)
                border.Draw(canvas);
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

        public void setActualClient(Client actualClient)
        {
            this.actualClient = actualClient;
        }
    }
}