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

namespace MrPiattoRestaurant.ModelsDB
{
    public class AuxiliarReservation
    {
        public int IdauxiliarReservation { get; set; }
        public int IdauxiliarTable { get; set; }
        public DateTime Date { get; set; }
        public int AmountOfPeople { get; set; }
        public int ReservationStatus { get; set; }
        public string? Url { get; set; }
        public string? Phone { get; set; }
        public bool? Checked { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        public virtual AuxiliarTables IdauxiliarTableNavigation { get; set; }
    }
}