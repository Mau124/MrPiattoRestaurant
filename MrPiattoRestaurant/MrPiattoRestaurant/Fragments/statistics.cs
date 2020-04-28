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
using MrPiattoRestaurant.Models;


namespace MrPiattoRestaurant.Fragments
{
    public class Statistics : Android.Support.V4.App.Fragment
    {
        Context context;

        List<Entry> entriesDays = new List<Entry>();
        List<Entry> entriesTableHours = new List<Entry>();
        List<Entry> entriesHours = new List<Entry>();
        List<Entry> entriesWaiters = new List<Entry>();
        List<Entry> entriesTableUseAverage = new List<Entry>();
        List<Entry> entriesAlexa = new List<Entry>();
        List<string> colors = new List<string>();
        List<string> daysWeek = new List<string>();

        List<double?> daysAverages = new List<double?>();
        List<double?> hourAverages = new List<double?>();

        double? avgHour0 = null, avgHour1 = null, avgHour2 = null, avgHour3 = null;
        double? avgHour4 = null, avgHour5 = null, avgHour6 = null, avgHour7 = null;
        double? avgHour8 = null, avgHour9 = null, avgHour10 = null, avgHour11 = null;
        double? avgHour12 = null, avgHour13 = null, avgHour14 = null, avgHour15 = null;
        double? avgHour16 = null, avgHour17 = null, avgHour18 = null, avgHour19 = null;
        double? avgHour20 = null, avgHour21 = null, avgHour22 = null, avgHour23 = null;

        double? avgDay0 = null, avgDay1 = null, avgDay2 = null;
        double? avgDay3 = null, avgDay4 = null, avgDay5 = null;
        double? avgDay6 = null;

        Spinner spinnerTableUse;
        Spinner spinnerWaiters;
        Spinner spinnerTableAverage;

        List<String> listTableUse = new List<String>();
        List<String> listWaiters = new List<String>();
        List<String> listTableAverage = new List<String>();

        DateTime hInterval1, hInterval2;
        DateTime dInterval1, dInterval2;
        DateTime thourInterval1, thourInterval2;
        DateTime tdayInterval1, tdayInterval2;
        DateTime wInterval1, wInterval2;
        DateTime sInterval1, sInterval2;

        TextView hourInterval1, hourInterval2;
        TextView tableUseInterval1, tableUseInterval2;
        TextView daysInterval1, daysInterval2;
        TextView waitersInterval1, waitersInterval2;
        TextView tableAverageInterval1, tableAverageInterval2;
        TextView alexaInterval1, alexaInterval2;

        Restaurant restaurant = new Restaurant();
        List<DayStatistics> dayStatistics = new List<DayStatistics>();
        List<HourStatistics> hourStatistics = new List<HourStatistics>();
        List<TableStatistics> tableStatistics = new List<TableStatistics>();
        List<Waiters> waiters = new List<Waiters>();
        List<Surveys> surveys = new List<Surveys>();
        Schedule schedule = new Schedule();

        ChartView chartView1;
        ChartView chartView2;
        ChartView chartView3;
        ChartView chartView4;
        ChartView chartView5;
        ChartView chartView6;

        LineChart chart1;
        BarChart chart2;
        DonutChart chart3;
        RadialGaugeChart chart4;
        BarChart chart5;
        BarChart chart6;

        APICaller API = new APICaller();

        public Statistics(Context context, Restaurant restaurant)
        {
            this.context = context;
            this.restaurant = restaurant;
            dayStatistics = API.GetDayStatistics(restaurant.Idrestaurant);
            hourStatistics = API.GetHourStatistics(restaurant.Idrestaurant);
            tableStatistics = API.GetTableStatistics(restaurant.Idrestaurant);
            waiters = API.GetWaiters(restaurant.Idrestaurant);
            surveys = API.GetSurveys(restaurant.Idrestaurant);
            schedule = API.GetSchedule(restaurant.Idrestaurant);
            fillColors();
            fillDaysWeek();
        }

        private void fillColors()
        {
            colors.Add("#E06D64");
            colors.Add("#E9785D");
            colors.Add("#EF8555");
            colors.Add("#F3934E");
            colors.Add("#F4A247");
            colors.Add("#F2B141");
            colors.Add("#EDC13F");
        }

