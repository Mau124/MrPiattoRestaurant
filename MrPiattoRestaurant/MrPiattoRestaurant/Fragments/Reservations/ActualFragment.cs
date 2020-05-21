using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Java.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Bluetooth;
using System.Threading.Tasks;
using System.IO;

using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using static Android.Gms.Vision.Detector;

using MrPiattoRestaurant.adapters.actualListAdapters;
using MrPiattoRestaurant.Models.Reservations;
using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.Resources.utilities;
using MrPiattoRestaurant.adapters;

namespace MrPiattoRestaurant.Fragments.Reservations
{
    public class ActualFragment : Android.Support.V4.App.Fragment, ISurfaceHolderCallback, IProcessor
    {
        // Propiedades para la lectura del qr
        SurfaceView surfaceView;
        TextView txtResult;
        BarcodeDetector barcodeDetector;
        CameraSource cameraSource;
        const int RequestCameraPermissionID = 1001;

        // Propiedades generales
        Button button;
        RecyclerView mRecyclerView;
        LinearLayout message;

        //RecyclerView elements
        public RecyclerView.LayoutManager mLayoutManager;
        public ActualListAdapter mAdapter;

        private APICaller API = new APICaller();

        public List<Table> ocupiedTables = new List<Table>();
        public List<GestureRecognizerView> floors = new List<GestureRecognizerView>();

        public Context context;
        public Restaurant restaurant;

        // Propiedades para NFC
        private Java.Lang.String dataToSend;
        //Variables para el manejo del bluetooth Adaptador y Socket
        private BluetoothAdapter mBluetoothAdapter = null;
        private BluetoothSocket btSocket = null;
        //Streams de lectura I/O
        private Stream outStream = null;
        private Stream inStream = null;
        //MAC Address del dispositivo Bluetooth
        private static string address = "98:d3:61:f9:52:f1";
        //Id Unico de comunicacion
        private static UUID MY_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        //Widget
        TextView Result;

        public delegate void UpdateEventHandler();
        public event UpdateEventHandler Update;
        protected virtual void OnUpdate()
        {
            if (Update != null)
            {
                Update();
            }
        }

        public ActualFragment()
        {
            //Default Constructor
        }
        public ActualFragment(Context context, Restaurant restaurant, List<Table> ocupiedTables, List<GestureRecognizerView> floors)
        {
            this.context = context;
            this.ocupiedTables = ocupiedTables;
            this.restaurant = restaurant;
            this.floors = floors;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.recycler_actualList, container, false);

            button = view.FindViewById<Button>(Resource.Id.idButton);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);
            message = view.FindViewById<LinearLayout>(Resource.Id.idNoClients);

            mLayoutManager = new LinearLayoutManager(Application.Context);
            mAdapter = new ActualListAdapter(context, ocupiedTables);
            mAdapter.EndFood += OnEndFood;

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            if (ocupiedTables.Any())
            {
                mRecyclerView.Visibility = ViewStates.Visible;
                message.Visibility = ViewStates.Gone;
            } 
            else
            {
                mRecyclerView.Visibility = ViewStates.Gone;
                message.Visibility = ViewStates.Visible;
            }

            button.Click += NewClient;

