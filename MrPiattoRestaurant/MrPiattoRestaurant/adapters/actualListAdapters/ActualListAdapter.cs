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

using MrPiattoRestaurant.Models.Reservations;

namespace MrPiattoRestaurant.adapters.actualListAdapters
{
    public class ActualListAdapter : RecyclerView.Adapter
    {
        public delegate void EndFoodEventHandler(int position);
        public event EndFoodEventHandler EndFood;
        protected virtual void OnTablePressed(int position)
        {
            if (EndFood != null)
            {
                EndFood(position);
            }
        }

        public List<Table> ocupiedTables;
        public Context context;
        public ActualListAdapter(Context context, List<Table> ocupiedTables)
        {
            this.context = context;
            this.ocupiedTables = ocupiedTables;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_actualList, parent, false);
            ActualListViewHolder vh = new ActualListViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            holder.IsRecyclable = false;
            ActualListViewHolder vh = holder as ActualListViewHolder;
            vh.personName.Text = ocupiedTables.ElementAt(position).actualClient.name;
            vh.tableName.Text = ocupiedTables.ElementAt(position).TableName;
            vh.timeUsed.Text = ocupiedTables.ElementAt(position).actualClient.timeUsed.ToString();

            vh.menu.Click += (s, arg) =>
            {
                Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(Application.Context, vh.menu);
                menu.Inflate(Resource.Menu.actualListMenu);

                menu.MenuItemClick += (s1, arg1) =>
                {
                    switch (arg1.Item.ItemId)
                    {
                        case Resource.Id.item1:
                            if (ocupiedTables[position].isJoined)
                            {
                                // Eliminara la mesa unida
                                ocupiedTables[position].UnJoined();

                                // Debemos de actualizar el recycler, eliminando a este cliente
                                EndFood(position);
                            } else
                            {
                                ocupiedTables[position].setOcupied(false);
                                ocupiedTables.Remove(ocupiedTables[position]);
                                EndFood(position);
                            }
                            AnswerSurveyDialog();
                            break;
                        case Resource.Id.item2:
                            Toast.MakeText(Application.Context, "Position: " + position + " Opcion 2", ToastLength.Short).Show();
                            break;
                    }
                };

                menu.Show();
            };
        }

        public override int ItemCount
        {
            get { return ocupiedTables.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public void AnswerSurveyDialog()
        {
            LayoutInflater inflater = LayoutInflater.From(Application.Context);
            View content = inflater.Inflate(Resource.Layout.dialog_answer_survey, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();
        }
        public class ActualListViewHolder : RecyclerView.ViewHolder
        {
            public TextView personName, tableName, timeUsed, menu;

            public ActualListViewHolder (View itemView) : base (itemView)
            {
                personName = itemView.FindViewById<TextView>(Resource.Id.idPersonName);
                tableName = itemView.FindViewById<TextView>(Resource.Id.idtableName);
                timeUsed = itemView.FindViewById<TextView>(Resource.Id.idTimeUsed);
                menu = itemView.FindViewById<TextView>(Resource.Id.idViewOptions);
            }
        }
    }
}