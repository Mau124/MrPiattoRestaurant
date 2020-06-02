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

using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.Fragments
{
    public class PoliciesFragment : Android.Support.V4.App.Fragment
    {
        RelativeLayout penalType;
        TextView strikeType;
        EditText maxRes, minRes, maxArrive, modDay, modHour, modSeats, strikeTime;
        Spinner spinner, spinner2, spinner3, spinner4;
        Switch switch1, switch2;
        Button mod, accept;

        Restaurant restaurant = new Restaurant();
        Policies policies = new Policies();
        APICaller API = new APICaller();
        APIUpdate APIupdate = new APIUpdate();

        private Context context;

        Color main = new Color(222, 96, 104);
        Color unused = new Color(134, 142, 150);
        Color disable = new Color(205, 208, 209);

        bool isModifying = false;

        public PoliciesFragment(Context context, Restaurant restaurant)
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
            View view = inflater.Inflate(Resource.Layout.activity_dashboard_policies, container, false);

            mod = view.FindViewById<Button>(Resource.Id.idMod);
            accept = view.FindViewById<Button>(Resource.Id.idAccept);

            spinner = view.FindViewById<Spinner>(Resource.Id.idSpinner);
            spinner2 = view.FindViewById<Spinner>(Resource.Id.idSpinner2);
            spinner3 = view.FindViewById<Spinner>(Resource.Id.idSpinner3);
            spinner4 = view.FindViewById<Spinner>(Resource.Id.idSpinner4);
            switch1 = view.FindViewById<Switch>(Resource.Id.idSwitch1);
            switch2 = view.FindViewById<Switch>(Resource.Id.idSwitch2);
            penalType = view.FindViewById<RelativeLayout>(Resource.Id.idPenalType);

            maxRes = view.FindViewById<EditText>(Resource.Id.idMaxRes);
            minRes = view.FindViewById<EditText>(Resource.Id.idMinRes);
            maxArrive = view.FindViewById<EditText>(Resource.Id.idMaxArrive);
            modDay = view.FindViewById<EditText>(Resource.Id.idModDay);
            modHour = view.FindViewById<EditText>(Resource.Id.idModHour);
            modSeats = view.FindViewById<EditText>(Resource.Id.idModSeats);
            strikeTime = view.FindViewById<EditText>(Resource.Id.idStrikeTime);
            strikeType = view.FindViewById<TextView>(Resource.Id.idStrikeType);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            spinner2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner2_ItemSelected);
            spinner3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner3_ItemSelected);
            spinner4.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner4_ItemSelected);

            InitializePolitics();
            InitializePolicies();

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

            return view;
        }
        private void InitializePolitics()
        {
            isModifying = false;

            maxRes.Enabled = false;
            minRes.Enabled = false;
            maxArrive.Enabled = false;
            modDay.Enabled = false;
            modHour.Enabled = false;
            modSeats.Enabled = false;
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

        private void InitializePolicies()
        {
            maxRes.Text = "";
            minRes.Text = "";
            maxArrive.Text = "";
            modDay.Text = "";
            modHour.Text = "";
            modSeats.Text = "";

            policies = API.GetPolicies(restaurant.Idrestaurant);
            maxRes.Hint = policies.MaxTimeRes.ToString();
            minRes.Hint = policies.MinTimeRes.ToString();
            maxArrive.Hint = policies.MaxTimeArr.ToString();
            modDay.Hint = policies.ModTimeDays.ToString() + " dias";
            modHour.Hint = policies.ModTimeHours.ToString() + " horas";
            modSeats.Hint = policies.ModTimeSeats.ToString() + " asientos";

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
                penalType.Visibility = ViewStates.Gone;
            }
            else
            {
                switch1.Checked = true;
                strikeType.Text = "Horas";
                strikeTime.Visibility = ViewStates.Visible;
                spinner4.Visibility = ViewStates.Visible;
                penalType.Visibility = ViewStates.Visible;
            }


            switch2.Checked = (policies.Strikes) ? true : false;
        }

        private void modifyPolitics(object sender, EventArgs e)
        {
            if (!isModifying)
            {
                maxRes.Enabled = true;
                minRes.Enabled = true;
                maxArrive.Enabled = true;
                modDay.Enabled = true;
                modHour.Enabled = true;
                modSeats.Enabled = true;
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
            }
            else
            {
                mod.Text = "Modificar";
                InitializePolicies();
                InitializePolitics();
                isModifying = false;
            }
        }
        private async void acceptPolitics(object sender, EventArgs e)
        {
            if (!maxRes.Text.Equals(""))
            {
                int value = (int)spinner.SelectedItemId;

                switch (value)
                {
                    case 0:
                        if (Int32.Parse(maxRes.Text) > 20160 || Int32.Parse(maxRes.Text) < 120)
                        {
                            Toast.MakeText(context, "La hora máxima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MaxTimeRes = Int32.Parse(maxRes.Text);
                            policies.MaxTimePer = (int)spinner.SelectedItemId;
                        }
                        break;
                    case 1:
                        if (Int32.Parse(maxRes.Text) > 336 || Int32.Parse(maxRes.Text) < 2)
                        {
                            Toast.MakeText(context, "La hora máxima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MaxTimeRes = Int32.Parse(maxRes.Text);
                            policies.MaxTimePer = (int)spinner.SelectedItemId;
                        }
                        break;
                    case 2:
                        if (Int32.Parse(maxRes.Text) > 14)
                        {
                            Toast.MakeText(context, "La hora máxima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MaxTimeRes = Int32.Parse(maxRes.Text);
                            policies.MaxTimePer = (int)spinner.SelectedItemId;
                        }
                        break;
                    case 3:
                        if (Int32.Parse(maxRes.Text) > 2)
                        {
                            Toast.MakeText(context, "La hora máxima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        } else
                        {
                            policies.MaxTimeRes = Int32.Parse(maxRes.Text);
                            policies.MaxTimePer = (int)spinner.SelectedItemId;
                        }
                        break;
                    case 4:
                        Toast.MakeText(context, "La hora máxima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        break;
                    case 5:
                        Toast.MakeText(context, "La hora máxima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        break;
                }
            }
            if (!maxArrive.Text.Equals(""))
            {
                int value = (int)spinner3.SelectedItemId;

                switch (value)
                {
                    case 0:
                        if (Int32.Parse(maxArrive.Text) < 10 || Int32.Parse(maxArrive.Text) > 30)
                        {
                            Toast.MakeText(context, "El campo debe estar entre 10 y 30 minutos", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MaxTimeArr = Int32.Parse(maxArrive.Text);
                            policies.MaxTimeArrPer = (int)spinner3.SelectedItemId;
                        }
                        break;
                    case 1:
                        Toast.MakeText(context, "El campo debe estar entre 10 y 30 minutos", ToastLength.Long).Show();
                        break;
                    case 2:
                        Toast.MakeText(context, "El campo debe estar entre 10 y 30 minutos", ToastLength.Long).Show();
                        break;
                    case 3:
                        Toast.MakeText(context, "El campo debe estar entre 10 y 30 minutos", ToastLength.Long).Show();
                        break;
                    case 4:
                        Toast.MakeText(context, "El campo debe estar entre 10 y 30 minutos", ToastLength.Long).Show();
                        break;
                    case 5:
                        Toast.MakeText(context, "El campo debe estar entre 10 y 30 minutos", ToastLength.Long).Show();
                        break;
                }
            }
            if (switch1.Checked)
            {
                if (!strikeTime.Text.Equals(""))
                {
                    policies.StrikeType = Int32.Parse(strikeTime.Text);
                }
                policies.StrikeTypePer = (int)spinner4.SelectedItemId;
            } else
            {
                policies.StrikeType = 0;
                policies.StrikeTypePer = 0;
            }
            if (!minRes.Text.Equals(""))
            {
                policies.MinTimeRes = Int32.Parse(minRes.Text);
                int value = (int)spinner2.SelectedItemId;

                switch (value)
                {
                    case 0:
                        if (Int32.Parse(minRes.Text) > 20160 || Int32.Parse(minRes.Text) < 120)
                        {
                            Toast.MakeText(context, "La hora mínima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MinTimeRes = Int32.Parse(minRes.Text);
                            policies.MinTimePer = (int)spinner2.SelectedItemId;
                        }
                        break;
                    case 1:
                        if (Int32.Parse(minRes.Text) > 336 || Int32.Parse(minRes.Text) < 2)
                        {
                            Toast.MakeText(context, "La hora mínima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MinTimeRes = Int32.Parse(minRes.Text);
                            policies.MinTimePer = (int)spinner2.SelectedItemId;
                        }
                        break;
                    case 2:
                        if (Int32.Parse(minRes.Text) > 14)
                        {
                            Toast.MakeText(context, "La hora mínima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MinTimeRes = Int32.Parse(minRes.Text);
                            policies.MinTimePer = (int)spinner2.SelectedItemId;
                        }
                        break;
                    case 3:
                        if (Int32.Parse(minRes.Text) > 2)
                        {
                            Toast.MakeText(context, "La hora mínima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        }
                        else
                        {
                            policies.MinTimeRes = Int32.Parse(minRes.Text);
                            policies.MinTimePer = (int)spinner2.SelectedItemId;
                        }
                        break;
                    case 4:
                        Toast.MakeText(context, "La hora mínima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        break;
                    case 5:
                        Toast.MakeText(context, "La hora mínima debe ser 2 semanas como máximo y 2 horas como mínimo", ToastLength.Long).Show();
                        break;
                }
            }

            if (!modDay.Text.Equals(""))
            {
                policies.ModTimeDays = Int32.Parse(modDay.Text);
            }

            if (!modHour.Text.Equals(""))
            {
                if (!modDay.Text.Equals(""))
                {
                    if (Int32.Parse(modDay.Text) == 0 && Int32.Parse(modHour.Text) < 2)
                    {
                        Toast.MakeText(context, "Este apartado debe contener al menos dos horas", ToastLength.Long).Show();
                    } else
                    {
                        policies.ModTimeHours = Int32.Parse(modHour.Text);
                    }
                } else
                {
                    if (policies.ModTimeDays == 0 && Int32.Parse(modHour.Text) < 2)
                    {
                        Toast.MakeText(context, "Este apartado debe contener al menos dos horas", ToastLength.Long).Show();
                    } else
                    {
                        policies.ModTimeHours = Int32.Parse(modHour.Text);
                    }
                }
            }

            if (!modSeats.Text.Equals(""))
            {
                policies.ModTimeSeats = Int32.Parse(modSeats.Text);
            }

            // Falta minimo tiempo de modificar

            if (switch2.Checked)
            {
                policies.Strikes = true;
            } else
            {
                policies.Strikes = false;
            }

            var response = await APIupdate.UpdatePolicies(policies);
            Toast.MakeText(context, response, ToastLength.Long).Show();

            mod.Text = "Modificar";
            InitializePolitics();
            InitializePolicies();
            isModifying = false;
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
                penalType.Visibility = ViewStates.Visible;
            }
            else
            {
                switch1.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
                strikeType.Text = "Permanente";
                strikeTime.Visibility = ViewStates.Gone;
                spinner4.Visibility = ViewStates.Gone;
                penalType.Visibility = ViewStates.Gone;
            }

            if (switch2.Checked)
            {
                switch2.ThumbDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                switch2.TrackDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
            }
            else
            {
                switch2.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch2.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
            }
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
                penalType.Visibility = ViewStates.Visible;
            }
            else
            {
                switch1.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
                strikeType.Text = "Permanente";
                strikeTime.Visibility = ViewStates.Gone;
                spinner4.Visibility = ViewStates.Gone;
                penalType.Visibility = ViewStates.Gone;
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
            }
            else
            {
                switch2.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch2.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
            }
        }
    }
}