            return view;
        }

        //public void Update(List<Table> ocupiedTables)
        //{
        //    this.ocupiedTables = ocupiedTables;
        //    mAdapter = new ActualListAdapter(context, ocupiedTables);
        //    mAdapter.EndFood += OnEndFood;
        //    mRecyclerView.SetAdapter(mAdapter);
        //}
        
        private void OnEndFood(int position)
        {
            mAdapter = new ActualListAdapter(context, ocupiedTables);
            mAdapter.EndFood += OnEndFood;
            mRecyclerView.SetAdapter(mAdapter);
        }

        public void NewClient(object sender, EventArgs e)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.dialog_new_client, null);
            LinearLayout qr, nfc, manual;

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            qr = content.FindViewById<LinearLayout>(Resource.Id.idQR);
            nfc = content.FindViewById<LinearLayout>(Resource.Id.idNFC);
            manual = content.FindViewById<LinearLayout>(Resource.Id.idManual);

            manual.Click += delegate
            {
                alertDialog.Dismiss();
                ManualAssign();
            };

            qr.Click += delegate
            {
                alertDialog.Dismiss();
                QrAssign();
            };

            nfc.Click += delegate
            {
                alertDialog.Dismiss();
                NFCAssign();
            };

        }

        private void ManualAssign()
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_manual_assigment, null);
            TextView hour;
            RecyclerView mRecyclerView;

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            hour = content.FindViewById<TextView>(Resource.Id.idHour);
            mRecyclerView = content.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            hour.Click += delegate
            {
                TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
                {
                    hour.Text = time.ToString("hh:mm tt");

                    List<Models.Notification> res = new List<Models.Notification>();
                    List<AuxiliarReservation>? auxReservations = API.GetNotAuxReservationsByHour(restaurant.Idrestaurant, time);
                    List<ManualReservations>? manReservations = API.GetNotManReservationsByHour(restaurant.Idrestaurant, time);

                    if (auxReservations != null)
                    {
                        foreach (AuxiliarReservation r in auxReservations)
                        {
                            res.Add(new Models.Notification(r.Name, r.LastName, "Union", r.Date, r.Phone, r.IdauxiliarTableNavigation.StringIdtables));
                        }
                    }

                    if (manReservations != null)
                    {
                        foreach (ManualReservations r in manReservations)
                        {
                            res.Add(new Models.Notification(r.Name, r.LastName, r.IdtableNavigation.tableName, r.Date, r.Phone, r.IdtableNavigation.floorIndex, r.IdtableNavigation.Idtables));
                        }
                    }

                    RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(Application.Context);
                    AssigmentAdapter mAdapter = new AssigmentAdapter(context, res, floors, restaurant);

                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    mRecyclerView.SetAdapter(mAdapter);

                    mAdapter.FinishSelection += delegate
                    {
                        OnUpdate();
                        alertDialog.Dismiss();
                    };
                });
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };
        }

        private void QrAssign()
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_check_qr, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            surfaceView = content.FindViewById<SurfaceView>(Resource.Id.cameraView);
            txtResult = content.FindViewById<TextView>(Resource.Id.txtResult);
            Bitmap bitMap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.qr);
            barcodeDetector = new BarcodeDetector.Builder(context)
                .SetBarcodeFormats(BarcodeFormat.QrCode)
                .Build();

            cameraSource = new CameraSource
                .Builder(context, barcodeDetector)
                .SetRequestedPreviewSize(640, 480)
                .Build();

            surfaceView.Holder.AddCallback(this);
            barcodeDetector.SetProcessor(this);

        }

        private void NFCAssign()
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_check_nfc, null);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            Result = content.FindViewById<TextView>(Resource.Id.idResult);

            //CheckBt();
            //Connect();

            //alertDialog.DismissEvent += delegate
            //{
            //    if (btSocket.IsConnected)
            //    {
            //        try
            //        {
            //            btSocket.Close();
            //        }
            //        catch (System.Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //};
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestCameraPermissionID:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            if (ActivityCompat.CheckSelfPermission(context, Android.Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                            {
                                // Request Permission
                                ActivityCompat.RequestPermissions(Activity, new string[]
                                {
                                Android.Manifest.Permission.Camera
                                }, RequestCameraPermissionID);
                                return;
                            }
                            try
                            {
                                cameraSource.Start(surfaceView.Holder);
                            }
                            catch (InvalidOperationException)
                            {
                            }
                        }
                    }
                    break;
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (ActivityCompat.CheckSelfPermission(Activity, Android.Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                //Request Permision  
                ActivityCompat.RequestPermissions(Activity, new string[]
                {
                    Android.Manifest.Permission.Camera
                }, RequestCameraPermissionID);
                return;
            }
            try
            {
                cameraSource.Start(surfaceView.Holder);
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray qrcodes = detections.DetectedItems;
            if (qrcodes.Size() != 0)
            {
                txtResult.Post(() => {
                    Vibrator vibrator = (Vibrator)context.GetSystemService(Context.VibratorService);
                    vibrator.Vibrate(1000);
                    txtResult.Text = ((Barcode)qrcodes.ValueAt(0)).RawValue;
                });
            }
        }

        public void Release()
        {

        }

        // Metodos para la conexion de bluetooth
        //Metodo de verificacion del sensor Bluetooth
        private void CheckBt()
        {
            //asignamos el sensor bluetooth con el que vamos a trabajar
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            //Verificamos que este habilitado
            if (!mBluetoothAdapter.Enable())
            {
                Toast.MakeText(context, "Bluetooth Desactivado",
                    ToastLength.Short).Show();
            }
            //verificamos que no sea nulo el sensor
            if (mBluetoothAdapter == null)
            {
                Toast.MakeText(context,
                    "Bluetooth No Existe o esta Ocupado", ToastLength.Short)
                    .Show();
            }
        }

        //Evento de conexion al Bluetooth
        public void Connect()
        {
            //Iniciamos la conexion con el arduino
            BluetoothDevice device = mBluetoothAdapter.GetRemoteDevice(address);
            System.Console.WriteLine("Conexion en curso" + device);

            //Indicamos al adaptador que ya no sea visible
            mBluetoothAdapter.CancelDiscovery();
            try
            {
                //Inicamos el socket de comunicacion con el arduino
                btSocket = device.CreateRfcommSocketToServiceRecord(MY_UUID);
                //Conectamos el socket
                btSocket.Connect();
                System.Console.WriteLine("Conexion Correcta");
            }
            catch (System.Exception e)
            {
                //en caso de generarnos error cerramos el socket
                Console.WriteLine(e.Message);
                try
                {
                    btSocket.Close();
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Imposible Conectar");
                }
                System.Console.WriteLine("Socket Creado");
            }
            //Una vez conectados al bluetooth mandamos llamar el metodo que generara el hilo
            //que recibira los datos del arduino
            beginListenForData();
            //NOTA envio la letra e ya que el sketch esta configurado para funcionar cuando
            //recibe esta letra.
            dataToSend = new Java.Lang.String("e");
            writeData(dataToSend);
        }

        //Evento para inicializar el hilo que escuchara las peticiones del bluetooth
        public void beginListenForData()
        {
            //Extraemos el stream de entrada
            try
            {
                inStream = btSocket.InputStream;
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Creamos un hilo que estara corriendo en background el cual verificara si hay algun dato
            //por parte del arduino
            Task.Factory.StartNew(() => {
                //declaramos el buffer donde guardaremos la lectura
                byte[] buffer = new byte[1024];
                //declaramos el numero de bytes recibidos
                int bytes;
                while (true)
                {
                    try
                    {
                        //leemos el buffer de entrada y asignamos la cantidad de bytes entrantes
                        bytes = inStream.Read(buffer, 0, buffer.Length);
                        //Verificamos que los bytes contengan informacion
                        if (bytes > 0)
                        {
                            //Corremos en la interfaz principal
                            Activity.RunOnUiThread(() => {
                                //Convertimos el valor de la informacion llegada a string
                                string valor = System.Text.Encoding.ASCII.GetString(buffer);
                                //Agregamos a nuestro label la informacion llegada
                                Result.Text = Result.Text + "\n" + valor;
                            });
                        }
                    }
                    catch (Java.IO.IOException)
                    {
                        //En caso de error limpiamos nuestra label y cortamos el hilo de comunicacion
                        Activity.RunOnUiThread(() => {
                            Result.Text = string.Empty;
                        });
                        break;
                    }
                }
            });
        }

        //Metodo de envio de datos la bluetooth
        private void writeData(Java.Lang.String data)
        {
            //Extraemos el stream de salida
            try
            {
                outStream = btSocket.OutputStream;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
            }

            //creamos el string que enviaremos
            Java.Lang.String message = data;

            //lo convertimos en bytes
            byte[] msgBuffer = message.GetBytes();

            try
            {
                //Escribimos en el buffer el arreglo que acabamos de generar
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
            }
        }

    }
}