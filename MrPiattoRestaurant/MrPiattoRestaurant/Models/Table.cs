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
    /// Class for a restaurant table
    /// </summary>
    public class Table
    {
        private const int width = 100;
        private const int height = 100;
        private const int textSize = 20;
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

            Point p = new Point(firstX, firstY);
            TableDistributions = new Dictionary<Point, DateTime>();
            TableDistributions.Add(p, DateTime.Now);
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
            for (int i = 0; i < TableDistributions.Count; ++i)
            {
                if ((TableDistributions.ElementAt(i).Value.Hour == date.Hour)
                    && (TableDistributions.ElementAt(i).Value.Minute == date.Minute))
                {
                    return;
                }
            }
            TableDistributions.Add(point, date);
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