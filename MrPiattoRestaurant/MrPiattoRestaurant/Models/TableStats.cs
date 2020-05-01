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

namespace MrPiattoRestaurant.Models
{
    public class TableStats
    {
        public string Name { get; set; }
        public double? Average { get; set; }

        public TableStats(string name, double? average)
        {
            Name = name;
            Average = average;
        }
    }
}