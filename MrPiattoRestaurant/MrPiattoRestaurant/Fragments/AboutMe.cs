﻿using System;
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
using MrPiattoRestaurant.adapters;

namespace MrPiattoRestaurant.Fragments
{
    public class AboutMe : Android.Support.V4.App.Fragment
    {
        private Context context;
        EditText maxRes, minRes, maxArrive, minMod;
        Spinner spinner, spinner2, spinner3;
        Switch switch1, switch2;
        Button mod, accept;
        List<string> waiters = new List<string>();

        Color main = new Color(222, 96, 104);
        Color unused = new Color(134, 142, 150);
        Color disable = new Color(205, 208, 209);

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        WaitersAdapter mAdapter;

        bool isModifying = false;

        public AboutMe(Context context)
        {
            this.context = context;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            waiters.Add("Mauricio Andres Flores Perez");
            waiters.Add("Juanito Perez Lopez");
            waiters.Add("Alan Mauricio Farfan Pita");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.activity_dashboard_aboutme, container, false);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            spinner = view.FindViewById<Spinner>(Resource.Id.idSpinner);
            spinner2 = view.FindViewById<Spinner>(Resource.Id.idSpinner2);
            spinner3 = view.FindViewById<Spinner>(Resource.Id.idSpinner3);
            switch1 = view.FindViewById<Switch>(Resource.Id.idSwitch1);
            switch2 = view.FindViewById<Switch>(Resource.Id.idSwitch2);

            maxRes = view.FindViewById<EditText>(Resource.Id.idMaxRes);
            minRes = view.FindViewById<EditText>(Resource.Id.idMinRes);
            maxArrive = view.FindViewById<EditText>(Resource.Id.idMaxArrive);
            minMod = view.FindViewById<EditText>(Resource.Id.idMinMod);

            mod = view.FindViewById<Button>(Resource.Id.idMod);
            accept = view.FindViewById<Button>(Resource.Id.idAccept);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            spinner2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner2_ItemSelected);
            spinner3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner3_ItemSelected);

            mLayoutManager = new LinearLayoutManager(context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new WaitersAdapter(waiters);
            mRecyclerView.SetAdapter(mAdapter);

            var adapter = ArrayAdapter.CreateFromResource(
                context, Resource.Array.periods_array, Resource.Layout.spinner_item_politics);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner2.Adapter = adapter;
            spinner3.Adapter = adapter;

            InitializePolitics();

            switch1.CheckedChange += switch1_Toggled;
            switch2.CheckedChange += switch2_Toggled;

            mod.Click += modifyPolitics;
            accept.Click += acceptPolitics;

            return view;
        }

        private void InitializePolitics()
        {
            maxRes.Enabled = false;
            minRes.Enabled = false;
            maxArrive.Enabled = false;
            minMod.Enabled = false;

            spinner.Enabled = false;
            spinner2.Enabled = false;
            spinner3.Enabled = false;

            switch1.Enabled = false;
            switch2.Enabled = false;

            switch2.ThumbDrawable.SetColorFilter(disable, PorterDuff.Mode.SrcAtop);
            switch2.TrackDrawable.SetColorFilter(disable, PorterDuff.Mode.Multiply);

            switch1.ThumbDrawable.SetColorFilter(disable, PorterDuff.Mode.SrcAtop);
            switch1.TrackDrawable.SetColorFilter(disable, PorterDuff.Mode.Multiply);

            accept.Visibility = ViewStates.Gone;
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

        private void switch1_Toggled(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var toast = Toast.MakeText(context, "I Love Xamarin !" +
            (e.IsChecked ? "Yes" : " No"), ToastLength.Short);
            toast.Show();
            if (e.IsChecked)
            {
                switch1.ThumbDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(main, PorterDuff.Mode.SrcAtop);
            } else
            {
                switch1.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
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
            } else
            {
                switch1.ThumbDrawable.SetColorFilter(unused, PorterDuff.Mode.SrcAtop);
                switch1.TrackDrawable.SetColorFilter(unused, PorterDuff.Mode.Multiply);
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

                spinner.Enabled = true;
                spinner2.Enabled = true;
                spinner3.Enabled = true;

                switch1.Enabled = true;
                switch2.Enabled = true;

                changeSwitch();

                mod.Text = "Cancelar";
                accept.Visibility = ViewStates.Visible;
                isModifying = true;
            } else
            {
                InitializePolitics();
                isModifying = false;
            }
        }

        private void acceptPolitics(object sender, EventArgs e)
        {
            InitializePolitics();
        }
    }
}