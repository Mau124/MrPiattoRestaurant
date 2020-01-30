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

using MrPiattoRestaurant.models;

namespace MrPiattoRestaurant.adapters
{
    public class SectionedExpandableGridAdapter : RecyclerView.Adapter
    {
        private const int VIEW_TYPE_SECTION = Resource.Layout.layout_section;
        private const int VIEW_TYPE_ITEM = Resource.Layout.layout_item;
        // Event handler for item clicks
        public event EventHandler<int> ItemClick;

        // Date set
        public List<Item> items;

        // Load the adapter with the dat set (table album) at construction time
        public SectionedExpandableGridAdapter(List<Item> items, GridLayoutManager gridLayoutManager)
        {
            this.items = items;
            gridLayoutManager.SetSpanSizeLookup(new MyGridLayoutManagerSpanSizeLookup(
                getSpanSizeFunc: position =>
                {
                    if (!items.ElementAt(position).isSection) return 1;
                    else return gridLayoutManager.SpanCount;
                }
                ));
        }

        //Create a new table cardview (Invoked by the layout manager)
        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the Cardview for the table
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(viewType, parent, false);

            // Create a ViewHolder to find and hold these view references, and
            // register OnClick with the view holder
            TableViewHolder vh = new TableViewHolder(itemView, viewType, OnClick);
            return vh;
        }

        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TableViewHolder vh = holder as TableViewHolder;


            switch (vh.viewType)
            {
                case VIEW_TYPE_ITEM:
                    //Load the photo image resource from the photo album
                    vh.Image.SetImageResource(items.ElementAt(position).tableID);

                    //Load the photo caption from the photo album
                    vh.Caption.Text = items.ElementAt(position).caption;
                    break;
                case VIEW_TYPE_SECTION:
                    vh.sectionTextView.Text = items.ElementAt(position).sectionName;

                    if (items.ElementAt(position).isExpandable)
                    {
                        vh.sectionToggleButton.SetImageResource(Resource.Drawable.ic_keyboard_arrow_up_white_24dp);
                    }
                    else
                    {
                        vh.sectionToggleButton.SetImageResource(Resource.Drawable.ic_keyboard_arrow_down_white_24dp);
                    }
                    break;
            }
        }

        public override int ItemCount
        {
            get { return items.Count(); }
        }
        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public override int GetItemViewType(int position)
        {
            if (!items.ElementAt(position).isSection)
                return VIEW_TYPE_ITEM;
            else
                return VIEW_TYPE_SECTION;
        }
        public class TableViewHolder : RecyclerView.ViewHolder
        {
            //Common
            public View itemView { get; set; }
            public int viewType { get; set; }

            //Section
            public TextView sectionTextView { get; set; }
            public ImageView sectionToggleButton { get; set; }

            //Items
            public ImageView Image { get; set; }
            public TextView Caption { get; set; }

            public TableViewHolder(View itemView, int viewType, Action<int> listener) : base(itemView)
            {
                //Locate and cache view references
                this.viewType = viewType;
                this.itemView = itemView;

                if (viewType == VIEW_TYPE_ITEM)
                {
                    Image = itemView.FindViewById<ImageView>(Resource.Id.image_item);
                    Caption = itemView.FindViewById<TextView>(Resource.Id.text_item);
                }
                else
                {
                    sectionTextView = itemView.FindViewById<TextView>(Resource.Id.text_section);
                    sectionToggleButton = itemView.FindViewById<ImageView>(Resource.Id.toggle_button_section);
                    //sectionToggleButton = itemView.FindViewById<ToggleButton>(Resource.Id.toggle_button_section);
                }

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
            }
        }

        public class MyGridLayoutManagerSpanSizeLookup : GridLayoutManager.SpanSizeLookup
        {
            private readonly Func<int, int> _getSpanSizeFunc;

            public MyGridLayoutManagerSpanSizeLookup(Func<int, int> getSpanSizeFunc)
            {
                _getSpanSizeFunc = getSpanSizeFunc;
            }

            public override int GetSpanSize(int position)
            {
                return _getSpanSizeFunc(position);
            }
        }
    }
}