        private void fillDaysWeek()
        {
            daysWeek.Add("Lunes");
            daysWeek.Add("Martes");
            daysWeek.Add("Miercoles");
            daysWeek.Add("Jueves");
            daysWeek.Add("Viernes");
            daysWeek.Add("Sabado");
            daysWeek.Add("Domingo");
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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
            entriesHours.Clear();
            hourAverages.Clear();
            List<HourStatistics> auxStatistics = new List<HourStatistics>();
            auxStatistics = hourStatistics.Where(d => d.DateStatistics >= hInterval1 && d.DateStatistics <= hInterval2).ToList();

            avgHour0 = null;
            avgHour1 = null;
            avgHour2 = null;
            avgHour3 = null;
            avgHour4 = null;
            avgHour5 = null;
            avgHour6 = null;
            avgHour7 = null;
            avgHour8 = null;
            avgHour9 = null;
            avgHour10 = null;
            avgHour11 = null;
            avgHour12 = null;
            avgHour13 = null;
            avgHour14 = null; 
            avgHour15 = null;
            avgHour16 = null;
            avgHour17 = null;
            avgHour18 = null;
            avgHour19 = null;
            avgHour20 = null;
            avgHour21 = null;
            avgHour22 = null;
            avgHour23 = null;

            foreach (HourStatistics h in auxStatistics)
            {
                avgHour0 = (avgHour0 != null) ? ((h.Average0000.HasValue) ? avgHour0 * h.Average0000 : avgHour0) : ((h.Average0000.HasValue) ? h.Average0000 : avgHour0);
                avgHour1 = (avgHour1 != null) ? ((h.Average0100.HasValue) ? avgHour1 * h.Average0100 : avgHour1) : ((h.Average0100.HasValue) ? h.Average0100 : avgHour1);
                avgHour2 = (avgHour2 != null) ? ((h.Average0200.HasValue) ? avgHour2 * h.Average0200 : avgHour2) : ((h.Average0200.HasValue) ? h.Average0200 : avgHour2);
                avgHour3 = (avgHour3 != null) ? ((h.Average0300.HasValue) ? avgHour3 * h.Average0300 : avgHour3) : ((h.Average0300.HasValue) ? h.Average0300 : avgHour3);
                avgHour4 = (avgHour4 != null) ? ((h.Average0400.HasValue) ? avgHour4 * h.Average0400 : avgHour4) : ((h.Average0400.HasValue) ? h.Average0400 : avgHour4);
                avgHour5 = (avgHour5 != null) ? ((h.Average0500.HasValue) ? avgHour5 * h.Average0500 : avgHour5) : ((h.Average0500.HasValue) ? h.Average0500 : avgHour5);
                avgHour6 = (avgHour6 != null) ? ((h.Average0600.HasValue) ? avgHour6 * h.Average0600 : avgHour6) : ((h.Average0600.HasValue) ? h.Average0600 : avgHour6);
                avgHour7 = (avgHour7 != null) ? ((h.Average0700.HasValue) ? avgHour7 * h.Average0700 : avgHour7) : ((h.Average0700.HasValue) ? h.Average0700 : avgHour7);
                avgHour8 = (avgHour8 != null) ? ((h.Average0800.HasValue) ? avgHour8 * h.Average0800 : avgHour8) : ((h.Average0800.HasValue) ? h.Average0800 : avgHour8);
                avgHour9 = (avgHour9 != null) ? ((h.Average0900.HasValue) ? avgHour9 * h.Average0900 : avgHour9) : ((h.Average0900.HasValue) ? h.Average0900 : avgHour9);
                avgHour10 = (avgHour10 != null) ? ((h.Average1000.HasValue) ? avgHour10 * h.Average1000 : avgHour10) : ((h.Average1000.HasValue) ? h.Average1000 : avgHour10);
                avgHour11 = (avgHour11 != null) ? ((h.Average1100.HasValue) ? avgHour11 * h.Average1100 : avgHour11) : ((h.Average1100.HasValue) ? h.Average1100 : avgHour11);
                avgHour12 = (avgHour12 != null) ? ((h.Average1200.HasValue) ? avgHour12 * h.Average1200 : avgHour12) : ((h.Average1200.HasValue) ? h.Average1200 : avgHour12);
                avgHour13 = (avgHour13 != null) ? ((h.Average1300.HasValue) ? avgHour13 * h.Average1300 : avgHour13) : ((h.Average1300.HasValue) ? h.Average1300 : avgHour13);
                avgHour14 = (avgHour14 != null) ? ((h.Average1400.HasValue) ? avgHour14 * h.Average1400 : avgHour14) : ((h.Average1400.HasValue) ? h.Average1400 : avgHour14);
                avgHour15 = (avgHour15 != null) ? ((h.Average1500.HasValue) ? avgHour15 * h.Average1500 : avgHour15) : ((h.Average1500.HasValue) ? h.Average1500 : avgHour15);
                avgHour16 = (avgHour16 != null) ? ((h.Average1600.HasValue) ? avgHour16 * h.Average1600 : avgHour16) : ((h.Average1600.HasValue) ? h.Average1600 : avgHour16);
                avgHour17 = (avgHour17 != null) ? ((h.Average1700.HasValue) ? avgHour17 * h.Average1700 : avgHour17) : ((h.Average1700.HasValue) ? h.Average1700 : avgHour17);
                avgHour18 = (avgHour18 != null) ? ((h.Average1800.HasValue) ? avgHour18 * h.Average1800 : avgHour18) : ((h.Average1800.HasValue) ? h.Average1800 : avgHour18);
                avgHour19 = (avgHour19 != null) ? ((h.Average1900.HasValue) ? avgHour19 * h.Average1900 : avgHour19) : ((h.Average1900.HasValue) ? h.Average1900 : avgHour19);
                avgHour20 = (avgHour20 != null) ? ((h.Average2000.HasValue) ? avgHour20 * h.Average2000 : avgHour20) : ((h.Average2000.HasValue) ? h.Average2000 : avgHour20);
                avgHour21 = (avgHour21 != null) ? ((h.Average2100.HasValue) ? avgHour21 * h.Average2100 : avgHour21) : ((h.Average2100.HasValue) ? h.Average2100 : avgHour21);
                avgHour22 = (avgHour22 != null) ? ((h.Average2200.HasValue) ? avgHour22 * h.Average2200 : avgHour22) : ((h.Average2200.HasValue) ? h.Average2200 : avgHour22);
                avgHour23 = (avgHour23 != null) ? ((h.Average2300.HasValue) ? avgHour23 * h.Average2300 : avgHour23) : ((h.Average2300.HasValue) ? h.Average2300 : avgHour23);
            }

            hourAverages.Add(avgHour0);
            hourAverages.Add(avgHour1);
            hourAverages.Add(avgHour2);
            hourAverages.Add(avgHour3);
            hourAverages.Add(avgHour4);
            hourAverages.Add(avgHour5);
            hourAverages.Add(avgHour6);
            hourAverages.Add(avgHour7);
            hourAverages.Add(avgHour8);
            hourAverages.Add(avgHour9);
            hourAverages.Add(avgHour10);
            hourAverages.Add(avgHour11);
            hourAverages.Add(avgHour12);
            hourAverages.Add(avgHour13);
            hourAverages.Add(avgHour14);
            hourAverages.Add(avgHour15);
            hourAverages.Add(avgHour16);
            hourAverages.Add(avgHour17);
            hourAverages.Add(avgHour18);
            hourAverages.Add(avgHour19);
            hourAverages.Add(avgHour20);
            hourAverages.Add(avgHour21);
            hourAverages.Add(avgHour22);
            hourAverages.Add(avgHour23);

            for (int i = 0; i < 24; ++i)
            {
                if (hourAverages[i] != null)
                {
                    Entry entry = new Entry((float)hourAverages[i])
                    {
                        Color = SKColor.Parse("#E06D64"),
                        Label = i.ToString(),
                        ValueLabel = hourAverages[i].ToString()
                    };
                    entriesHours.Add(entry);
                }
            }

            chart1 = new LineChart() { Entries = entriesHours };
            chartView1.Chart = chart1;
        }

