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

        // Esta propiedad indica tres posibles estados de la mesa descritos a continuacion
        // 0: No es posible seleccionar la mesa
        // 1: La mesa esta seleccionada
        // 2: La mesa no esta seleccionada
        public bool isJoined { get; set; }
        public int Selected { get; set; }
        public int Id { get; set; }
        public string TableName { get; set; }
        public string tableDrawable { get; set; }
        public string type { get; set; }
        public int seats { get; set; }

        public string actualColor { get; set; }

        public Drawable image;
        private Drawable border;
        private Paint paint;

        public Context context;

        public Client actualClient { get; set; }
        public List<Client> reservations { get; set; }
        public List<Table> tables { get; set; }

        public bool borderOn { get; set; }
        public bool isOcupied { get; set; }

        public int firstX { get; set; }
        public int firstY { get; set; }
        public int secondX { get; set; }
        public int secondY { get; set; }

        public delegate void TableUnJoinedEventHandler(Table table);
        public event TableUnJoinedEventHandler TableUnJoined;
        protected virtual void OnTableUnJoined()
        {
            if (TableUnJoined != null)
            {
                TableUnJoined(this);
            }
        }

        public void UnJoined()
        {
            OnTableUnJoined();
        }

        public delegate void DrawEventHandler();
        public event DrawEventHandler Draw;
        protected virtual void OnDraw()
        {
            if (Draw != null)
            {
                Draw();
            }
        }

        public void DrawTable()
        {
            OnDraw();
        }

        public Table(Context context, int Id, string tableName, string type, int seats, int firstX, int firstY, bool borderOn)
        {
            isJoined = false;
            this.context = context;
            this.Id = Id;
            this.firstX = firstX;
            this.firstY = firstY;
            this.type = type;
            this.seats = seats;
            this.borderOn = borderOn;

            Selected = 2;
            TableName = tableName;
            actualColor = "#B0ADE8";

            InitializeProperties();
            InitializeImages();
        }

        // Constructor utilizado cuando se crea una union de mesas
        public Table(Context context, List<Table> tables, Client actualClient, int seats)
        {
            isJoined = true;
            this.context = context;

            this.tables = new List<Table>(tables);

            this.actualClient = actualClient;
            this.seats = seats;

            TableName = "Union";

            firstX = tables.First().firstX;
            firstY = tables.First().firstY;
            type = tables.First().type;

            tableDrawable = type + seats + "ocupied";
            secondX = firstX + width;
            secondY = firstY + height;
            reservations = new List<Client>();

            actualColor = "#E2B7B8";
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

        private void setPaint(string color)
        {
            paint = new Paint();
            paint.Color = Color.ParseColor(color);
            paint.SetStyle(Paint.Style.Fill);
            paint.TextSize = textSize;
            paint.TextAlign = Paint.Align.Center;
            paint.SetTypeface(Typeface.SansSerif);
            paint.AntiAlias = true;
        }

        private void InitializePaint()
        {
            paint = new Paint();
            paint.Color = Color.ParseColor(actualColor);
            paint.SetStyle(Paint.Style.Fill);
            paint.TextSize = textSize;
            paint.TextAlign = Paint.Align.Center;
            paint.SetTypeface(Typeface.SansSerif);
            paint.AntiAlias = true;
        }
        public void InitializeImages()
        {
            InitializePaint();
            image = context.Resources.GetDrawable(context.Resources.GetIdentifier(tableDrawable, "drawable", context.PackageName));
            border = context.Resources.GetDrawable(Resource.Drawable.border);

            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
            border.SetBounds(this.firstX - 5, this.firstY - 5, secondX + 5, secondY + 5);
        }

        public void setClient(List<Client> clients)
        {
            reservations = new List<Client>(clients);
        }
        public void setOcupied(bool ocupied)
        {
            if (ocupied)
            {
                isOcupied = true;
                actualColor = "#E2B7B8";
                tableDrawable = tableDrawable + "ocupied";
                InitializeImages();
                OnDraw();
            } else
            {
                isOcupied = false;
                actualColor = "#B0ADE8";
                tableDrawable = type + seats;
                InitializeImages();
                OnDraw();
            }
        }

        public void setDrawable(string type, int seats)
        {
            this.type = type;
            this.seats = seats;
            string auxDrawable = type + seats;
            image = context.Resources.GetDrawable(context.Resources.GetIdentifier(auxDrawable, "drawable", context.PackageName));
            image.SetBounds(this.firstX, this.firstY, secondX, secondY);
            border.SetBounds(this.firstX - 5, this.firstY - 5, secondX + 5, secondY + 5);
        }

        public void setDrawable(string selection)
        {
            string auxDrawable = type + seats + selection;
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

        public void setSelected(int selected)
        {
            Selected = selected;

            switch(Selected)
            {
                case 0:
                    setDrawable("disable");
                    setPaint("#B5BDCD");
                    break;
                case 1:
                    setDrawable("selected");
                    setPaint("#7FCBFB");
                    break;
                case 2:
                    setDrawable("");
                    setPaint("#B0ADE8");
                    break;
            }
        }

        // Retorna verdadero si se va a sobrelapar con una reservacion
        public bool nextReservations()
        {
            DateTime date = DateTime.Now.AddHours(2);

            foreach (Client c in reservations)
            {
                if (date > c.reservationDate)
                    return true;
            }

            return false;
        }

        public int getWidth() { return width; }
        public int getHeight() { return height; }
    }
}