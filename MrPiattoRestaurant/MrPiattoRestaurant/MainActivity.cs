using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MrPiattoRestaurant
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public RelativeLayout container;
        public LinearLayout options;
        public Button newFloor, modifyFloor;
        public Spinner floorName;
        public List<GestureRecognizerView> floors = new List<GestureRecognizerView>();
        public List<string> floorsNames = new List<string>();
        public View v;

        public int floorIndex = new int();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            container = FindViewById<RelativeLayout>(Resource.Id.container);
            newFloor = FindViewById<Button>(Resource.Id.newFloor);
            modifyFloor = FindViewById<Button>(Resource.Id.idModify);
            floorName = FindViewById<Spinner>(Resource.Id.floorName);
            options = FindViewById<LinearLayout>(Resource.Id.idOptions);

            //We create the first floor and add it to the list of floors
            GestureRecognizerView floor = new GestureRecognizerView(this, "Piso 1", 0);
            floors.Add(floor);
            floorsNames.Add("Piso 1");

            floor.TablePressed += OnTablePressed;
            //We pass the floor to the container
            container.AddView(floors.ElementAt(floorIndex));

            //Event to create another floor
            newFloor.Click += delegate { addFloor(); };

            modifyFloor.Click += delegate
            {
                floors.ElementAt(floorIndex).AddTable(this.Resources.GetDrawable(Resource.Drawable.Table1), this);
            };

            //Create events for the spinner
            floorName.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            var adapter = new ArrayAdapter<string>(this,
                Android.Resource.Layout.SimpleSpinnerItem, floorsNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            floorName.Adapter = adapter;
        }

        public void OnTablePressed(object source, TablePressedEventArgs args)
        {
            options.RemoveAllViews();

            LayoutInflater inflater = LayoutInflater.From(this);
            View tablePropertiesView = inflater.Inflate(Resource.Layout.table_properties, options, true);

            TextView x = tablePropertiesView.FindViewById<TextView>(Resource.Id.idTableName);

            //We identify the buttons
            Button buttonSave = tablePropertiesView.FindViewById<Button>(Resource.Id.idSaveButton);
            Button buttonDelete = tablePropertiesView.FindViewById<Button>(Resource.Id.idDeleteButton);

            buttonSave.Click += delegate { SaveChanges(); };
            buttonDelete.Click += delegate { DeleteTable(args.floorIterator, args.tableIterator); };

            x.Text = "Mesa " + args.tableIterator;

            Table t = floors[args.floorIterator].getTableProperties(args.tableIterator);
        }

        public void SaveChanges()
        {

        }
        public void DeleteTable(int floorIterator, int tableIterator)
        {
            floors[floorIterator].DeleteTable(tableIterator);
            floors[floorIterator].Draw();
        }

        public void PressedButton()
        {
            options.RemoveViewAt(0);
            Toast.MakeText(this, "Funciona", ToastLength.Long).Show();
        }
        public void addFloor()
        {
            options.RemoveAllViews();
            LayoutInflater inflater = LayoutInflater.From(this);
            View addFloorView = inflater.Inflate(Resource.Layout.AddFloor, options, true);

            Button addFloor = addFloorView.FindViewById<Button>(Resource.Id.idAddButton);
            EditText afloorName = addFloorView.FindViewById<EditText>(Resource.Id.idFloorName);

            addFloor.Click += delegate {
                string s = afloorName.Text;
                GestureRecognizerView auxFloor = new GestureRecognizerView(this, s, floors.Count());
                auxFloor.TablePressed += OnTablePressed;
                floors.Add(auxFloor);
                floorsNames.Add(s);
                container.RemoveAllViews();
                container.AddView(auxFloor);

                var adapter = new ArrayAdapter<string>(this,
                    Android.Resource.Layout.SimpleSpinnerItem, floorsNames);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                floorName.Adapter = adapter;
                floorName.SetSelection(floorsNames.Count() - 1);
            };

            //We create the dialog window

        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs ev)
        {
            Spinner spinner = (Spinner)sender;
            int nFloor = (int) spinner.GetItemIdAtPosition(ev.Position);

            container.RemoveAllViews();
            container.AddView(floors.ElementAt(nFloor));

            floorIndex = nFloor;

            
            Toast.MakeText(this, nFloor.ToString(), ToastLength.Long).Show();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}