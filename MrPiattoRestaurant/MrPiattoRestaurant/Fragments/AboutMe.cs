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
using Android.Widget;
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
        private Policies policies = new Policies();
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

        TextView strikeType;
        EditText maxRes, minRes, maxArrive, minMod, strikeTime;
        Spinner spinner, spinner2, spinner3, spinner4;
        Switch switch1, switch2;
        Button mod, accept, addWaiter;

        EditText restaurantName, restaurantMail, restaurantDesc;
        Button modifyRes, modifyPass, modifyHours, acceptRes;

        Color main = new Color(222, 96, 104);
        Color unused = new Color(134, 142, 150);
        Color disable = new Color(205, 208, 209);

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        WaitersAdapter mAdapter;

        bool isModifying = false;
        bool isModifyinRes = false;

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

            spinner = view.FindViewById<Spinner>(Resource.Id.idSpinner);
            spinner2 = view.FindViewById<Spinner>(Resource.Id.idSpinner2);
            spinner3 = view.FindViewById<Spinner>(Resource.Id.idSpinner3);
            spinner4 = view.FindViewById<Spinner>(Resource.Id.idSpinner4);
            switch1 = view.FindViewById<Switch>(Resource.Id.idSwitch1);
            switch2 = view.FindViewById<Switch>(Resource.Id.idSwitch2);

            maxRes = view.FindViewById<EditText>(Resource.Id.idMaxRes);
            minRes = view.FindViewById<EditText>(Resource.Id.idMinRes);
            maxArrive = view.FindViewById<EditText>(Resource.Id.idMaxArrive);
            minMod = view.FindViewById<EditText>(Resource.Id.idMinMod);
            strikeTime = view.FindViewById<EditText>(Resource.Id.idStrikeTime);
            strikeType = view.FindViewById<TextView>(Resource.Id.idStrikeType);

            mod = view.FindViewById<Button>(Resource.Id.idMod);
            accept = view.FindViewById<Button>(Resource.Id.idAccept);
            addWaiter = view.FindViewById<Button>(Resource.Id.idAddWaiter);

            restaurantName = view.FindViewById<EditText>(Resource.Id.idRestaurantName);
            restaurantMail = view.FindViewById<EditText>(Resource.Id.idRestaurantMail);
            restaurantDesc = view.FindViewById<EditText>(Resource.Id.idRestaurantDesc);

            modifyRes = view.FindViewById<Button>(Resource.Id.idModifyRes);
            modifyPass = view.FindViewById<Button>(Resource.Id.idModifyPass);
            modifyHours = view.FindViewById<Button>(Resource.Id.idModifyHours);
            acceptRes = view.FindViewById<Button>(Resource.Id.idAcceptRes);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            spinner2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner2_ItemSelected);
            spinner3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner3_ItemSelected);
            spinner4.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner4_ItemSelected);

            InitializePolitics();
            InitializeRes();
            InitializePolicies();
            InitializeWaiters();
            InitializeSchedule();


            mLayoutManager = new LinearLayoutManager(context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new WaitersAdapter(waiters, context);
            mRecyclerView.SetAdapter(mAdapter);

            var adapter = ArrayAdapter.CreateFromResource(
                context, Resource.Array.periods_array, Resource.Layout.spinner_item_politics);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner2.Adapter = adapter;
            spinner3.Adapter = adapter;
            spinner4.Adapter = adapter;

            switch1.CheckedChange += switch1_Toggled;
            switch2.CheckedChange += switch2_Toggled;

            mod.Click += modifyPolitics;
            accept.Click += acceptPolitics;
            addWaiter.Click += onAddWaiter;

            modifyRes.Click += modifyRestaurant;
            acceptRes.Click += acceptRestaurant;
            modifyPass.Click += modifyPassword;
            modifyHours.Click += modifyHoursRes;

            return view;
        }

        private void InitializePolitics()
        {
            maxRes.Enabled = false;
            minRes.Enabled = false;
            maxArrive.Enabled = false;
            minMod.Enabled = false;
            strikeTime.Enabled = false;

            spinner.Enabled = false;
            spinner2.Enabled = false;
            spinner3.Enabled = false;
            spinner4.Enabled = false;

            switch1.Enabled = false;
            switch2.Enabled = false;

            switch2.ThumbDrawable.SetColorFilter(disable, PorterDuff.Mode.SrcAtop);
            switch2.TrackDrawable.SetColorFilter(disable, PorterDuff.Mode.Multiply);

            switch1.ThumbDrawable.SetColorFilter(disable, PorterDuff.Mode.SrcAtop);
            switch1.TrackDrawable.SetColorFilter(disable, PorterDuff.Mode.Multiply);

            accept.Visibility = ViewStates.Gone;
        }

        private void InitializeRes()
        {
            restaurantName.Hint = restaurant.Name;
            restaurantMail.Hint = restaurant.Mail;
            restaurantDesc.Hint = restaurant.Description;

            restaurantName.Enabled = false;
            restaurantMail.Enabled = false;
            restaurantDesc.Enabled = false;

            acceptRes.Visibility = ViewStates.Gone;
        }

        private void InitializePolicies()
        {
            policies = API.GetPolicies(restaurant.Idrestaurant);
            maxRes.Hint = policies.MaxTimeRes.ToString();
            minRes.Hint = policies.MinTimeRes.ToString();
            maxArrive.Hint = policies.MaxTimeArr.ToString();
            minMod.Hint = policies.ModTimeHours.ToString() + "h/ " + policies.ModTimeDays.ToString() + "d/ " + policies.ModTimeSeats.ToString() + "s";

            spinner.SetSelection(policies.MaxTimePer);
            spinner2.SetSelection(policies.MinTimePer);
            spinner3.SetSelection(policies.StrikeTypePer);
            spinner4.SetSelection(policies.MaxTimeArrPer);

            if (policies.StrikeType == 0)
            {
                switch1.Checked = false;
                strikeType.Text = "Permanente";
                strikeTime.Text = policies.StrikeType.ToString();
                strikeTime.Visibility = ViewStates.Gone;
                spinner4.Visibility = ViewStates.Gone;
            } else
            {
                switch1.Checked = true;
                strikeType.Text = "Horas";
                strikeTime.Visibility = ViewStates.Visible;
                spinner4.Visibility = ViewStates.Visible;
            }


            switch2.Checked = (policies.Strikes) ? true : false;
        }

        private void InitializeWaiters()
        {
            waiters = API.GetWaiters(restaurant.Idrestaurant);
        }

        private void InitializeSchedule()
        {
            schedule = API.GetSchedule(restaurant.Idrestaurant);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The period is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(context, toast, ToastLength.Long).Show();
        }

        private void spinner2_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The period is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(context, toast, ToastLength.Long).Show();
        }

        private void spinner3_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The period is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(context, toast, ToastLength.Long).Show();
        }

        private void spinner4_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The period is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(context, toast, ToastLength.Long).Show();
        }

        private void switch1_Toggled(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var toast = Toast.MakeText(context, "I Love Xamarin !" +
            (e.IsChecked ? "Yes" : " No"), ToastLength.Short);
            toast.Show();
            if (e.IsChecked)
            {
                switch1.ThumbDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                strikeType.Text = "Horas";
                strikeTime.Visibility = ViewStates.Visible;
                spinner4.Visibility = ViewStates.Visible;
            } else
            {
                switch1.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
                strikeType.Text = "Permanente";
                strikeTime.Visibility = ViewStates.Gone;
                spinner4.Visibility = ViewStates.Gone;
            }
        }

        private void switch2_Toggled(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var toast = Toast.MakeText(context, "I Love Xamarin !" +
            (e.IsChecked ? "Yes" : " No"), ToastLength.Short);
            toast.Show();
            if (e.IsChecked)
            {
                switch2.ThumbDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                switch2.TrackDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
            } else
            {
                switch2.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch2.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
            }
        }

        private void changeSwitch()
        {
            if (switch1.Checked)
            {
                switch1.ThumbDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                strikeType.Text = "Horas";
                strikeTime.Visibility = ViewStates.Visible;
                spinner4.Visibility = ViewStates.Visible;
            } else
            {
                switch1.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
                strikeType.Text = "Permanente";
                strikeTime.Visibility = ViewStates.Gone;
                spinner4.Visibility = ViewStates.Gone;
            }

            if (switch2.Checked)
            {
                switch2.ThumbDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                switch2.TrackDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
            } else
            {
                switch2.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch2.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
            }
        }

        private void modifyPolitics(object sender, EventArgs e)
        {
            if (!isModifying)
            {
                maxRes.Enabled = true;
                minRes.Enabled = true;
                maxArrive.Enabled = true;
                minMod.Enabled = true;
                strikeTime.Enabled = true;

                spinner.Enabled = true;
                spinner2.Enabled = true;
                spinner3.Enabled = true;
                spinner4.Enabled = true;

                switch1.Enabled = true;
                switch2.Enabled = true;

                changeSwitch();

                mod.Text = "Cancelar";
                accept.Visibility = ViewStates.Visible;
                isModifying = true;
            } else
            {
                mod.Text = "Modificar";
                InitializePolitics();
                isModifying = false;
            }
        }

        private void modifyRestaurant(object sender, EventArgs e)
        {
            if (!isModifyinRes)
            {
                restaurantName.Enabled = true;
                restaurantMail.Enabled = true;
                restaurantDesc.Enabled = true;
                isModifyinRes = true;

                acceptRes.Visibility = ViewStates.Visible;
                modifyRes.Text = "Cancelar";
                modifyPass.Visibility = ViewStates.Gone;
                modifyHours.Visibility = ViewStates.Gone;
            } else
            {
                InitializeRes();
                modifyPass.Visibility = ViewStates.Visible;
                modifyHours.Visibility = ViewStates.Visible;
                modifyRes.Text = "Modificar";
                isModifyinRes = false;
            }
        }

        private void modifyPassword(object sender, EventArgs e)
        {
            InitializeRes();
            modifyPass.Visibility = ViewStates.Visible;
            modifyRes.Text = "Modificar";
            isModifyinRes = false;

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
            InitializeRes();
            InitializeSchedule();
            modifyHours.Visibility = ViewStates.Visible;
            modifyRes.Text = "Modificar";
            isModifyinRes = false;

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

        private async void acceptRestaurant(object sender, EventArgs e)
        {
            InitializeRes();

            Restaurant res = new Restaurant(restaurant);

            res.Name = restaurantName.Text;
            res.Description = restaurantDesc.Text;

            var response = await APIupdate.UpdateRestaurant(res);
            Toast.MakeText(context, response, ToastLength.Long).Show();
            
            modifyPass.Visibility = ViewStates.Visible;
            modifyHours.Visibility = ViewStates.Visible;
            modifyRes.Text = "Modificar";
            isModifyinRes = false;
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

        private void acceptPolitics(object sender, EventArgs e)
        {
            mod.Text = "Modificar";
            InitializePolitics();
            isModifying = false;
        }
    }
}