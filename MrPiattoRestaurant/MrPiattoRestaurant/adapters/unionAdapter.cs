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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MrPiattoRestaurant.adapters
{
    public class UnionAdapter : RecyclerView.Adapter
    {
        private Context context;
        public List<Table> tables;
        public UnionAdapter(Context context, List<Table> tables)
        {
            this.context = context;
            this.tables = tables;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_union, parent, false);
            UnionViewHolder vh = new UnionViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            UnionViewHolder vh = holder as UnionViewHolder;

            vh.tableName.Text = tables[position].TableName;
            vh.tableSeats.Text = tables[position].seats.ToString();

            Drawable auxImg = tables[position].image.GetConstantState().NewDrawable().Mutate();
            vh.tableImg.SetBackgroundDrawable(auxImg);
        }

        public override int ItemCount
        {
            get { return tables.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public class UnionViewHolder : RecyclerView.ViewHolder
        {
            public TextView tableName, tableSeats;
            public ImageView tableImg;

            public UnionViewHolder(View itemView) : base(itemView)
            {
                tableName = itemView.FindViewById<TextView>(Resource.Id.idtableName);
                tableSeats = itemView.FindViewById<TextView>(Resource.Id.idtableSeats);
                tableImg = itemView.FindViewById<ImageView>(Resource.Id.idImageTable);
            }
        }
    }
}