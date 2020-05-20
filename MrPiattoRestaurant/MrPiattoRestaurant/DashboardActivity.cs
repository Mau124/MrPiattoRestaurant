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
using Android.Net;
using Android.Content;
using Android.Widget;

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
        Button button;
        ImageView dismiss;
        ImageView getImage;
        EditText name;
        EditText desc;
        LinearLayout containerImages;
        private Restaurant restaurant = new Restaurant();
        private APICaller API = new APICaller();

        private Android.Net.Uri uriToSend;
        private int imageId = 1000;
        private string[] mails;
        private DateTime dateInterval1;
        private DateTime dateInterval2;

        ImageView promotions;

        ImageView iv;

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
            button = content.FindViewById<Button>(Resource.Id.idButton);
            getImage = content.FindViewById<ImageView>(Resource.Id.idImage);
            name = content.FindViewById<EditText>(Resource.Id.idName);
            desc = content.FindViewById<EditText>(Resource.Id.idDesc);
            containerImages = content.FindViewById<LinearLayout>(Resource.Id.idContainerImages);

            hourInterval1 = content.FindViewById<TextView>(Resource.Id.idHourInterval1);
            hourInterval2 = content.FindViewById<TextView>(Resource.Id.idHourInterval2);

            iv = new ImageView(this);
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(100, 100);
            lp.SetMargins(10, 10, 10, 10);
            iv.LayoutParameters = lp;
            containerImages.AddView(iv);

            uriToSend = null;
            InitializeDates();

            getImage.Click += delegate
            {
                Intent = new Intent();
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "Selecciona una imagen"), imageId);
            };
            
            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

            button.Click += delegate
            {
                // Checamos que todos los campos esten correctos
                getMails();
                if (name.Text.Equals(""))
                {
                    Toast.MakeText(Application.Context, "La promocion necesita un nombre", ToastLength.Short).Show();
                } else if (!areClients())
                {
                    Toast.MakeText(Application.Context, "No existen clientes en ese intervalo de tiempo", ToastLength.Short).Show();
                } else
                {
                    Intent send = new Intent(Intent.ActionSend);
                    send.SetType("message/rfc822");
                    send.PutExtra(Intent.ExtraEmail, mails);
                    send.PutExtra(Intent.ExtraSubject, name.Text);
                    if (uriToSend != null)
                    {
                        send.PutExtra(Intent.ExtraStream, uriToSend);
                    }
                    send.PutExtra(Intent.ExtraText, desc.Text);
                    try
                    {
                        StartActivity(Intent.CreateChooser(send, "Enviando correo..."));
                        alertDialog.Dismiss();

                    }
                    catch (Android.Content.ActivityNotFoundException ex)
                    {
                        Toast.MakeText(this, "Hubo un problema al enviar el correo", ToastLength.Long).Show();
                    }
                }
            };


            hourInterval1.Click += onHourInterval1;
            hourInterval2.Click += onHourInterval2;
        }

        private void InitializeDates()
        {
            List<Reservation> res = API.GetAllReservations(restaurant.Idrestaurant);

            dateInterval1 = res.Select(r => r.Date).Min(r => r.Date);
            dateInterval2 = res.Select(r => r.Date).Max(r => r.Date);

            hourInterval1.Text = dateInterval1.ToString("dd / MM / yyyy");
            hourInterval2.Text = dateInterval2.ToString("dd / MM / yyyy");
        }

        private void getMails()
        {
            List<User> users = API.GetUsers(restaurant.Idrestaurant, dateInterval1, dateInterval2);
            List<string> usersMails = new List<string>();

            foreach (User u in users)
            {
                usersMails.Add(u.Mail);
            }

            mails = usersMails.ToArray();
        }

        private bool areClients()
        {
            List<User> users = API.GetUsers(restaurant.Idrestaurant, dateInterval1, dateInterval2);

            if (users == null) return false;
            if (users.Any()) return true;
            return false;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if ((requestCode == imageId) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                uriToSend = uri;


                iv.SetImageURI(uri);
            }
        }

        private void onHourInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval1.Text = time.ToString("dd / MM / yyyy");
                dateInterval1 = time;
            });
            frag.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }

        private void onHourInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval2.Text = time.ToString("dd / MM / yyyy");
                dateInterval2 = time;
            });
            frag.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }
    }
}