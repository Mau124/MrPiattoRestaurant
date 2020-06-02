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

using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant
{
    [Activity(Label = "LoginActivity", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        Button login;
        TextView recover;

        EditText user, password;

        public APICaller API = new APICaller();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_login);

            // Create your application here
            login = FindViewById<Button>(Resource.Id.idLogin);
            recover = FindViewById<TextView>(Resource.Id.idRecover);
            user = FindViewById<EditText>(Resource.Id.idUser);
            password = FindViewById<EditText>(Resource.Id.idPassword);

            login.Click += loginClick;

            recover.Click += recoverPassword;
        }

        public void recoverPassword(object sender, EventArgs args)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_recover_password, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(this).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            EditText mail = content.FindViewById<EditText>(Resource.Id.idMail);
            Button button = content.FindViewById<Button>(Resource.Id.idButton);

            button.Click += delegate
            {

            };
        }

        public void loginClick(object sender, EventArgs args)
        {
            if (user.Text.Equals("") || password.Text.Equals(""))
            {
                Toast.MakeText(this, "Alguno de los campos no esta completo", ToastLength.Long).Show();
            } else
            {
                List<Restaurant> restaurants = API.GetAllRestaurants().Where(r => r.Mail == user.Text && r.Password == password.Text).ToList();

                if (restaurants.Any())
                {
                    Intent main = new Intent(this, typeof(MainActivity));
                    main.PutExtra("id", restaurants.First().Idrestaurant);
                    StartActivity(main);
                } else
                {
                    Toast.MakeText(this, "Usuario o contraseña incorrectos", ToastLength.Long).Show();
                }
            }
        }
    }
}