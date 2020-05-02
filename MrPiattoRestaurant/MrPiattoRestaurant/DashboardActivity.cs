using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;

using MrPiattoRestaurant.Fragments;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant
{
    [Activity(Label = "DashboardActivity")]
    public class DashboardActivity : AppCompatActivity
    {
        ImageView aboutMe, statistics, photosGallery, policies;
        TextView hourInterval1, hourInterval2;
        ImageView dismiss;
        private Restaurant restaurant = new Restaurant();
        private APICaller API = new APICaller();

        ImageView promotions;

        Android.Support.V4.App.Fragment aboutMeFragment;
        Android.Support.V4.App.Fragment policiesFragment;
        Android.Support.V4.App.Fragment statisticsFragment;
        Android.Support.V4.App.Fragment photosGalleryFragment;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            // Create your application here

            int idRes = Intent.GetIntExtra("id", 0);
            InitializeRestaurant(idRes);

            aboutMe = FindViewById<ImageView>(Resource.Id.idAboutMe);
            policies = FindViewById<ImageView>(Resource.Id.idPolicies);
            statistics = FindViewById<ImageView>(Resource.Id.idStatistics);
            photosGallery = FindViewById<ImageView>(Resource.Id.idPhotosGallery);
            promotions = FindViewById<ImageView>(Resource.Id.idPromotions);

            aboutMe.Click += aboutMeClick;
            policies.Click += policiesClick;
            statistics.Click += statisticsClick;
            photosGallery.Click += photosGalleryClick;
            promotions.Click += promotionsClick;

            aboutMeFragment = new AboutMe(this, restaurant);
            policiesFragment = new PoliciesFragment(this, restaurant);
            statisticsFragment = new Statistics(this, restaurant);
            photosGalleryFragment = new PhotosGallery(this, restaurant);

            LoadFragment(Resource.Id.idAboutMe);

        }

        private void InitializeRestaurant(int idRestaurant)
        {
            restaurant = API.GetRestaurant(idRestaurant);
        }

        public void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.idAboutMe:
                    fragment = aboutMeFragment;
                    break;
                case Resource.Id.idPolicies:
                    fragment = policiesFragment;
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
            photosGallery.SetImageResource(Resource.Drawable.art);
            aboutMe.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            aboutMe.SetImageResource(Resource.Drawable.userdashboardselected);
            policies.SetBackgroundResource(0);
            policies.SetImageResource(Resource.Drawable.policies);
            LoadFragment(Resource.Id.idAboutMe);
        }

        public void policiesClick(object sender, EventArgs args)
        {
            statistics.SetBackgroundResource(0);
            statistics.SetImageResource(Resource.Drawable.report);
            photosGallery.SetBackgroundResource(0);
            photosGallery.SetImageResource(Resource.Drawable.art);
            aboutMe.SetBackgroundResource(0);
            aboutMe.SetImageResource(Resource.Drawable.userdashboard);
            policies.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            policies.SetImageResource(Resource.Drawable.policiesSelected);
            LoadFragment(Resource.Id.idPolicies);
        }

        public void statisticsClick(object sender, EventArgs args)
        {
            aboutMe.SetBackgroundResource(0);
            aboutMe.SetImageResource(Resource.Drawable.userdashboard);
            photosGallery.SetBackgroundResource(0);
            photosGallery.SetImageResource(Resource.Drawable.art);
            statistics.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            statistics.SetImageResource(Resource.Drawable.reportselected);
            policies.SetBackgroundResource(0);
            policies.SetImageResource(Resource.Drawable.policies);
            LoadFragment(Resource.Id.idStatistics);
        }

        public void photosGalleryClick(object sender, EventArgs args)
        {
            aboutMe.SetBackgroundResource(0);
            aboutMe.SetImageResource(Resource.Drawable.userdashboard);
            statistics.SetBackgroundResource(0);
            statistics.SetImageResource(Resource.Drawable.report);
            photosGallery.SetBackgroundResource(Resource.Drawable.dashboard_selected_item);
            photosGallery.SetImageResource(Resource.Drawable.artSelected);
            policies.SetBackgroundResource(0);
            policies.SetImageResource(Resource.Drawable.policies);
            LoadFragment(Resource.Id.idPhotosGallery);
        }

        public void promotionsClick(object sender, EventArgs args)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_promotions, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(this).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

            hourInterval1 = content.FindViewById<TextView>(Resource.Id.idHourInterval1);
            hourInterval2 = content.FindViewById<TextView>(Resource.Id.idHourInterval2);

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

            hourInterval1.Click += onHourInterval1;
            hourInterval2.Click += onHourInterval2;

        }

        private void onHourInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval1.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }

        private void onHourInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval2.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }
    }
}