        private void fillDays()
        {
            entriesDays.Clear();
            daysAverages.Clear();

            List<DayStatistics> auxStatistics = new List<DayStatistics>();
            auxStatistics = dayStatistics.Where(d => d.DateStatistics >= dInterval1 && d.DateStatistics <= dInterval2).ToList();

            avgDay0 = null;
            avgDay1 = null;
            avgDay2 = null;
            avgDay3 = null;
            avgDay4 = null;
            avgDay5 = null;
            avgDay6 = null;

            foreach (DayStatistics d in auxStatistics)
            {
                avgDay0 = (avgDay0 != null) ? ((d.AverageMonday.HasValue) ? avgDay0 * d.AverageMonday : avgDay0) : ((d.AverageMonday.HasValue) ? d.AverageMonday : avgDay0);
                avgDay1 = (avgDay1 != null) ? ((d.AverageTuesday.HasValue) ? avgDay1 * d.AverageTuesday : avgDay1) : ((d.AverageTuesday.HasValue) ? d.AverageTuesday : avgDay1);
                avgDay2 = (avgDay2 != null) ? ((d.AverageWednesday.HasValue) ? avgDay2 * d.AverageWednesday : avgDay2) : ((d.AverageWednesday.HasValue) ? d.AverageWednesday : avgDay2);
                avgDay3 = (avgDay3 != null) ? ((d.AverageThursday.HasValue) ? avgDay3 * d.AverageThursday : avgDay3) : ((d.AverageThursday.HasValue) ? d.AverageThursday : avgDay3);
                avgDay4 = (avgDay4 != null) ? ((d.AverageFriday.HasValue) ? avgDay4 * d.AverageFriday : avgDay4) : ((d.AverageFriday.HasValue) ? d.AverageFriday : avgDay4);
                avgDay5 = (avgDay5 != null) ? ((d.AverageSaturday.HasValue) ? avgDay5 * d.AverageSaturday : avgDay5) : ((d.AverageSaturday.HasValue) ? d.AverageSaturday : avgDay5);
                avgDay6 = (avgDay6 != null) ? ((d.AverageSunday.HasValue) ? avgDay6 * d.AverageSunday : avgDay6) : ((d.AverageSunday.HasValue) ? d.AverageSunday : avgDay6);
            }

            daysAverages.Add(avgDay0);
            daysAverages.Add(avgDay1);
            daysAverages.Add(avgDay2);
            daysAverages.Add(avgDay3);
            daysAverages.Add(avgDay4);
            daysAverages.Add(avgDay5);
            daysAverages.Add(avgDay6);

            for (int i = 0; i < 7; ++i)
            {
                if (daysAverages[i] != null)
                {
                    Entry entry = new Entry((float)daysAverages[i])
                    {
                        Color = SKColor.Parse(colors[i % 7]),
                        Label = daysWeek[i],
                        ValueLabel = daysAverages[i].ToString()
                    };
                    entriesDays.Add(entry);
                }
            }

            chart3 = new DonutChart() { Entries = entriesDays };
            chartView3.Chart = chart3;
        }

