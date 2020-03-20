using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using MrPiattoRestaurant.Models;

namespace MrPiattoRestaurant.adapters
{
    public class PhotosGalleryAdapter : RecyclerView.Adapter
    {
        List<ItemPhotos> itemPhotos = new List<ItemPhotos>();

        public PhotosGalleryAdapter(List<ItemPhotos> itemPhotos)
        {
            this.itemPhotos = itemPhotos;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.layout_item_gallery, parent, false);
            PhotosGalleryViewHolder vh = new PhotosGalleryViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            holder.IsRecyclable = false;
            PhotosGalleryViewHolder vh = holder as PhotosGalleryViewHolder;

            // Load the photo image resource from the photo album:
            vh.image.SetImageResource(itemPhotos[position].imageID);

            // Load the photo caption from the photo album:
            vh.imageName.Text = itemPhotos[position].imageText;
        }

        public override int ItemCount
        {
            get { return itemPhotos.Count; }
        }

        public class PhotosGalleryViewHolder : RecyclerView.ViewHolder
        {
            public TextView imageName;
            public ImageView image;
            public Button button;

            public PhotosGalleryViewHolder(View itemView) : base(itemView)
            {
                imageName = itemView.FindViewById<TextView>(Resource.Id.idImageName);
                image = itemView.FindViewById<ImageView>(Resource.Id.idImage);
                button = itemView.FindViewById<Button>(Resource.Id.idButton);
            }
        }
    }
}