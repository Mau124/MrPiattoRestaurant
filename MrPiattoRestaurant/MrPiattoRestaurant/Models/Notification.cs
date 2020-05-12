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
    public class Notification
    {
        public Notification(string Name, string LastName, string TableName, DateTime Date, string Phone)
        {
            this.Name = Name;
            this.LastName = LastName;
            this.TableName = TableName;
            this.Date = Date;
            this.Phone = Phone;
        }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string TableName { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
    }
}