        private void fillTableHours()
        {
            entriesTableHours.Clear();

            List<TableStatistics> auxStatistics = new List<TableStatistics>();
            List<TableStats> tableStats = new List<TableStats>();

            // Obtenemos y ordenamos las mesas por el id de la mesa
            auxStatistics = tableStatistics.Where(d => d.DateStatistics >= thourInterval1 && d.DateStatistics <= thourInterval2).ToList();
            auxStatistics.OrderBy(id => id.IDTable); 

            // Iteramos por cada  mesa y obtenemos el promedio de sus horas
            for (int i = 0; i < auxStatistics.Count;)
            {
                int auxId = auxStatistics[i].IDTable;
                double averageAux = 0;
                while (i < auxStatistics.Count && auxId == auxStatistics[i].IDTable)
                {
                    if (averageAux == 0)
                        averageAux = auxStatistics[i].AvarageUse;
                    else
                        averageAux = (averageAux + auxStatistics[i].AvarageUse) / 2;
                    ++i;
                }
                tableStats.Add(new TableStats(i.ToString(), averageAux));
            }

            // Mostramos los resultados en las graficas
            for (int i = 0; i < tableStats.Count; ++i)
            {
                Entry entry = new Entry((float)tableStats[i].Average)
                {
                    Color = SKColor.Parse(colors[i % 7]),
                    Label = tableStats[i].Name,
                    ValueLabel = tableStats[i].Average.ToString()
                };
                entriesTableHours.Add(entry);
            }

            chart2 = new BarChart() { Entries = entriesTableHours };
            chartView2.Chart = chart2;
        }

