﻿using System;
using System.Collections.Generic;

namespace MrPiattoRestaurant.ModelsDB
{
    public partial class DayStatistics
    {
        public int IddaysStatistics { get; set; }
        public int Idrestaurant { get; set; }
        public double? AverageMonday { get; set; }
        public double? AverageTuesday { get; set; }
        public double? AverageWednesday { get; set; }
        public double? AverageThursday { get; set; }
        public double? AverageFriday { get; set; }
        public double? AverageSaturday { get; set; }
        public double? AverageSunday { get; set; }
        public DateTime DateStatistics { get; set; }

        //public virtual Restaurant IdrestaurantNavigation { get; set; }
    }
}
