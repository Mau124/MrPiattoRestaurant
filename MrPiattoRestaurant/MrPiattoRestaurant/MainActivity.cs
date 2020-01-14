using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace MrPiattoRestaurant
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public RelativeLayout container;
        public Button newFloor;
        public Spinner floorName;
        public List<View> floors = new List<View>();
        public List<string> floorsNames = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            container = FindViewById<RelativeLayout>(Resource.Id.container);
            newFloor = FindViewById<Button>(Resource.Id.newFloor);
            floorName = FindViewById<Spinner>(Resource.Id.floorName);

            //We create the first floor and add it to the list of floors
            View floor = new GestureRecognizerView(this, "Piso 1");
            floors.Add(floor);
            floorsNames.Add("Piso 1");

            //We pass the floor to the container
            container.AddView(floors[0]);

            //Event to create another floor
            newFloor.Click += delegate { addFloor(); };

            //Create events for the spinner
            floorName.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            var adapter = new ArrayAdapter<string>(this,
                Android.Resource.Layout.SimpleSpinnerItem, floorsNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            floorName.Adapter = adapter;
        }
        public void addFloor()
        {
            View floor = new GestureRecognizerView(this, "Nuevo Piso");
            floors.Add(floor);
            floorsNames.Add("Nuevo Piso");

            //We clean the view and add the new floor
            container.RemoveAllViews();
            container.AddView(floors[floors.Count - 1]);

            //Update Spinner and select last floor
            var adapter = new ArrayAdapter<string>(this,
               Android.Resource.Layout.SimpleSpinnerItem, floorsNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            floorName.Adapter = adapter;

            floorName.SetSelection(floorsNames.Count - 1);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs ev)
        {
            Spinner spinner = (Spinner)sender;
            int nFloor = (int) spinner.GetItemIdAtPosition(ev.Position);

            container.RemoveAllViews();
            container.AddView(floors[nFloor]);

            Toast.MakeText(this, nFloor.ToString(), ToastLength.Long).Show();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}