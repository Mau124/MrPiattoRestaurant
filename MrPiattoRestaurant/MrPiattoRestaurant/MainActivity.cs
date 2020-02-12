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

using MrPiattoRestaurant.adapters;
using MrPiattoRestaurant.Fragments.Reservations;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.InteractiveViews;

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

        public ActualFragment actualFragment = new ActualFragment();
        public FutureFragment futureFragment = new FutureFragment();
        public WaitFragment waitFragment = new WaitFragment();

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
            timeLine = FindViewById <Android.Support.Constraints.ConstraintLayout>(Resource.Id.idTimeLine);
            dashboard = FindViewById<ImageButton>(Resource.Id.idDashboard);

            date = FindViewById<TextView>(Resource.Id.idDate);
            hour = FindViewById<TextView>(Resource.Id.idHour);

            //We create the first floor and add it to the list of floors
            GestureRecognizerView floor = new GestureRecognizerView(this, "Piso 1", 0);
            floors.Add(floor);
            floorsNames.Add("Piso 1");

            floor.TablePressed += OnTablePressed;
            //We pass the floor to the container
            container.AddView(floors.ElementAt(floorIndex));

            TimeLineView timeLineView = new TimeLineView(this);
            timeLine.AddView(timeLineView);

            //Event to create another floor
            newFloor.Click += delegate { addFloor(); };

            modifyFloor.Click += delegate
            {
                catalogueTable();
            };

            options.RemoveAllViews();

            LayoutInflater inflater = LayoutInflater.From(this);
            View mainContainer = inflater.Inflate(Resource.Layout.layout_main_container, options, true);

            ImageButton actual, future, wait;

            actual = mainContainer.FindViewById<ImageButton>(Resource.Id.idActual);
            future = mainContainer.FindViewById<ImageButton>(Resource.Id.idFuture);
            wait = mainContainer.FindViewById<ImageButton>(Resource.Id.idWait);

            FragmentManager.BeginTransaction().Add(Resource.Id.idContainer, actualFragment).Commit();

            //Create events for the spinner
            floorName.ItemSelected += new System.EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            var adapter = new ArrayAdapter<string>(this,
                Android.Resource.Layout.SimpleSpinnerItem, floorsNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            floorName.Adapter = adapter;

            date.Click += DateSelect_OnClick;
            hour.Click += HourSelect_OnClick;

            dashboard.Click += OpenDashboard;

            actual.Click += OpenActualFragment;
            future.Click += OpenFutureFragment;
            wait.Click += OpenWaitFragment;
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

        public void OnItemPressed(object source, int idTable)
        {
            floors.ElementAt(floorIndex).AddTable(this.Resources.GetDrawable(idTable), this);
            Toast.MakeText(this, "Posicion: " + idTable , ToastLength.Long).Show();
        }

        public void OnCancel(View view)
        {
            options.RemoveAllViews();
            LayoutInflater inflater = LayoutInflater.From(this);
            View tablePropertiesView = inflater.Inflate(Resource.Layout.layout_main_container, options, true);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            ActualFragment auxFragment = new ActualFragment(actualFragment.actualList);
            transaction.Replace(Resource.Id.idContainer, auxFragment);
            transaction.AddToBackStack(null);
            transaction.Commit();

            Toast.MakeText(this, "Se presiono ", ToastLength.Long).Show();
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
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        public void HourSelect_OnClick(object sender, EventArgs args)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                hour.Text = time.ToShortTimeString();
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        public void OpenDashboard(object sender, EventArgs args)
        {
            Intent dashboard = new Intent(this, typeof(DashboardActivity));
            StartActivity(dashboard);
        }

        public void OpenActualFragment(object sender, EventArgs args)
        {
            FragmentManager.BeginTransaction().Replace(Resource.Id.idContainer, actualFragment).AddToBackStack(null).Commit();
        }

        public void OpenFutureFragment(object sender, EventArgs args)
        {
            FragmentManager.BeginTransaction().Add(Resource.Id.idContainer, futureFragment).AddToBackStack(null).Commit();
        }

        public void OpenWaitFragment(object sender, EventArgs args)
        {
            FragmentManager.BeginTransaction().Replace(Resource.Id.idContainer, waitFragment).AddToBackStack(null).Commit();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}