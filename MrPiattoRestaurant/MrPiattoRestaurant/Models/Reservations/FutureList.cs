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
    public class FutureList
    {
        public string tableName { get; set; }
        public string personName { get; set; }
        public int reservationCode { get; set; }

        public FutureList(string tableName, string personName, int reservationCode)
        {
            this.tableName = tableName;
            this.personName = personName;
            this.reservationCode = reservationCode;
        }
    }
}