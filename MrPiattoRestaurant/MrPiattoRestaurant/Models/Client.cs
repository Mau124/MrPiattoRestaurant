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
    public class Client
    {
        public string name { get; set; }
        public int timeUsed { get; set; }
        public DateTime reservationDate { get; set; }

        public Client(string name, int timeUsed, DateTime reservationDate)
        {
            this.name = name;
            this.timeUsed = timeUsed;
            this.reservationDate = reservationDate;
        }
    }
}