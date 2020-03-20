﻿using System;
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
        ImageView aboutMe, statistics, photosGallery;

        Android.Support.V4.App.Fragment aboutMeFragment;
        Android.Support.V4.App.Fragment statisticsFragment;
        Android.Support.V4.App.Fragment photosGalleryFragment;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            // Create your application here

            aboutMe = FindViewById<ImageView>(Resource.Id.idAboutMe);
            statistics = FindViewById<ImageView>(Resource.Id.idStatistics);
            photosGallery = FindViewById<ImageView>(Resource.Id.idPhotosGallery);

            aboutMe.Click += aboutMeClick;
            statistics.Click += statisticsClick;
            photosGallery.Click += photosGalleryClick;

            aboutMeFragment = new AboutMe();
            statisticsFragment = new Statistics(this);
            photosGalleryFragment = new PhotosGallery(this);

            LoadFragment(Resource.Id.idAboutMe);

        }

        public void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.idAboutMe:
                    fragment = aboutMeFragment;
                    break;
                case Resource.Id.idStatistics:
                    fragment = statisticsFragment;
                    break;
                case Resource.Id.idPhotosGallery:
                    fragment = photosGalleryFragment;
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
            photosGallery.SetBackgroundResource(0);
            photosGallery.SetImageResource(Resource.Drawable.bullhorn);
            aboutMe.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            aboutMe.SetImageResource(Resource.Drawable.userdashboardselected);
            LoadFragment(Resource.Id.idAboutMe);
        }

        public void statisticsClick(object sender, EventArgs args)
        {
            aboutMe.SetBackgroundResource(0);
            aboutMe.SetImageResource(Resource.Drawable.userdashboard);
            photosGallery.SetBackgroundResource(0);
            photosGallery.SetImageResource(Resource.Drawable.bullhorn);
            statistics.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            statistics.SetImageResource(Resource.Drawable.reportselected);
            LoadFragment(Resource.Id.idStatistics);
        }

        public void photosGalleryClick(object sender, EventArgs args)
        {
            aboutMe.SetBackgroundResource(0);
            aboutMe.SetImageResource(Resource.Drawable.userdashboard);
            statistics.SetBackgroundResource(0);
            statistics.SetImageResource(Resource.Drawable.report);
            photosGallery.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            photosGallery.SetImageResource(Resource.Drawable.bullhornselected);
            LoadFragment(Resource.Id.idPhotosGallery);
        }
    }
}