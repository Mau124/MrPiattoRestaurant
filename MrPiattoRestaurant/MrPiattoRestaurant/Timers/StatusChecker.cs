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
using MrPiattoRestaurant.InteractiveViews;

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
        private List<GestureRecognizerView> floors = new List<GestureRecognizerView>();
        private TimeLineView timeLine;
        private TextView hour;

        public delegate void UpdateEventHandler();
        public event UpdateEventHandler Update;
        protected virtual void OnUpdate()
        {
            if (Update != null)
            {
                Update();
            }
        }

        public StatusChecker(Context context, int count, TimeLineView timeLine, List<GestureRecognizerView> floors, TextView hour)
        {
            invokeCount = 0;
            this.context = context;
            this.timeLine = timeLine;
            this.floors = floors;
            this.hour = hour;
            maxCount = count;
        }

        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss") + (++invokeCount));

            timeLine.SetTime(DateTime.Now.Hour, DateTime.Now.Minute);

            foreach (GestureRecognizerView floor in floors)
            {
                foreach (Table t in floor.ocupiedTables)
                {
                    t.actualClient.timeUsed++;
                }
            }
            OnUpdate();

            if (invokeCount == maxCount)
            {
                invokeCount = 0;
                autoEvent.Set();
            }

        }
    }
}