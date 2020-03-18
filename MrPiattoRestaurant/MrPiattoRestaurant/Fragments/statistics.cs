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

namespace MrPiattoRestaurant.Fragments
{
    public class Statistics : Android.Support.V4.App.Fragment
    {
        List<Entry> entriesDays, entriesHours;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
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

            var chart1 = new LineChart() { Entries = entriesHours };
            var chart2 = new BarChart() { Entries = entriesDays };
            var chart3 = new DonutChart() { Entries = entriesDays };
            var chart4 = new RadialGaugeChart() { Entries = entriesDays };
            var chart5 = new BarChart() { Entries = entriesDays };
            var chart6 = new BarChart() { Entries = entriesDays };

            chartView1.Chart = chart1;
            chartView2.Chart = chart2;
            chartView3.Chart = chart3;
            chartView4.Chart = chart4;
            chartView5.Chart = chart5;
            chartView6.Chart = chart6;

            return view;
        }
    }
}