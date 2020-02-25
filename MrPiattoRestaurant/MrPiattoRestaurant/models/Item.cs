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

namespace MrPiattoRestaurant.models
{
    public class Item
    {
        public string sectionName { get; set; }
        public int tableID { get; set; }
        public string caption { get; set; }
        public bool isSection { get; set; }
        public bool isExpandable { get; set; }
        public string type { get; set; }
        public int seats { get; set; }
        public string tableDrawable { get; set; }
        public Item(string sectionName)
        {
            this.sectionName = sectionName;
            isSection = true;
            isExpandable = true;
        }
        public Item(string caption, int tableID, string type, int seats)
        {
            this.caption = caption;
            this.tableID = tableID;
            this.type = type;
            this.seats = seats;

            tableDrawable = type + seats;
            isSection = false;
        }
    }
}