        private void fillWaiters()
        {
            entriesWaiters.Clear();

            List<Waiters> auxWaiters = new List<Waiters>();
            List<TableStats> tableStats = new List<TableStats>();

            auxWaiters = waiters.Where(d => d.DateStatistics >= wInterval1 && d.DateStatistics <= wInterval2).ToList();

            // Colocamos los valores
            var rates = auxWaiters
                .GroupBy(d => d.WaiterFirstName, a => a.WaiterRating)
                .Select(g => new 
                {
                    Name = g.Key,
                    Avg = g.Average()
                });

            foreach (var rate in rates)
            {
                tableStats.Add(new TableStats(rate.Name, rate.Avg));
            }

            // Mostramos los resultados en las graficas
            for (int i = 0; i < tableStats.Count; ++i)
            {
                Entry entry = new Entry((float)tableStats[i].Average)
                {
                    Color = SKColor.Parse(colors[i % 7]),
                    Label = tableStats[i].Name,
                    ValueLabel = tableStats[i].Average.ToString()
                };
                entriesWaiters.Add(entry);
            }

            chart4 = new RadialGaugeChart() { Entries = entriesWaiters };
            chartView4.Chart = chart4;
        }

        private void fillTableDays()
        {
            entriesTableUseAverage.Clear();

            List<TableStatistics> auxStatistics = new List<TableStatistics>();
            List<TableStats> tableStats = new List<TableStats>();
            List<int> ids = new List<int>();

            // Obtenemos y ordenamos las mesas por el id de la mesa
            auxStatistics = tableStatistics.Where(d => d.DateStatistics >= tdayInterval1 && d.DateStatistics <= tdayInterval2).ToList();

            ids = auxStatistics.Select(id => id.IDTable).Distinct().ToList();

            // Iteramos por cada  mesa y obtenemos el promedio de sus horas
            foreach (int id in ids)
            {
                double averageAux = 0;
                var rates = auxStatistics
                    .Where(i => i.IDTable == id)
                    .GroupBy(d => d.DateStatistics.Date, a => a.AvarageUse)
                    .Select(g => new
                    {
                        DateStatistics = g.Key,
                        AvarageUse = g.Sum()
                    });

                foreach(var x in rates)
                {
                    if (averageAux == 0)
                        averageAux = x.AvarageUse;
                    else
                        averageAux = (averageAux + x.AvarageUse) / 2;
                }
                tableStats.Add(new TableStats(id.ToString(), averageAux));
            }

            // Mostramos los resultados en las graficas
            for (int i = 0; i < tableStats.Count; ++i)
            {
                Entry entry = new Entry((float)tableStats[i].Average)
                {
                    Color = SKColor.Parse(colors[i % 7]),
                    Label = tableStats[i].Name,
                    ValueLabel = tableStats[i].Average.ToString()
                };
                entriesTableUseAverage.Add(entry);
            }

            chart5 = new BarChart() { Entries = entriesTableUseAverage };
            chartView5.Chart = chart5;
        }

        private void fillSurveys()
        {
            entriesAlexa.Clear();

            List<Surveys> auxSurveys = new List<Surveys>();

            auxSurveys = surveys.Where(d => d.DateStatistics >= sInterval1 && d.DateStatistics <= sInterval2).ToList();

            // Colocamos los valores
            if (auxSurveys.Any())
            {
                double foodRating = auxSurveys.Average(a => a.FoodRating);
                double comfortRating = auxSurveys.Average(a => a.ComfortRating);
                double serviceRating = auxSurveys.Average(a => a.ServiceRating);

                // Mostramos los resultados en las graficas
                Entry entry;
                entry = new Entry((float)foodRating)
                {
                    Color = SKColor.Parse(colors[0]),
                    Label = "Comida",
                    ValueLabel = foodRating.ToString()
                };
                entriesAlexa.Add(entry);

                entry = new Entry((float)comfortRating)
                {
                    Color = SKColor.Parse(colors[1]),
                    Label = "Comodidad",
                    ValueLabel = comfortRating.ToString()
                };
                entriesAlexa.Add(entry);

                entry = new Entry((float)serviceRating)
                {
                    Color = SKColor.Parse(colors[2]),
                    Label = "Servicio",
                    ValueLabel = serviceRating.ToString()
                };
                entriesAlexa.Add(entry);

                chart6 = new BarChart() { Entries = entriesAlexa };
                chartView6.Chart = chart6;
            } else
            {
                chart6 = new BarChart() { Entries = entriesAlexa };
                chartView6.Chart = chart6;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.activity_dashboard_statistics, container, false);

            chartView1 = view.FindViewById<ChartView>(Resource.Id.idChart1);
            chartView2 = view.FindViewById<ChartView>(Resource.Id.idChart2);
            chartView3 = view.FindViewById<ChartView>(Resource.Id.idChart3);
            chartView4 = view.FindViewById<ChartView>(Resource.Id.idChart4);
            chartView5 = view.FindViewById<ChartView>(Resource.Id.idChart5);
            chartView6 = view.FindViewById<ChartView>(Resource.Id.idChart6);

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

            InitializeIntervals();
            ShowIntervals();

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

            // Fill all charts
            fillHours();
            fillDays();
            fillTableHours();
            fillTableDays();
            fillWaiters();
            fillSurveys();

            return view;
        }

