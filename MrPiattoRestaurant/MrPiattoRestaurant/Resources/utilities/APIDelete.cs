using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MrPiattoRestaurant.ModelsDB;
using Newtonsoft.Json;

namespace MrPiattoRestaurant.Resources.utilities
{
    public class APIDelete
    {
        private static readonly string url = "http://200.23.157.109/api/";
        //private static readonly string url = "http://10.0.2.2/api/";

        public APIDelete() { }

        public async Task<string> DeleteWaiter(Waiters waiter)
        {
            string result;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}ResInfo/Delete/Waiters/{waiter.Idwaiter}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<string>(json);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> DeleteTable(int tableID)
        {
            string result;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}Grid/Delete/Tables/{tableID}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<string>(json);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> DeletePhotos(RestaurantPhotos idPhoto)
        {
            string result;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}Photos/Delete/Photos/{idPhoto}");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<string>(json);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}