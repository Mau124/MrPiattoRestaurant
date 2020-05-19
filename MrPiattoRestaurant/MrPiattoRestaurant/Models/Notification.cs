﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MrPiattoRestaurant.Models
{
    public class Notification
    {
        public Notification(string Name, string LastName, string TableName, DateTime Date, string Phone)
        {
            type = -1;
            this.Name = Name;
            this.LastName = LastName;
            this.TableName = TableName;
            this.Date = Date;
            this.Phone = Phone;
        }

        public Notification(string Name, string LastName, string TableName, DateTime Date, string Phone, int floorIndex, int tableIndex)
        {
            type = 0;
            this.Name = Name;
            this.LastName = LastName;
            this.TableName = TableName;
            this.Date = Date;
            this.Phone = Phone;
            this.floorIndex = floorIndex;
            this.tableIndex = tableIndex;
        }

        public Notification(string Name, string LastName, string TableName, DateTime Date, string Phone, int floorIndex, string tableString)
        {
            type = 1;
            this.Name = Name;
            this.LastName = LastName;
            this.TableName = TableName;
            this.Date = Date;
            this.Phone = Phone;
            this.floorIndex = floorIndex;
            this.tableString = tableString;
        }

        // Type of notificarion:
        // -1: System
        //  0: Manual
        //  1: Auxiliar
        public int type { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string TableName { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        public int floorIndex { get; set; }
        public int tableIndex { get; set; }
        public int seats { get; set; }
        public string tableString { get; set; }
    }
}