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

using Microcharts;
using SkiaSharp;
using Entry = Microcharts.Entry;
using Microcharts.Droid;

using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.Resources.utilities;
using MrPiattoRestaurant.ModelsDB;


namespace MrPiattoRestaurant.Fragments
{
    public class Statistics : Android.Support.V4.App.Fragment
    {
        Context context;

        double hour0, hour1, hour2, hour3;
        double hour4, hour5, hour6, hour7;
        double hour8, hour9, hour10, hour11;
        double hour12, hour13, hour14, hour15;
        double hour16, hour17, hour18, hour19;
        double hour20, hour21, hour22, hour23;

        List<Entry> entriesDays;
        List<Entry> entriesTableUse;
        List<Entry> entriesHours = new List<Entry>();
        List<Entry> entriesWaiters;
        List<Entry> entriesTableUseAverage;
        List<Entry> entriesAlexa;

        Spinner spinnerTableUse;
        Spinner spinnerWaiters;
        Spinner spinnerTableAverage;

        List<String> listTableUse = new List<String>();
        List<String> listWaiters = new List<String>();
        List<String> listTableAverage = new List<String>();

        TextView hourInterval1, hourInterval2;
        TextView tableUseInterval1, tableUseInterval2;
        TextView daysInterval1, daysInterval2;
        TextView waitersInterval1, waitersInterval2;
        TextView tableAverageInterval1, tableAverageInterval2;
        TextView alexaInterval1, alexaInterval2;

        Restaurant restaurant = new Restaurant();
        DayStatistics dayStatistics = new DayStatistics();
        List<HourStatistics> hourStatistics = new List<HourStatistics>();
        Schedule schedule = new Schedule();

        APICaller API = new APICaller();

        public Statistics(Context context, Restaurant restaurant)
        {
            this.context = context;
            this.restaurant = restaurant;
            dayStatistics = API.GetDayStatistics(restaurant.Idrestaurant);
            hourStatistics = API.GetHourStatistics(restaurant.Idrestaurant);
            schedule = API.GetSchedule(restaurant.Idrestaurant);
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            // Chart 1. Hours
            fillHours();

            // Chart 2. Table use per hour
            entriesTableUse = new List<Entry>
            {
                new Entry(20)
                {
                    Color = SKColor.Parse("#E06D64"),
                    Label = "Mesa 1",
                    ValueLabel = "20"
                },
                new Entry(40)
                {
                    Color = SKColor.Parse("#E38259"),
                    Label = "Mesa 2",
                    ValueLabel = "40"
                },
                new Entry(50)
                {
                    Color = SKColor.Parse("#E69852"),
                    Label = "Mesa 3",
                    ValueLabel = "50"
                },
                new Entry(15)
                {
                    Color = SKColor.Parse("#E5AE51"),
                    Label = "Mesa 4",
                    ValueLabel = "15"
                },
                new Entry(30)
                {
                    Color = SKColor.Parse("#EDC13F"),
                    Label = "Mesa 5",
                    ValueLabel = "30"
                },
            };

            // Chart 3. Days
            entriesDays = new List<Entry>
            {
                new Entry(200)
                {
                    Color = SKColor.Parse("#E06D64"),
                    Label = "Lunes",
                    ValueLabel = "200"
                },
                new Entry(400)
                {
                    Color = SKColor.Parse("#E38259"),
                    Label = "Martes",
                    ValueLabel = "400"
                },
                new Entry(500)
                {
                    Color = SKColor.Parse("#E69852"),
                    Label = "Miercoles",
                    ValueLabel = "500"
                },
                new Entry(150)
                {
                    Color = SKColor.Parse("#E5AE51"),
                    Label = "Jueves",
                    ValueLabel = "150"
                },
                new Entry(300)
                {
                    Color = SKColor.Parse("#EDC13F"),
                    Label = "Viernes",
                    ValueLabel = "300"
                },
            };

            // Chart 4. Waiters
            entriesWaiters = new List<Entry>
            {
                new Entry(9)
                {
                    Color = SKColor.Parse("#E06D64"),
                    Label = "Juan",
                    ValueLabel = "9"
                },
                new Entry(9.2f)
                {
                    Color = SKColor.Parse("#E38259"),
                    Label = "Pepe",
                    ValueLabel = "9.2"
                },
                new Entry(8.7f)
                {
                    Color = SKColor.Parse("#E69852"),
                    Label = "Jesus",
                    ValueLabel = "8.7"
                },
                new Entry(9)
                {
                    Color = SKColor.Parse("#E5AE51"),
                    Label = "Jose",
                    ValueLabel = "9"
                },
                new Entry(10)
                {
                    Color = SKColor.Parse("#EDC13F"),
                    Label = "Guillermo",
                    ValueLabel = "10"
                },
            };

            // Chart 5. Table Use Average per hour
            entriesTableUseAverage = new List<Entry>
            {
                new Entry(20)
                {
                    Color = SKColor.Parse("#E06D64"),
                    Label = "Mesa 1",
                    ValueLabel = "20"
                },
                new Entry(40)
                {
                    Color = SKColor.Parse("#E38259"),
                    Label = "Mesa 2",
                    ValueLabel = "40"
                },
                new Entry(50)
                {
                    Color = SKColor.Parse("#E69852"),
                    Label = "Mesa 3",
                    ValueLabel = "50"
                },
                new Entry(15)
                {
                    Color = SKColor.Parse("#E5AE51"),
                    Label = "Mesa 4",
                    ValueLabel = "15"
                },
                new Entry(30)
                {
                    Color = SKColor.Parse("#EDC13F"),
                    Label = "Mesa 5",
                    ValueLabel = "30"
                },
            };

            // Chart 5. Alexa
            entriesAlexa = new List<Entry>
            {
                new Entry(8.7f)
                {
                    Color = SKColor.Parse("#E06D64"),
                    Label = "Calidad comida",
                    ValueLabel = "8.7"
                },
                new Entry(9.2f)
                {
                    Color = SKColor.Parse("#E38259"),
                    Label = "Comodidad restaurante",
                    ValueLabel = "9.2"
                },
                new Entry(9.8f)
                {
                    Color = SKColor.Parse("#E69852"),
                    Label = "Servicio Restaurante",
                    ValueLabel = "9.8"
                },
            };

            //Adding to tableUse
            listTableUse.Add("1 ... 5");
            listTableUse.Add("6 ... 10");
            listTableUse.Add("11 ... 15");

            //Adding to waiters
            listWaiters.Add("1 ... 5");
            listWaiters.Add("6 ... 10");

            //Adding to tableAverage
            listTableAverage.Add("1 ... 5");
            listTableAverage.Add("6 ... 10");
            listTableAverage.Add("11 ... 15");
        }

