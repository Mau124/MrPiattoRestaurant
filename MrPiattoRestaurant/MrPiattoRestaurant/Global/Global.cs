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

using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.Global
{
    public class Global
    {
        public List<ActualList> actualList { get; set; }
        public List<FutureList> futureList { get; set; }
        public List<WaitList> waitList { get; set; }

        public Global()
        {
            actualList = new List<ActualList>();
            futureList = new List<FutureList>();
            waitList = new List<WaitList>();
        }
    }
}