        private void InitializeIntervals()
        {
            hInterval1 = hourStatistics.Min(d => d.DateStatistics);
            hInterval2 = hourStatistics.Max(d => d.DateStatistics);
            dInterval1 = dayStatistics.Min(d => d.DateStatistics);
            dInterval2 = dayStatistics.Max(d => d.DateStatistics);
            thourInterval1 = hourStatistics.Min(d => d.DateStatistics);
            thourInterval2 = hourStatistics.Max(d => d.DateStatistics);
            tdayInterval1 = hourStatistics.Min(d => d.DateStatistics);
            tdayInterval2 = hourStatistics.Max(d => d.DateStatistics);
            wInterval1 = waiters.Min(d => d.DateStatistics);
            wInterval2 = waiters.Max(d => d.DateStatistics);
            sInterval1 = surveys.Min(d => d.DateStatistics);
            sInterval2 = surveys.Max(d => d.DateStatistics);
        }

        private void ShowIntervals()
        {
            hourInterval1.Text = hInterval1.ToString("dd/MM/yyyy");
            hourInterval2.Text = hInterval2.ToString("dd/MM/yyyy");
            daysInterval1.Text = dInterval1.ToString("dd/MM/yyyy");
            daysInterval2.Text = dInterval2.ToString("dd/MM/yyyy");
            tableUseInterval1.Text = thourInterval1.ToString("dd/MM/yyyy");
            tableUseInterval2.Text = thourInterval2.ToString("dd/MM/yyyy");
            tableAverageInterval1.Text = tdayInterval1.ToString("dd/MM/yyyy");
            tableAverageInterval2.Text = tdayInterval2.ToString("dd/MM/yyyy");
            waitersInterval1.Text = wInterval1.ToString("dd/MM/yyyy");
            waitersInterval2.Text = wInterval2.ToString("dd/MM/yyyy");
            alexaInterval1.Text = sInterval1.ToString("dd/MM/yyyy");
            alexaInterval2.Text = sInterval2.ToString("dd/MM/yyyy");
        }

        private void onHourInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval1.Text = time.ToString("dd/MM/yyyy");
                hInterval1 = time;
                fillHours();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onHourInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                hourInterval2.Text = time.ToString("dd/MM/yyyy");
                hInterval2 = time;
                fillHours();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableUseInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableUseInterval1.Text = time.ToString("dd/MM/yyyy");
                thourInterval1 = time;
                fillTableHours();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableUseInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableUseInterval2.Text = time.ToString("dd/MM/yyyy");
                thourInterval2 = time;
                fillTableHours();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onDaysInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                daysInterval1.Text = time.ToString("dd/MM/yyyy");
                dInterval1 = time;
                fillDays();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onDaysInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                daysInterval2.Text = time.ToString("dd/MM/yyyy");
                dInterval2 = time;
                fillDays();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onWaitersInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                waitersInterval1.Text = time.ToString("dd/MM/yyyy");
                wInterval1 = time;
                fillWaiters();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onWaitersInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                waitersInterval2.Text = time.ToString("dd/MM/yyyy");
                wInterval2 = time;
                fillWaiters();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableAverageInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableAverageInterval1.Text = time.ToString("dd/MM/yyyy");
                tdayInterval1 = time;
                fillTableDays();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onTableAverageInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tableAverageInterval2.Text = time.ToString("dd/MM/yyyy");
                tdayInterval2 = time;
                fillTableDays();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onAlexaInterval1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                alexaInterval1.Text = time.ToString("dd/MM/yyyy");
                sInterval1 = time;
                fillSurveys();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void onAlexaInterval2(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                alexaInterval2.Text = time.ToString("dd/MM/yyyy");
                sInterval2 = time;
                fillSurveys();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
    }
}
