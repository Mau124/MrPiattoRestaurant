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
using MrPiattoRestaurant.adapters;

namespace MrPiattoRestaurant.Fragments
{
    public class PhotosGallery : Android.Support.V4.App.Fragment
    {
        List<ItemPhotos> itemPhotos = new List<ItemPhotos>();
        Context context;

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        PhotosGalleryAdapter mAdapter;

        public PhotosGallery(Context context)
        {
            this.context = context;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            itemPhotos.Clear();
            itemPhotos.Add(new ItemPhotos(Resource.Drawable.res, "Banner"));
            itemPhotos.Add(new ItemPhotos(Resource.Drawable.res, "Imagen 2"));
            itemPhotos.Add(new ItemPhotos(Resource.Drawable.res, "Imagen 3"));
            itemPhotos.Add(new ItemPhotos(Resource.Drawable.res, "Imagen 4"));
            itemPhotos.Add(new ItemPhotos(Resource.Drawable.res, "Imagen 5"));
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