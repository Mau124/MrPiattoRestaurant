﻿using System;
using System.Collections.Generic;

namespace MrPiattoRestaurant.ModelsDB
{
    public partial class RestaurantPhotos
    {
        public int IdrestaurantPhotos { get; set; }
        public int Idrestaurant { get; set; }
        public string Url { get; set; }

        //public virtual Restaurant IdrestaurantNavigation { get; set; }
    }
}
