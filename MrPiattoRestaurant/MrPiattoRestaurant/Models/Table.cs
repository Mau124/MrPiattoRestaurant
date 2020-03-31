﻿using System;
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
        public Dictionary<Point, DateTime> TableDistributions { get; set; }

        public bool borderOn { get; set; }
        public bool isOcupied { get; set; }

        public int firstX { get; set; }
        public int firstY { get; set; }
        public int secondX { get; set; }
        public int secondY { get; set; }

        /// <summary>
        /// Constructor to create a table
        /// </summary>
        /// <param name="context">Context of the activity</param>
        /// <param name="type">Table type. Can be 'c' for circle or 's' for square</param>
        /// <param name="seats">Number of seats</param>
        /// <param name="firstX">Position of x</param>
        /// <param name="firstY">Position of y</param>
        /// <param name="borderOn">If appears with a border or not</param>
        public Table(Context context, string type, int seats, int firstX, int firstY, bool borderOn)
        {
            this.context = context;
            this.firstX = firstX - (width / 2);
            this.firstY = firstY - (height / 2);
            this.type = type;
            this.seats = seats;
            this.borderOn = borderOn;

            InitializeProperties();
            InitializePaint();
            InitializeImages();

            Client client = new Client("Pedro", 5, DateTime.Now);
            reservations.Add(client);
            reservations.Add(client);
            reservations.Add(client);

            client = new Client("Juan", 6, DateTime.Now.AddDays(5));
            reservations.Add(client);
            reservations.Add(client);
            reservations.Add(client);
        }
        //Inicialization methods
        private void InitializeProperties()
        {
            tableDrawable = type + seats;
            secondX = firstX + width;
            secondY = firstY + height;
            isOcupied = false;
            TableName = "Text";

            reservations = new List<Client>();

            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            Point p = new Point(firstX, firstY);

            TableDistributions = new Dictionary<Point, DateTime>();
            TableDistributions.Add(p, date);
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
        //Finish the initialization
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

        public void setTableName(string tableName)
        {
            this.TableName = tableName;
        }

        /// <summary>
        /// Function that adds a new coordinate for a table
        /// </summary>
        /// <param name="point">Point x and y where the new table is located</param>
        /// <param name="date">Date in wich that distribution is changed</param>
        public void AddDistribution(Point point, DateTime date)
        {
            int i = 0, j = 0;
            Point auxPoint;
            DateTime auxDate;

            if (date > TableDistributions.ElementAt(TableDistributions.Count() - 1).Value)
            {
                TableDistributions.Add(point, date);
            }

            while (date > TableDistributions.ElementAt(i).Value && i < TableDistributions.Count())
                i++;

            if (TableDistributions.Count() == 0 || i == TableDistributions.Count())
            {
                if (TableDistributions.Count != 0)
                    if (date != TableDistributions.ElementAt(0).Value)
                    {
                        TableDistributions.Add(point, date);
                    } else
                    {
                        TableDistributions.Add(point, date);
                        i++;
                    }
                else 
                    TableDistributions.Add(point, date);
                return;
            } 

            Dictionary<Point, DateTime> aux = new Dictionary<Point, DateTime>();

            while (j < i)
            {
                auxPoint = new Point(TableDistributions.ElementAt(j).Key);
                auxDate = new DateTime(TableDistributions.ElementAt(j).Value.Year, TableDistributions.ElementAt(j).Value.Month,
                    TableDistributions.ElementAt(j).Value.Day, TableDistributions.ElementAt(j).Value.Hour, TableDistributions.ElementAt(j).Value.Minute, 0);

                aux.Add(auxPoint, auxDate);
                j++;
            }

            auxPoint = new Point(point);
            auxDate = date;

            if (date != TableDistributions.ElementAt(j).Value)
            {
                aux.Add(auxPoint, auxDate);
            }
            else
            {
                aux.Add(auxPoint, auxDate);
                j++;
            }

            while (j < TableDistributions.Count())
            {
                auxPoint = new Point(TableDistributions.ElementAt(j).Key);
                auxDate = new DateTime(TableDistributions.ElementAt(j).Value.Year, TableDistributions.ElementAt(j).Value.Month,
                    TableDistributions.ElementAt(j).Value.Day, TableDistributions.ElementAt(j).Value.Hour, TableDistributions.ElementAt(j).Value.Minute, 0);

                aux.Add(auxPoint, auxDate);
                j++;
            }

            TableDistributions = new Dictionary<Point, DateTime>(aux);
        }

        /// <summary>
        /// Function that changes table image on the plane depending on the time sent
        /// </summary>
        /// <param name="hours">Hours for table</param>
        /// <param name="minutes">Minutes for table</param>
        public void ChangeTableDistribution(DateTime date)
        {
            int i = 0;
            while (i < TableDistributions.Count && date > TableDistributions.ElementAt(i).Value)
                i++;
            //Assign new x and y for table
            if (i != 0)
            {
                firstX = TableDistributions.ElementAt(i - 1).Key.X;
                firstY = TableDistributions.ElementAt(i - 1).Key.Y;

                secondX = firstX + width;
                secondY = firstY + height;
                setDrawable(type, seats);
                Toast.MakeText(Application.Context, "Distribucion: " + (i - 1) + "Size: " + TableDistributions.Count(), ToastLength.Short).Show();
            }
        }

        //Return true if hours and minutes are bigger than the table distribution in that index
        private bool Compare(int hours, int minutes, int tableIndex)
        {
            if (hours > TableDistributions.ElementAt(tableIndex).Value.Hour) return true;
            if (hours == TableDistributions.ElementAt(tableIndex).Value.Hour)
            {
                if (minutes > TableDistributions.ElementAt(tableIndex).Value.Minute) return true;
            }
            return false;
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