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
        private static readonly string url = "http://200.23.157.109/api/";
        //private static readonly string url = "http://10.0.2.2/api/";

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

        public async Task<string> UpdatePolicies(Policies policies)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(policies);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("ResInfo/Policies", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateWaiters(Waiters waiter)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(waiter);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("ResInfo/Waiters/Update", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddWaiter(Waiters waiter)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(waiter);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("ResInfo/Waiters/Add", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateSchedule(Schedule schedule)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(schedule);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Schedules/Update", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddTable(RestaurantTables table)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(table);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Grid", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateTable(RestaurantTables table)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(table);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Grid/UpdateTable", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddAuxiliarTable(AuxiliarTables table)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(table);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Grid/Aux", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddAuxReservation(AuxiliarReservation reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/Aux", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddManReservation(ManualReservations reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/ManRes", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateReservation(Reservation reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/Res/Update", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateAuxReservation(AuxiliarReservation reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/Aux/Update", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateManReservation(ManualReservations reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/ManRes/Update", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddDayStatistics(DayStatistics dayStats)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(dayStats);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Statistics/Add/Day", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddHourStatistics(HourStatistics hourStats)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(hourStats);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Statistics/Add/Hour", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddTableStatistics(TableStatistics tableStats)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(tableStats);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Statistics/Add/TableStats", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateReservation2(Reservation reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/Res/UpdateFromNot", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateAuxReservation2(AuxiliarReservation reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/Aux/UpdateFromNot", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateManReservation2(ManualReservations reservation)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{url}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(reservation);
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Reservations/Not/ManRes/UpdateFromNot", byteContent).Result;
            return await response.Content.ReadAsStringAsync();
        }
    }
}