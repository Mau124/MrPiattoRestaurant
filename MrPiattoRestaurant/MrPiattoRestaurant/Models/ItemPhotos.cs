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
    public class ItemPhotos
    {
        public int imageID { get; set; }
        public string imageText { get; set; }

        public ItemPhotos()
        {
            imageID = Resource.Drawable.res;
            imageText = "Default";
        }

        public ItemPhotos(int imageID, string imageText)
        {
            this.imageID = imageID;
            this.imageText = imageText;
        }
    }
}