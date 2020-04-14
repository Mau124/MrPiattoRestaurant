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
    /// <summary>
    /// Clase Table
    /// Esta clase provee una abstracción de una mesa de la vida real. Es por esta razón
    /// que contiene todos los atributos y métodos que pudiesemos atribuir a una mesa 
    /// física utilizada en un restaurante como el número de asientos, si es circular o cuadrada,
    /// el cliente que esta sentado actualmente, etc. Además esta clase prove de propiedades y
    /// métodos para la representación visual de la mesa dentro de un lienzo (canvas)
    /// </summary>
    public class Table
    {
        private int width = 100;
        private int height = 100;
        private int textSize = 20;
        public string TableName { get; set; }
        public string tableDrawable { get; set; }
        public string type { get; set; }
        public int seats { get; set; }

        private Drawable image;
        private Drawable border;
        private Paint paint;

        public Context context;

        public Client actualClient { get; set; }
        public List<Client> reservations { get; set; }

        public bool borderOn { get; set; }
        public bool isOcupied { get; set; }

        public int firstX { get; set; }
        public int firstY { get; set; }
        public int secondX { get; set; }
        public int secondY { get; set; }
        public Table(Context context, string tableName, string type, int seats, int firstX, int firstY, bool borderOn)
        {
            this.context = context;
            this.firstX = firstX;
            this.firstY = firstY;
            this.type = type;
            this.seats = seats;
            this.borderOn = borderOn;
            TableName = tableName;

            InitializeProperties();
            InitializePaint();
            InitializeImages();
        }
        private void InitializeProperties()
        {
            tableDrawable = type + seats;
            secondX = firstX + width;
            secondY = firstY + height;
            isOcupied = false;

            reservations = new List<Client>();
        }
        private void InitializePaint()
        {
            paint = new Paint();
            paint.Color = Color.ParseColor("#B0ADE8");
            paint.SetStyle(Paint.Style.Fill);
            paint.TextSize = textSize;
            paint.TextAlign = Paint.Align.Center;
            paint.SetTypeface(Typeface.SansSerif);
            paint.AntiAlias = true;
        }
        private void InitializeImages()
        {
            image = context.Resources.GetDrawable(context.Resources.GetIdentifier(tableDrawable, "drawable", context.PackageName));
            border = context.Resources.GetDrawable(Resource.Drawable.border);

            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
            border.SetBounds(this.firstX - 5, this.firstY - 5, secondX + 5, secondY + 5);
        }

        public void setClient(List<Client> clients)
        {
            reservations = new List<Client>(clients);
        }
        public void setOcupied()
        {
            string auxDrawable = tableDrawable + "ocupied";
            image = context.Resources.GetDrawable(context.Resources.GetIdentifier(auxDrawable, "drawable", context.PackageName));
            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
        }

        public void setDrawable(string type, int seats)
        {
            string auxDrawable = type + seats;
            image = context.Resources.GetDrawable(context.Resources.GetIdentifier(auxDrawable, "drawable", context.PackageName));
            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
            border.SetBounds(this.firstX - 5, this.firstY - 5, secondX + 5, secondY + 5);
        }
        public void DrawTable(Canvas canvas)
        {

            image.Draw(canvas);
            canvas.DrawText(TableName, firstX + (width / 2), firstY + (height / 2) + (textSize / 2), paint);
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

        public int getWidth() { return width; }
        public int getHeight() { return height; }
    }
}