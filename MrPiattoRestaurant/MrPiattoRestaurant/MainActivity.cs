using System.Collections.Generic;
using System;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using Android.Support.Design.Widget;

using MrPiattoRestaurant.adapters;
using MrPiattoRestaurant.Fragments.Reservations;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.InteractiveViews;
using MrPiattoRestaurant.Models.Reservations;
using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.Views;

namespace MrPiattoRestaurant
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public RelativeLayout container;
        public LinearLayout options;
        public Button newFloor, modifyFloor;
        public ImageButton dashboard;
        public Spinner floorName;
        public List<GestureRecognizerView> floors = new List<GestureRecognizerView>();
        public List<string> floorsNames = new List<string>();
        public View v;
        public TextView date, hour;
        public Android.Support.Constraints.ConstraintLayout timeLine;
        public ImageView Return;

        public FutureFragment futureFragment = new FutureFragment();
        public WaitFragment waitFragment;

        public BottomNavigationView bottomNavigation;

        public int floorIndex = new int();

        LayoutInflater inflater;
        View mainContainer;

        TimeLineView timeLineView;
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
            timeLine = FindViewById <Android.Support.Constraints.ConstraintLayout>(Resource.Id.idTimeLine);
            dashboard = FindViewById<ImageButton>(Resource.Id.idDashboard);
            Return = FindViewById<ImageView>(Resource.Id.idReturn);

            date = FindViewById<TextView>(Resource.Id.idDate);
            hour = FindViewById<TextView>(Resource.Id.idHour);

            waitFragment = new WaitFragment(this);

            Return.Click += ReturnActualTime;

            //We create the first floor and add it to the list of floors
            GestureRecognizerView floor = new GestureRecognizerView(this, "Piso 1", 0);
            floors.Add(floor);
            floorsNames.Add("Piso 1");

            floor.TablePressed += OnTablePressed;
            //We pass the floor to the container
            container.AddView(floors.ElementAt(floorIndex));

            timeLineView = new TimeLineView(this);
            timeLine.AddView(timeLineView);

            //Event to create another floor
            newFloor.Click += delegate { addFloor(); };

            modifyFloor.Click += delegate
            {
                catalogueTable();
            };

            options.RemoveAllViews();

            inflater = LayoutInflater.From(this);
            mainContainer = inflater.Inflate(Resource.Layout.layout_main_container, options, true);

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.idBottom_navigation);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
            LoadFragment(Resource.Id.idActualList);


            //Create events for the spinner
            floorName.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            var adapter = new ArrayAdapter<string>(this,
                Android.Resource.Layout.SimpleSpinnerItem, floorsNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            floorName.Adapter = adapter;

            date.Click += DateSelect_OnClick;
            hour.Click += HourSelect_OnClick;

            dashboard.Click += OpenDashboard;

            waitFragment.AddClient += AddWaitClient;

            floor.Drag += OnDrag;

            timeLineView.TimeLinePressed += UpdateTime;
        }
        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }
        public void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.idActualList:
                    fragment = new ActualFragment(this, floors.ElementAt(floorIndex).ocupiedTables);
                    break;
                case Resource.Id.idFutureList:
                    fragment = futureFragment;
                    break;
                case Resource.Id.idWaitList:
                    fragment = waitFragment;
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.idContent_frame, fragment)
                .Commit();
        }
        //Views
        /*
         Here are all the views and its methods
        */
        public void OnTablePressed(int floorIndex, int tableIndex)
        {
            options.RemoveAllViews();
            TablePropertiesView tablepropertiesView = new TablePropertiesView(this, floors, floorIndex, tableIndex);
            tablepropertiesView.CreateView(options);
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
                auxFloor.Drag += OnDrag;
                floors.Add(auxFloor);
                floorsNames.Add(s);
                container.RemoveAllViews();
                container.AddView(auxFloor);

                var adapter = new ArrayAdapter<string>(this,
                    Android.Resource.Layout.SimpleSpinnerItem, floorsNames);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                floorName.Adapter = adapter;
                floorName.SetSelection(floorsNames.Count() - 1);

                catalogueTable();
            };

            //We create the dialog window
        }

        public void catalogueTable()
        {
            RecyclerView mRecyclerView;
            SectionedExpandableGridHelper mSectionedExpandableHelper;

            options.RemoveAllViews();

            LayoutInflater inflater = LayoutInflater.From(this);
            View catalogueTables = inflater.Inflate(Resource.Layout.layout_catalogue, options, true);

            ImageView cancel = catalogueTables.FindViewById<ImageView>(Resource.Id.idCancel);

            mRecyclerView = catalogueTables.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);
            mSectionedExpandableHelper = new SectionedExpandableGridHelper(this, mRecyclerView, 3);

            mSectionedExpandableHelper.ItemPressed += OnItemPressed;
            cancel.Click += delegate { OnCancel(catalogueTables); };
        }

        public void OnItemPressed(string type, int seats)
        {
            floors.ElementAt(floorIndex).AddTable(type, seats);
        }

        public void OnCancel(View view)
        {
            options.RemoveAllViews();

            inflater = LayoutInflater.From(this);
            mainContainer = inflater.Inflate(Resource.Layout.layout_main_container, options, true);

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.idBottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
            LoadFragment(Resource.Id.idActualList);
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

        public void DateSelect_OnClick(object sender, EventArgs args)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                date.Text = time.ToLongDateString();
            });
            frag.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }

        public void HourSelect_OnClick(object sender, EventArgs args)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                timeLineView.SetTime(time.Hour, time.Minute);

                hour.Text = time.ToString("hh:mm tt");
            });
            frag.Show(SupportFragmentManager, TimePickerFragment.TAG);
        }

        private void ReturnActualTime(object sender, EventArgs args)
        {
            int hours = DateTime.Now.Hour;
            int minutes = DateTime.Now.Minute;

            timeLineView.SetTime(hours, minutes);
        }

        public void OpenDashboard(object sender, EventArgs args)
        {
            Intent dashboard = new Intent(this, typeof(DashboardActivity));
            StartActivity(dashboard);
        }

        public void AddWaitClient(object sender, EventArgs args)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.add_waitList, null);
            Button add, cancel;
            EditText nameClient, numSeats;

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(this).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            add = content.FindViewById<Button>(Resource.Id.idAdd);
            cancel = content.FindViewById<Button>(Resource.Id.idCancel);

            nameClient = content.FindViewById<EditText>(Resource.Id.idName);
            numSeats = content.FindViewById<EditText>(Resource.Id.idSeats);

            add.Click += (s, a) => {
                string name;
                int seats;

                name = nameClient.Text;
                seats = Int32.Parse(numSeats.Text);

                waitFragment.AddToList(name, seats);
                alertDialog.Dismiss();
            };
        }

        void OnDrag(object sender, View.DragEventArgs e)
        {
            // React on different dragging events
            var evt = e.Event;
            switch (evt.Action)
            {
                case DragAction.Ended:
                case DragAction.Started:
                    e.Handled = true;
                    break;

                // Dragged element enters the drop zone
                case DragAction.Entered:
                    break;

                // Dragged element exits the drop zone
                case DragAction.Exited:
                    break;

                // Dragged element has been dropped at the drop zone
                case DragAction.Drop:
                    float x = evt.GetX();
                    float y = evt.GetY();

                    // You can check if element may be dropped here
                    // If not do not set e.Handled to true
                    e.Handled = true;
                    int table = floors.ElementAt(floorIndex).IsOnTable(x, y);
                    if (table != -1)
                    {
                        floors.ElementAt(floorIndex).tables.ElementAt(table).setOcupied();
                        floors.ElementAt(floorIndex).Draw();


                        int pos = waitFragment.getDraggedItemPosition();
                        //Create alertdialog
                        View content = LayoutInflater.Inflate(Resource.Layout.dialog_confirm_assigntable_fromWait, null);

                        TextView name, seats;

                        name = content.FindViewById<TextView>(Resource.Id.idName);
                        seats = content.FindViewById<TextView>(Resource.Id.idSeats);

                        name.Text = waitFragment.waitList.ElementAt(pos).personName;
                        seats.Text = waitFragment.waitList.ElementAt(pos).numSeats.ToString();

                        Client client = new Client(name.Text, 0, DateTime.Now);
                        floors.ElementAt(floorIndex).setActualClientOnTable(client, table);
                        waitFragment.RemoveFromWaitList(pos);

                        bottomNavigation.SelectedItemId = Resource.Id.idActualList;
                        LoadFragment(Resource.Id.idActualList);

                        Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(this).Create();
                        alertDialog.SetCancelable(true);
                        alertDialog.SetView(content);
                        alertDialog.Show();
                        

                        Toast.MakeText(Application.Context, "Posicion " + pos, ToastLength.Short).Show();
                    } 
                    else
                    {
                        Toast.MakeText(Application.Context, "No esta sobre una mesa", ToastLength.Short).Show();
                    }
                    // Try to get clip data
                    //Toast.MakeText(Application.Context, "Coordenadas; x: " + x + " y: " + y, ToastLength.Short).Show();
                    break;
            }
        }

        public void UpdateTime(int hours, int minutes)
        {
            hour.Text = hours.ToString("00") + ":" + minutes.ToString("00");
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}