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
using Android.Graphics;
using Android.Support.V7.Widget;


using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.adapters;
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.Fragments
{
    public class AboutMe : Android.Support.V4.App.Fragment
    {
        private Context context;

        private Restaurant restaurant = new Restaurant();
        private List<Waiters> waiters = new List<Waiters>();
        private Schedule schedule = new Schedule();
        private APICaller API = new APICaller();
        private APIUpdate APIupdate = new APIUpdate();

        //This is for schedule
        TextView monday1, monday2;
        TextView tuesday1, tuesday2;
        TextView wednesday1, wednesday2;
        TextView thursday1, thursday2;
        TextView friday1, friday2;
        TextView saturday1, saturday2;
        TextView sunday1, sunday2;

        Button addWaiter;

        TextView restaurantName, restaurantMail, restaurantDesc;
        LinearLayout modifyRes, modifyPass, modifyHours;

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        WaitersAdapter mAdapter;

        public AboutMe(Context context, Restaurant restaurant)
        {
            this.context = context;
            this.restaurant = restaurant;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.activity_dashboard_aboutme, container, false);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            addWaiter = view.FindViewById<Button>(Resource.Id.idAddWaiter);

            restaurantName = view.FindViewById<TextView>(Resource.Id.idRestaurantName);
            restaurantMail = view.FindViewById<TextView>(Resource.Id.idRestaurantMail);
            restaurantDesc = view.FindViewById<TextView>(Resource.Id.idRestaurantDesc);

            modifyRes = view.FindViewById<LinearLayout>(Resource.Id.idModResInfo);
            modifyPass = view.FindViewById<LinearLayout>(Resource.Id.idModPass);
            modifyHours = view.FindViewById<LinearLayout>(Resource.Id.idModSche);

            InitializeWaiters();
            InitializeSchedule();

            mLayoutManager = new LinearLayoutManager(context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new WaitersAdapter(waiters, context);
            mRecyclerView.SetAdapter(mAdapter);

            addWaiter.Click += onAddWaiter;

            modifyRes.Click += modifyRestaurant;
            modifyPass.Click += modifyPassword;
            modifyHours.Click += modifyHoursRes;

            return view;
        }

        private void modifyRestaurant(object sender, EventArgs e)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_modify_res, null);

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };
        }
        private void InitializeWaiters()
        {
            waiters = API.GetWaiters(restaurant.Idrestaurant);
        }

        private void InitializeSchedule()
        {
            schedule = API.GetSchedule(restaurant.Idrestaurant);
        }

        private void modifyPassword(object sender, EventArgs e)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_new_password, null);

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };
        }

        private void modifyHoursRes(object sender, EventArgs e)
        {
            InitializeSchedule();

            View content = LayoutInflater.Inflate(Resource.Layout.layout_Schedule, null);

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

            monday1 = content.FindViewById<TextView>(Resource.Id.idMonday1);
            monday2 = content.FindViewById<TextView>(Resource.Id.idMonday2);
            tuesday1 = content.FindViewById<TextView>(Resource.Id.idTuesday1);
            tuesday2 = content.FindViewById<TextView>(Resource.Id.idTuesday2);
            wednesday1 = content.FindViewById<TextView>(Resource.Id.idWednesday1);
            wednesday2 = content.FindViewById<TextView>(Resource.Id.idWednesday2);
            thursday1 = content.FindViewById<TextView>(Resource.Id.idThursday1);
            thursday2 = content.FindViewById<TextView>(Resource.Id.idThursday2);
            friday1 = content.FindViewById<TextView>(Resource.Id.idFriday1);
            friday2 = content.FindViewById<TextView>(Resource.Id.idFriday2);
            saturday1 = content.FindViewById<TextView>(Resource.Id.idSaturday1);
            saturday2 = content.FindViewById<TextView>(Resource.Id.idSaturday2);
            sunday1 = content.FindViewById<TextView>(Resource.Id.idSunday1);
            sunday2 = content.FindViewById<TextView>(Resource.Id.idSunday2);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

            printSchedule();

            monday1.Click += clickMonday1;
            monday2.Click += clickMonday2;
            tuesday1.Click += clickTuesday1;
            tuesday2.Click += clickTuesday2;
            wednesday1.Click += clickWednesday1;
            wednesday2.Click += clickWednesday2;
            thursday1.Click += clickThursday1;
            thursday2.Click += clickThursday2;
            friday1.Click += clickFriday1;
            friday2.Click += clickFriday2;
            saturday1.Click += clickSaturday1;
            saturday2.Click += clickSaturday2;
            sunday1.Click += clickSunday1;
            sunday2.Click += clickSunday2;
        }

        private void printSchedule()
        {
            monday1.Text = (schedule.Otmonday.HasValue) ? schedule.Otmonday.Value.ToString(@"hh\:mm") : "--:--";
            monday2.Text = (schedule.Ctmonday.HasValue) ? schedule.Ctmonday.Value.ToString(@"hh\:mm") : "--:--";
            tuesday1.Text = (schedule.Ottuesday.HasValue) ? schedule.Ottuesday.Value.ToString(@"hh\:mm") : "--:--";
            tuesday2.Text = (schedule.Cttuestday.HasValue) ? schedule.Cttuestday.Value.ToString(@"hh\:mm") : "--:--";
            wednesday1.Text = (schedule.Otwednesday.HasValue) ? schedule.Otwednesday.Value.ToString(@"hh\:mm") : "--:--";
            wednesday2.Text = (schedule.Ctwednesday.HasValue) ? schedule.Ctwednesday.Value.ToString(@"hh\:mm") : "--:--";
            thursday1.Text = (schedule.Otthursday.HasValue) ? schedule.Otthursday.Value.ToString(@"hh\:mm") : "--:--";
            thursday2.Text = (schedule.Ctthursday.HasValue) ? schedule.Ctthursday.Value.ToString(@"hh\:mm") : "--:--";
            friday1.Text = (schedule.Otfriday.HasValue) ? schedule.Otfriday.Value.ToString(@"hh\:mm") : "--:--";
            friday2.Text = (schedule.Ctfriday.HasValue) ? schedule.Ctfriday.Value.ToString(@"hh\:mm") : "--:--";
            saturday1.Text = (schedule.Otsaturday.HasValue) ? schedule.Otsaturday.Value.ToString(@"hh\:mm") : "--:--";
            saturday2.Text = (schedule.Ctsaturday.HasValue) ? schedule.Ctsaturday.Value.ToString(@"hh\:mm") : "--:--";
            sunday1.Text = (schedule.Otsunday.HasValue) ? schedule.Otsunday.Value.ToString(@"hh\:mm") : "--:--";
            sunday2.Text = (schedule.Ctsunday.HasValue) ? schedule.Ctsunday.Value.ToString(@"hh\:mm") : "--:--";
        }

        private void clickMonday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                monday1.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickMonday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                monday2.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickTuesday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                tuesday1.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickTuesday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                tuesday2.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickWednesday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                wednesday1.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickWednesday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                wednesday2.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickThursday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                thursday1.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickThursday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                thursday2.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickFriday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                friday1.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickFriday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                friday2.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSaturday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                saturday1.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSaturday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                saturday2.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSunday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                sunday1.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSunday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                sunday2.Text = time.ToString("hh:mm");

            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void onAddWaiter(object sender, EventArgs e)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_add_waiter, null);

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };
        }
    }
}