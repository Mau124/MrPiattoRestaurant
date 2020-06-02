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
using Android.Graphics;

using MrPiattoRestaurant.Resources.utilities;
using MrPiattoRestaurant.ModelsDB;

namespace MrPiattoRestaurant.Notifications
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        private APICaller API = new APICaller();
        private APIUpdate APIupdate = new APIUpdate();
        private int time = new int();
        private Context context;
        private int idRes = new int();

        private List<Reservation> reservations = new List<Reservation>();
        private List<AuxiliarReservation> auxReservations = new List<AuxiliarReservation>();
        private List<ManualReservations> manualReservations = new List<ManualReservations>();

        public override void OnReceive(Context context, Intent intent)
        {
            this.context = context;
            idRes = intent.GetIntExtra("restaurant", 0);
            InitializeReservations();


            NotificationManager mNotificationManager = (NotificationManager)context.ApplicationContext.GetSystemService(Context.NotificationService);
            string CHANNEL_ID = "my_channel_01";
            string name = "my_channel";
            string Description = "This is my channel";
            NotificationImportance importance = NotificationImportance.High;

            NotificationChannel mChannel = new NotificationChannel(CHANNEL_ID, name, importance);
            mChannel.Description = Description;
            mChannel.EnableLights(true);
            mChannel.LightColor = Color.Red; 
            mChannel.EnableVibration(true);
            mChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });
            mChannel.SetShowBadge(false);

            mNotificationManager.CreateNotificationChannel(mChannel);

            var title = "Hello world!";
            var message = "Checkout this notification";

            Intent backIntent = new Intent(context, typeof(MainActivity));
            backIntent.SetFlags(ActivityFlags.NewTask);

            //The activity opened when we click the notification is SecondActivity
            //Feel free to change it to you own activity
            var resultIntent = new Intent(context, typeof(MainActivity));

            PendingIntent pending = PendingIntent.GetActivities(context, 0,
                new Intent[] { backIntent, resultIntent },
                PendingIntentFlags.OneShot);

            var builder =
                new Notification.Builder(context, CHANNEL_ID)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetAutoCancel(true)
                    .SetSmallIcon(Resource.Drawable.food)
                    .SetDefaults(NotificationDefaults.All);

            builder.SetContentIntent(pending);
            var notification = builder.Build();
            var manager = NotificationManager.FromContext(context);

            if (reservations.Any() && !auxReservations.Any() && !manualReservations.Any())
            {
                if (reservations.Count() == 1)
                {
                    title = reservations.First().IduserNavigation.FirstName + " " + reservations.First().IduserNavigation.LastName;
                    message = "Llegara en menos de 30 mins";
                } else
                {
                    title = "Varios clientes" ;
                    message = "Llegaran en 30 mins";
                }

                builder =
                new Notification.Builder(context, CHANNEL_ID)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetAutoCancel(true)
                    .SetSmallIcon(Resource.Drawable.food)
                    .SetDefaults(NotificationDefaults.All);

                builder.SetContentIntent(pending);
                notification = builder.Build();
                manager = NotificationManager.FromContext(context);
                manager.Notify(0, notification);

            } else if (!reservations.Any() && auxReservations.Any() && !manualReservations.Any())
            {
                if (auxReservations.Count() == 1)
                {
                    title = auxReservations.First().Name + " " + auxReservations.First().LastName;
                    message = "Llegara en menos de 30 mins";
                }
                else
                {
                    title = "Varios clientes";
                    message = "Llegaran en 30 mins";
                }

                builder =
                new Notification.Builder(context, CHANNEL_ID)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetAutoCancel(true)
                    .SetSmallIcon(Resource.Drawable.food)
                    .SetDefaults(NotificationDefaults.All);

                builder.SetContentIntent(pending);
                notification = builder.Build();
                manager = NotificationManager.FromContext(context);
                manager.Notify(0, notification);

            } else if (!reservations.Any() && !auxReservations.Any() && manualReservations.Any())
            {
                if (manualReservations.Count() == 1)
                {
                    title = manualReservations.First().Name + " " + manualReservations.First().LastName;
                    message = "Llegara en menos de 30 mins";
                }
                else
                {
                    title = "Varios clientes";
                    message = "Llegaran en 30 mins";
                }

                builder =
                new Notification.Builder(context, CHANNEL_ID)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetAutoCancel(true)
                    .SetSmallIcon(Resource.Drawable.food)
                    .SetDefaults(NotificationDefaults.All);

                builder.SetContentIntent(pending);
                notification = builder.Build();
                manager = NotificationManager.FromContext(context);
                manager.Notify(0, notification);

            } else if (reservations.Any() && auxReservations.Any() && manualReservations.Any())
            {
                title = "Varios clientes";
                message = "Llegaran en 30 mins";

                builder =
                new Notification.Builder(context, CHANNEL_ID)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetAutoCancel(true)
                    .SetSmallIcon(Resource.Drawable.food)
                    .SetDefaults(NotificationDefaults.All);

                builder.SetContentIntent(pending);
                notification = builder.Build();
                manager = NotificationManager.FromContext(context);
                manager.Notify(0, notification);
            }

            ++time;
            Toast.MakeText(context, "Se activo la notificacion" + time, ToastLength.Short).Show();
        }

        private void InitializeReservations()
        {
            reservations = API.GetNotReservations(idRes).Where(r => r.Checked == false).ToList();
            auxReservations = API.GetNotAuxReservations(idRes).Where(r => r.Checked == false).ToList();
            manualReservations = API.GetNotManReservations(idRes).Where(r => r.Checked == false).ToList();

            if (reservations != null)
            {
                foreach (Reservation r in reservations)
                {
                    r.Checked = true;
                    var response = APIupdate.UpdateReservation(r).Result;
                    Toast.MakeText(context, response, ToastLength.Long).Show();
                }
            }

            if (auxReservations != null)
            {
                foreach (AuxiliarReservation r in auxReservations)
                {
                    r.Checked = true;
                    var response = APIupdate.UpdateAuxReservation(r).Result;
                    Toast.MakeText(context, response, ToastLength.Long).Show();
                }
            }

            if (manualReservations != null)
            {
                foreach (ManualReservations r in manualReservations)
                {
                    r.Checked = true;
                    var response = APIupdate.UpdateManReservation(r).Result;
                    Toast.MakeText(context, response, ToastLength.Long).Show();
                }
            }
        }
    }
}