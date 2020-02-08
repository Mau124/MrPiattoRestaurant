using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class WaitFragment : Fragment
    {
        Button newWait;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.recycler_waitList, container, false);
            newWait = view.FindViewById<Button>(Resource.Id.idButton);

            newWait.Click += delegate { OnAddWait(); };

            return view;
        }

        public void OnAddWait()
        {
            Toast.MakeText(Application.Context, "Se presiono el boton desde wait", ToastLength.Long).Show();
        }
    }
}