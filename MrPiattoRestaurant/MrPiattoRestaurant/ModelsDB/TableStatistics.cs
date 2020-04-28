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

namespace MrPiattoRestaurant.ModelsDB
{
    public class TableStatistics
    {
        public int IDTableStatistics { get; set; }
        public int IDRestaurant { get; set; }
        public int IDTable { get; set; }
        public double AvarageUse { get; set; }
        public DateTime DateStatistics { get; set; }

        //public virtual Restaurant IdrestaurantNavigation { get; set; }
        //public virtual RestaurantTables IdrestaurantTablesNavigation { get; set; }
    }
}