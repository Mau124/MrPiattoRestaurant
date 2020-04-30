using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.ModelsDB;

namespace MrPiattoRestaurant.Resources.utilities
{
    public class APIUpdate
    {
        //private static readonly string url = "http://200.23.157.109/api/";
        private static readonly string url = "http://10.0.2.2/api/";

        public APIUpdate() { }

        public async Task<string> UpdateRestaurant(Restaurant restaurant)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(restaurant);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("ResInfo", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }
    }
}