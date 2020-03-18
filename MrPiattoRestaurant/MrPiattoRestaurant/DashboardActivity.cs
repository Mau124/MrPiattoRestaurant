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
        ImageView aboutMe, statistics, promotions;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            // Create your application here

            aboutMe = FindViewById<ImageView>(Resource.Id.idAboutMe);
            statistics = FindViewById<ImageView>(Resource.Id.idStatistics);
            promotions = FindViewById<ImageView>(Resource.Id.idPromotions);

            aboutMe.Click += aboutMeClick;
            statistics.Click += statisticsClick;
            promotions.Click += promotionsClick;

            LoadFragment(Resource.Id.idAboutMe);

        }

        public void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.idAboutMe:
                    fragment = new AboutMe();
                    break;
                case Resource.Id.idStatistics:
                    fragment = new Statistics(this);
                    break;
                case Resource.Id.idWaitList:
                    fragment = new Promotions();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.idContent_frame, fragment)
                .Commit();
        }

        public void aboutMeClick(object sender, EventArgs args)
        {
            statistics.SetBackgroundResource(0);
            statistics.SetImageResource(Resource.Drawable.report);
            promotions.SetBackgroundResource(0);
            promotions.SetImageResource(Resource.Drawable.bullhorn);
            aboutMe.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            aboutMe.SetImageResource(Resource.Drawable.userdashboardselected);
            LoadFragment(Resource.Id.idAboutMe);
        }

        public void statisticsClick(object sender, EventArgs args)
        {
            aboutMe.SetBackgroundResource(0);
            aboutMe.SetImageResource(Resource.Drawable.userdashboard);
            promotions.SetBackgroundResource(0);
            promotions.SetImageResource(Resource.Drawable.bullhorn);
            statistics.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            statistics.SetImageResource(Resource.Drawable.reportselected);
            LoadFragment(Resource.Id.idStatistics);
        }

        public void promotionsClick(object sender, EventArgs args)
        {
            aboutMe.SetBackgroundResource(0);
            aboutMe.SetImageResource(Resource.Drawable.userdashboard);
            statistics.SetBackgroundResource(0);
            statistics.SetImageResource(Resource.Drawable.report);
            promotions.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            promotions.SetImageResource(Resource.Drawable.bullhornselected);
            LoadFragment(Resource.Id.idStatistics);
        }
    }
}