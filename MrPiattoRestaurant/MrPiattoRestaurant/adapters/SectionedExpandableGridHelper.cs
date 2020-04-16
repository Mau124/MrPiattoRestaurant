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

        public delegate void ItemPressedEventHandler(string type, int seats);
        public event ItemPressedEventHandler ItemPressed;

        protected virtual void OnItemPressed(string type, int seats)
        {
            if (ItemPressed != null)
            {
                ItemPressed(type, seats);
            }
        }
        public SectionedExpandableGridHelper(Context context, RecyclerView recyclerView, int gridSpanCount)
        {
            Item c;
            Item i;

            c = new Item("Mesas cuadradas");
            actualItems.Add(c);

            i = new Item("Mesa 1", Resource.Drawable.s1, "s", 1);
            actualItems.Add(i);

            i = new Item("Mesa 2", Resource.Drawable.s2, "s", 2);
            actualItems.Add(i);

            i = new Item("Mesa 3", Resource.Drawable.s3, "s", 3);
            actualItems.Add(i);

            i = new Item("Mesa 4", Resource.Drawable.s4, "s", 4);
            actualItems.Add(i);

            i = new Item("Mesa 5", Resource.Drawable.s5, "s", 5);
            actualItems.Add(i);

            i = new Item("Mesa 6", Resource.Drawable.s6, "s", 6);
            actualItems.Add(i);

            i = new Item("Mesa 7", Resource.Drawable.s7, "s", 7);
            actualItems.Add(i);

            i = new Item("Mesa 8", Resource.Drawable.s8, "s", 8);
            actualItems.Add(i);

            i = new Item("Mesa 9", Resource.Drawable.s9, "s", 9);
            actualItems.Add(i);

            i = new Item("Mesa 10", Resource.Drawable.s10, "s", 10);
            actualItems.Add(i);

            i = new Item("Mesa 11", Resource.Drawable.s11, "s", 11);
            actualItems.Add(i);

            i = new Item("Mesa 12", Resource.Drawable.s12, "s", 12);
            actualItems.Add(i);

            c = new Item("Mesas circulares");
            actualItems.Add(c);

            i = new Item("Mesa 1", Resource.Drawable.c1, "c", 1);
            actualItems.Add(i);

            i = new Item("Mesa 2", Resource.Drawable.c2, "c", 2);
            actualItems.Add(i);

            i = new Item("Mesa 3", Resource.Drawable.c3, "c", 3);
            actualItems.Add(i);

            i = new Item("Mesa 4", Resource.Drawable.c4, "c", 4);
            actualItems.Add(i);

            i = new Item("Mesa 5", Resource.Drawable.c5, "c", 5);
            actualItems.Add(i);

            i = new Item("Mesa 6", Resource.Drawable.c6, "c", 6);
            actualItems.Add(i);

            i = new Item("Mesa 7", Resource.Drawable.c7, "c", 7);
            actualItems.Add(i);

            i = new Item("Mesa 8", Resource.Drawable.c8, "c", 8);
            actualItems.Add(i);

            i = new Item("Mesa 9", Resource.Drawable.c9, "c", 9);
            actualItems.Add(i);

            i = new Item("Mesa 10", Resource.Drawable.c10, "c", 10);
            actualItems.Add(i);

            i = new Item("Mesa 11", Resource.Drawable.c11, "c", 11);
            actualItems.Add(i);

            i = new Item("Mesa 12", Resource.Drawable.c12, "c", 12);
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
                OnItemPressed(adapterItems.ElementAt(position).type, adapterItems.ElementAt(position).seats);
            }
        }
    }
}