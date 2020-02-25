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

namespace MrPiattoRestaurant
{
    public class TablePressedEventArgs
    {
        public int floorIterator { get; set; }
        public int tableIterator { get; set; }
        public TablePressedEventArgs()
        {
            floorIterator = -1;
            tableIterator = -1;
        }
    }
}