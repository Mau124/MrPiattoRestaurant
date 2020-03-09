using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

using MrPiattoRestaurant.Fragments;

namespace MrPiattoRestaurant
{
    [Activity(Label = "DashboardActivity")]
    public class DashboardActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            // Create your application here
            LoadFragment(Resource.Id.idActualList);
        }

        public void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.idActualList:
                    fragment = new AboutMe();
                    break;
                case Resource.Id.idFutureList:
                    fragment = new AboutMe();
                    break;
                case Resource.Id.idWaitList:
                    fragment = new AboutMe();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.idContent_frame, fragment)
                .Commit();
        }
    }
}