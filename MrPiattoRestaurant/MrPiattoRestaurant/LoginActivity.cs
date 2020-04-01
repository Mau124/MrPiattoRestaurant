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

namespace MrPiattoRestaurant
{
    [Activity(Label = "LoginActivity", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        Button login;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_login);

            // Create your application here
            login = FindViewById<Button>(Resource.Id.idLogin);

            login.Click += loginClick;
        }

        public void loginClick(object sender, EventArgs args)
        {
            Intent loginIntent = new Intent(this, typeof(MainActivity));

            StartActivity(loginIntent);
        }
    }
}