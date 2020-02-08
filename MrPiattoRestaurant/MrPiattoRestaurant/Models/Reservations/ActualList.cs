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

namespace MrPiattoRestaurant.Models.Reservations
{
    public class ActualList
    {
        public string tableName { get; set; }
        public string personName { get; set; }
        public int timeUsed { get; set; }

        public ActualList(string personName, string tableName, int timeUsed)
        {
            this.personName = personName;
            this.tableName = tableName;
            this.timeUsed = timeUsed;
        }
    }
}