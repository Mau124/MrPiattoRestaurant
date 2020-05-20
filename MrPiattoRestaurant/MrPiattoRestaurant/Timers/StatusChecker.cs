using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.Timers
{
    public class StatusChecker
    {
        private int invokeCount;
        private int maxCount;
        private Context context;
        private APICaller API = new APICaller();

        private List<Reservation> reservation = new List<Reservation>();
        private List<AuxiliarReservation> auxReservation = new List<AuxiliarReservation>();
        private List<ManualReservations> manReservation = new List<ManualReservations>();

        public StatusChecker(Context context, int count)
        {
            invokeCount = 0;
            this.context = context;
            maxCount = count;
        }

        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss") + (++invokeCount));

            if (invokeCount == maxCount)
            {
                invokeCount = 0;
                autoEvent.Set();
            }

        }
    }
}