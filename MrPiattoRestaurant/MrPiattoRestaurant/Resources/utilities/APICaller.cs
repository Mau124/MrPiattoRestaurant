﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.ModelsDB;
using Newtonsoft.Json;

namespace MrPiattoRestaurant.Resources.utilities
{
    public class APICaller
    {
        //private static readonly string url = "http://200.23.157.109/api/";
        private static readonly string url = "http://10.0.2.2/api/";

        public APICaller() { }

        public Restaurant GetRestaurant(int idRestaurant)
        {
            Restaurant restaurant;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}ResInfo/{idRestaurant}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    restaurant = JsonConvert.DeserializeObject<Restaurant>(json);
                }

                return restaurant;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<RestaurantTables> GetTables(int idRestaurant)
        {
            List<RestaurantTables> resTables;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}Grid/{idRestaurant}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    resTables = JsonConvert.DeserializeObject<List<RestaurantTables>>(json);
                }

                return resTables;
            } catch(Exception)
            {
                return null;
            }
        }
        public string GetUserName(int idUser)
        {
            string userName;
            User user;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}ClientRes/{idUser}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    user = JsonConvert.DeserializeObject<User>(json);
                }

                userName = user.FirstName + user.LastName;
                return userName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<int, string> GetFloors(int idRes)
        {
            Dictionary<int, string> floors = new Dictionary<int, string>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}Grid/Floors/{idRes}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    floors = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                }

                return floors;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Policies GetPolicies(int idRes)
        {
            Policies policies = new Policies();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}ResInfo/Policies/{idRes}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    policies = JsonConvert.DeserializeObject<Policies>(json);
                }

                return policies;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}