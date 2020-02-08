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
    public class WaitList
    {
        public string personName { get; set; }
        public int numSeats { get; set; }

        public WaitList(string personName, int numSeats)
        {
            this.personName = personName;
            this.numSeats = numSeats;
        }
    }
}