        //Fill Hours with data from dabase
        private void fillHours()
        {
            entriesHours = new List<Entry>
            {
                new Entry(20)
                {
                    Color = SKColor.Parse("#E06D64"),
                    Label = "0",
                    ValueLabel = "20"
                },
                new Entry(40)
                {
                    Color = SKColor.Parse("#E1795E"),
                    Label = "1",
                    ValueLabel = "40"
                },
                new Entry(50)
                {
                    Color = SKColor.Parse("#E38558"),
                    Label = "2",
                    ValueLabel = "50"
                },
                new Entry(15)
                {
                    Color = SKColor.Parse("#E59154"),
                    Label = "3",
                    ValueLabel = "15"
                },
                new Entry(30)
                {
                    Color = SKColor.Parse("#E79E51"),
                    Label = "4",
                    ValueLabel = "30"
                },
                new Entry(20)
                {
                    Color = SKColor.Parse("#E5AB51"),
                    Label = "5",
                    ValueLabel = "20"
                },
                new Entry(40)
                {
                    Color = SKColor.Parse("#E3B752"),
                    Label = "6",
                    ValueLabel = "40"
                },
                new Entry(35)
                {
                    Color = SKColor.Parse("#E0C357"),
                    Label = "7",
                    ValueLabel = "35"
                },
                new Entry(25)
                {
                    Color = SKColor.Parse("#E3B752"),
                    Label = "8",
                    ValueLabel = "25"
                },
                new Entry(45)
                {
                    Color = SKColor.Parse("#E5AB51"),
                    Label = "9",
                    ValueLabel = "45"
                },
                new Entry(35)
                {
                    Color = SKColor.Parse("#E79E51"),
                    Label = "10",
                    ValueLabel = "35"
                },
                new Entry(35)
                {
                    Color = SKColor.Parse("#E59154"),
                    Label = "11",
                    ValueLabel = "35"
                },
                new Entry(35)
                {
                    Color = SKColor.Parse("#E38558"),
                    Label = "12",
                    ValueLabel = "35"
                },
                new Entry(45)
                {
                    Color = SKColor.Parse("#E1795E"),
                    Label = "13",
                    ValueLabel = "45"
                },
                new Entry(20)
                {
                    Color = SKColor.Parse("#E06D64"),
                    Label = "14",
                    ValueLabel = "20"
                },
                new Entry(22)
                {
                    Color = SKColor.Parse("#E1795E"),
                    Label = "15",
                    ValueLabel = "22"
                },
                new Entry(33)
                {
                    Color = SKColor.Parse("#E38558"),
                    Label = "16",
                    ValueLabel = "33"
                },
                new Entry(12)
                {
                    Color = SKColor.Parse("#E59154"),
                    Label = "17",
                    ValueLabel = "12"
                },
                new Entry(40)
                {
                    Color = SKColor.Parse("#E79E51"),
                    Label = "18",
                    ValueLabel = "40"
                },
                new Entry(57)
                {
                    Color = SKColor.Parse("#E5AB51"),
                    Label = "19",
                    ValueLabel = "57"
                },
                new Entry(35)
                {
                    Color = SKColor.Parse("#E3B752"),
                    Label = "20",
                    ValueLabel = "35"
                },
                new Entry(45)
                {
                    Color = SKColor.Parse("#E0C357"),
                    Label = "21",
                    ValueLabel = "45"
                },
                new Entry(51)
                {
                    Color = SKColor.Parse("#E3B752"),
                    Label = "22",
                    ValueLabel = "51"
                },
                new Entry(13)
                {
                    Color = SKColor.Parse("#E5AB51"),
                    Label = "23",
                    ValueLabel = "13"
                },
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.activity_dashboard_statistics, container, false);

            var chartView1 = view.FindViewById<ChartView>(Resource.Id.idChart1);
            var chartView2 = view.FindViewById<ChartView>(Resource.Id.idChart2);
            var chartView3 = view.FindViewById<ChartView>(Resource.Id.idChart3);
            var chartView4 = view.FindViewById<ChartView>(Resource.Id.idChart4);
            var chartView5 = view.FindViewById<ChartView>(Resource.Id.idChart5);
            var chartView6 = view.FindViewById<ChartView>(Resource.Id.idChart6);

            spinnerTableUse = view.FindViewById<Spinner>(Resource.Id.idSpinnerTableUse);
            spinnerWaiters = view.FindViewById<Spinner>(Resource.Id.idSpinnerWaiters);
            spinnerTableAverage = view.FindViewById<Spinner>(Resource.Id.idSpinnerTableAverage);

            hourInterval1 = view.FindViewById<TextView>(Resource.Id.idHourInterval1);
            hourInterval2 = view.FindViewById<TextView>(Resource.Id.idHourInterval2);
            tableUseInterval1 = view.FindViewById<TextView>(Resource.Id.idTableUseInterval1);
            tableUseInterval2 = view.FindViewById<TextView>(Resource.Id.idTableUseInterval2);
            daysInterval1 = view.FindViewById<TextView>(Resource.Id.idDaysInterval1);
            daysInterval2 = view.FindViewById<TextView>(Resource.Id.idDaysInterval2);
            waitersInterval1 = view.FindViewById<TextView>(Resource.Id.idWaitersInterval1);
            waitersInterval2 = view.FindViewById<TextView>(Resource.Id.idWaitersInterval2);
            tableAverageInterval1 = view.FindViewById<TextView>(Resource.Id.idTableAverageInterval1);
            tableAverageInterval2 = view.FindViewById<TextView>(Resource.Id.idTableAverageInterval2);
            alexaInterval1 = view.FindViewById<TextView>(Resource.Id.idAlexaInterval1);
            alexaInterval2 = view.FindViewById<TextView>(Resource.Id.idAlexaInterval2);


            var chart1 = new LineChart() { Entries = entriesHours };
            var chart2 = new BarChart() { Entries = entriesTableUse };
            var chart3 = new DonutChart() { Entries = entriesDays };
            var chart4 = new RadialGaugeChart() { Entries = entriesWaiters };
            var chart5 = new BarChart() { Entries = entriesTableUseAverage };
            var chart6 = new BarChart() { Entries = entriesAlexa };

            chartView1.Chart = chart1;
            chartView2.Chart = chart2;
            chartView3.Chart = chart3;
            chartView4.Chart = chart4;
            chartView5.Chart = chart5;
            chartView6.Chart = chart6;

            var adapter1 = new ArrayAdapter<string>(context, Resource.Layout.statistics_spinner_item, listTableUse);
            adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerTableUse.Adapter = adapter1;

            var adapter2 = new ArrayAdapter<string>(context, Resource.Layout.statistics_spinner_item, listWaiters);
            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerWaiters.Adapter = adapter2;

            var adapter3 = new ArrayAdapter<string>(context, Resource.Layout.statistics_spinner_item, listTableAverage);
            adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerTableAverage.Adapter = adapter3;

            hourInterval1.Click += onHourInterval1;
            hourInterval2.Click += onHourInterval2;
            tableUseInterval1.Click += onTableUseInterval1;
            tableUseInterval2.Click += onTableUseInterval2;
            daysInterval1.Click += onDaysInterval1;
            daysInterval2.Click += onDaysInterval2;
            waitersInterval1.Click += onWaitersInterval1;
            waitersInterval2.Click += onWaitersInterval2;
            tableAverageInterval1.Click += onTableAverageInterval1;
            tableAverageInterval2.Click += onTableAverageInterval2;
            alexaInterval1.Click += onAlexaInterval1;
            alexaInterval2.Click += onAlexaInterval2;

            return view;
        }

        private void onHourInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval1.Text = time.ToLongDateString();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onHourInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval2.Text = time.ToLongDateString();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableUseInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableUseInterval1.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableUseInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableUseInterval2.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onDaysInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                daysInterval1.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onDaysInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                daysInterval2.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onWaitersInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                waitersInterval1.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onWaitersInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                waitersInterval2.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableAverageInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableAverageInterval1.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableAverageInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableAverageInterval2.Text = time.ToString("dd/MM/yyyy");

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onAlexaInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                alexaInterval1.Text = time.ToLongDateString();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onAlexaInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                alexaInterval2.Text = time.ToLongDateString();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
    }
}
