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
using Android.Support.V7.Widget;

using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.adapters;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.Fragments
{
    public class PhotosGallery : Android.Support.V4.App.Fragment
    {
        private Context context;
        private APICaller API = new APICaller();
        private List<RestaurantPhotos> itemPhotos = new List<RestaurantPhotos>();
        private Restaurant restaurant = new Restaurant();
        

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        PhotosGalleryAdapter mAdapter;

        public PhotosGallery(Context context, Restaurant restaurant)
        {
            this.context = context;
            this.restaurant = restaurant;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            itemPhotos.Clear();
            itemPhotos = API.GetPhotos(restaurant.Idrestaurant);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.activity_dashboard_gallery, container, false);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            mLayoutManager = new GridLayoutManager(context, 3, GridLayoutManager.Vertical, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new PhotosGalleryAdapter(itemPhotos);
            mRecyclerView.SetAdapter(mAdapter);

            return view;
        }
    }
}