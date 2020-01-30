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
using Android.Graphics.Drawables;

using MrPiattoRestaurant.models;

namespace MrPiattoRestaurant.adapters
{
    public class SectionedExpandableGridHelper
    {
        public List<Item> actualItems = new List<Item>();
        public List<Item> adapterItems = new List<Item>();
        public RecyclerView mRecyclerView;
        public SectionedExpandableGridAdapter mSectionedExpandableGridAdapter;

        public delegate void ItemPressedEventHandler(object source, int idTable);
        public event ItemPressedEventHandler ItemPressed;

        protected virtual void OnItemPressed(int idTable)
        {
            if (ItemPressed != null)
            {
                ItemPressed(this, idTable);
            }
        }
        public SectionedExpandableGridHelper(Context context, RecyclerView recyclerView, int gridSpanCount)
        {
            Item c = new Item("Texto");
            Item i = new Item("Texto", Resource.Drawable.table);

            actualItems.Add(c);
            actualItems.Add(i);

            c = new Item("Mesas cuadradas");
            actualItems.Add(c);

            i = new Item("Mesa 1", Resource.Drawable.c1);
            actualItems.Add(i);

            i = new Item("Mesa 2", Resource.Drawable.c2);
            actualItems.Add(i);

            i = new Item("Mesa 3", Resource.Drawable.c3);
            actualItems.Add(i);

            i = new Item("Mesa 4", Resource.Drawable.c4);
            actualItems.Add(i);

            i = new Item("Mesa 5", Resource.Drawable.c5);
            actualItems.Add(i);

            c = new Item("Mesas circulares");
            actualItems.Add(c);

            i = new Item("Mesa 1", Resource.Drawable.c1);
            actualItems.Add(i);

            i = new Item("Mesa 2", Resource.Drawable.c2);
            actualItems.Add(i);

            i = new Item("Mesa 3", Resource.Drawable.c3);
            actualItems.Add(i);

            i = new Item("Mesa 4", Resource.Drawable.c4);
            actualItems.Add(i);

            i = new Item("Mesa 5", Resource.Drawable.c5);
            actualItems.Add(i);

            adapterItems = new List<Item>(actualItems);

            GridLayoutManager gridLayoutManager = new GridLayoutManager(context, gridSpanCount);
            recyclerView.SetLayoutManager(gridLayoutManager);
            mSectionedExpandableGridAdapter = new SectionedExpandableGridAdapter(adapterItems, gridLayoutManager);
            recyclerView.SetAdapter(mSectionedExpandableGridAdapter);

            mRecyclerView = recyclerView;

            mSectionedExpandableGridAdapter.ItemClick += OnItemClick;
        }

        // Handler for the item click event:
        void OnItemClick(object sender, int position)
        {
            // Display a toast that briefly shows the enumeration of the selected photo:
            if (adapterItems.ElementAt(position).isSection)
            {
                if (adapterItems.ElementAt(position).isExpandable)
                    adapterItems.ElementAt(position).isExpandable = false;
                else
                    adapterItems.ElementAt(position).isExpandable = true;

                bool expandableItems = true;
                adapterItems.Clear();

                foreach (Item item in actualItems.ToList())
                {
                    if (item.isSection)
                    {
                        adapterItems.Add(item);
                        if (item.isExpandable)
                        {
                            expandableItems = true;
                        }
                        else
                        {
                            expandableItems = false;
                        }
                    }
                    else
                    {
                        if (expandableItems)
                            adapterItems.Add(item);
                    }
                }
                mSectionedExpandableGridAdapter.NotifyDataSetChanged();
            } 
            else
            {
                OnItemPressed(adapterItems.ElementAt(position).tableID);
            }
        }
    }
}