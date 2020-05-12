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
        public string lastName { get; set; }
        public int timeUsed { get; set; }
        public DateTime reservationDate { get; set; }
        public int seats { get; set; }
        public string floorName { get; set; }
        public string tableName { get; set; }
        public string Phone { get; set; }
        public Client() { }

        public Client(string name, string lastName, DateTime reservationDate, int seats, string Phone)
        {
            this.name = name;
            this.lastName = lastName;
            this.reservationDate = reservationDate;
            this.seats = seats;
            this.Phone = Phone;
        }
        public Client(string name, int timeUsed, DateTime reservationDate, int seats)
        {
            this.name = name;
            this.timeUsed = timeUsed;
            this.reservationDate = reservationDate;
            this.seats = seats;
        }
    }
}