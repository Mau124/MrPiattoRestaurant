using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V7.Widget;


using MrPiattoRestaurant.Models;
using MrPiattoRestaurant.Pickers;
using MrPiattoRestaurant.adapters;
using MrPiattoRestaurant.ModelsDB;
using MrPiattoRestaurant.Resources.utilities;

namespace MrPiattoRestaurant.Fragments
{
    public class AboutMe : Android.Support.V4.App.Fragment
    {
        private Context context;

        private Restaurant restaurant = new Restaurant();
        private List<Waiters> waiters = new List<Waiters>();
        private Schedule schedule = new Schedule();
        private APICaller API = new APICaller();
        private APIUpdate APIupdate = new APIUpdate();

        //This is for schedule
        TextView monday1, monday2;
        TextView tuesday1, tuesday2;
        TextView wednesday1, wednesday2;
        TextView thursday1, thursday2;
        TextView friday1, friday2;
        TextView saturday1, saturday2;
        TextView sunday1, sunday2;

        CheckBox checkMonday, checkTuesday;
        CheckBox checkWednesday, checkThursday;
        CheckBox checkFriday, checkSaturday;
        CheckBox checkSunday;

        Button addWaiter;
        Button modSche;

        TextView resName, resMail, resDesc;
        TextView resDir, resPhone, resDress, resPrice, resCat, resPay;
        LinearLayout modifyRes, modifyPass, modifyHours;

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        WaitersAdapter mAdapter;

        public AboutMe(Context context, Restaurant restaurant)
        {
            this.context = context;
            this.restaurant = restaurant;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.activity_dashboard_aboutme, container, false);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.idRecyclerView);

            addWaiter = view.FindViewById<Button>(Resource.Id.idAddWaiter);

            resName = view.FindViewById<TextView>(Resource.Id.idRestaurantName);
            resMail = view.FindViewById<TextView>(Resource.Id.idRestaurantMail);
            resDesc = view.FindViewById<TextView>(Resource.Id.idRestaurantDesc);
            resDir = view.FindViewById<TextView>(Resource.Id.idRestaurantAddress);
            resPhone = view.FindViewById<TextView>(Resource.Id.idRestaurantPhone);
            resDress = view.FindViewById<TextView>(Resource.Id.idRestaurantDress);
            resPrice = view.FindViewById<TextView>(Resource.Id.idRestaurantPrice);
            resCat = view.FindViewById<TextView>(Resource.Id.idRestaurantCat);
            resPay = view.FindViewById<TextView>(Resource.Id.idRestaurantPay);


            modifyRes = view.FindViewById<LinearLayout>(Resource.Id.idModResInfo);
            modifyPass = view.FindViewById<LinearLayout>(Resource.Id.idModPass);
            modifyHours = view.FindViewById<LinearLayout>(Resource.Id.idModSche);

            InitializeRes();
            InitializeWaiters();
            InitializeSchedule();

            mLayoutManager = new LinearLayoutManager(context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new WaitersAdapter(context, waiters);
            mRecyclerView.SetAdapter(mAdapter);

            addWaiter.Click += onAddWaiter;

            modifyRes.Click += modifyRestaurant;
            modifyPass.Click += modifyPassword;
            modifyHours.Click += modifyHoursRes;

            mAdapter.ChangeItem += changeWaiter;
            return view;
        }

        private void InitializeRes()
        {
            resName.Text = restaurant.Name;
            resMail.Text = restaurant.Mail;
            resDesc.Text = restaurant.Description;
            resDir.Text = restaurant.Address;
            resPhone.Text = restaurant.Phone;
            resDress.Text = restaurant.Dress;
            // Categoria
            // Pago
        }

        private void modifyRestaurant(object sender, EventArgs e)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_modify_res, null);

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);

            EditText resName, resDesc, resDir;
            EditText resPhone, resDress, resPrice;
            EditText resCat, resPay;

            Button mod;

            resName = content.FindViewById<EditText>(Resource.Id.idRestaurantName);
            resDesc = content.FindViewById<EditText>(Resource.Id.idRestaurantDesc);
            resDir = content.FindViewById<EditText>(Resource.Id.idRestaurantAddress);
            resPhone = content.FindViewById<EditText>(Resource.Id.idRestaurantPhone);
            resDress = content.FindViewById<EditText>(Resource.Id.idRestaurantDress);
            resPrice = content.FindViewById<EditText>(Resource.Id.idRestaurantPrice);
            resCat = content.FindViewById<EditText>(Resource.Id.idRestaurantCat);
            resPay = content.FindViewById<EditText>(Resource.Id.idRestaurantPay);

            mod = content.FindViewById<Button>(Resource.Id.idMod);

            resName.Hint = restaurant.Name;
            resDesc.Hint = restaurant.Description;
            resDir.Hint = restaurant.Address;
            resPhone.Hint = restaurant.Phone;
            resDress.Hint = restaurant.Dress;
            resPrice.Hint = "$" + restaurant.Price.ToString();

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

            mod.Click += async delegate
            {
                if (!resName.Text.Equals(""))
                {
                    restaurant.Name = resName.Text;
                }
                if (!resDesc.Text.Equals(""))
                {
                    restaurant.Description = resDesc.Text;
                }
                if (!resDir.Text.Equals(""))
                {
                    restaurant.Address = resDir.Text;
                }
                if (!resPhone.Text.Equals(""))
                {
                    restaurant.Phone = resPhone.Text;
                }
                if (!resDress.Text.Equals(""))
                {
                    restaurant.Dress = resDress.Text;
                }
                if (!resPrice.Text.Equals(""))
                {
                    restaurant.Price = Double.Parse(resPrice.Text);
                }

                var response = await APIupdate.UpdateRestaurant(restaurant);
                Toast.MakeText(context, response, ToastLength.Long).Show();
                InitializeRes();
                alertDialog.Dismiss();
            };
        }
        private void InitializeWaiters()
        {
            waiters = API.GetWaiters(restaurant.Idrestaurant);
        }

        private void InitializeSchedule()
        {
            schedule = API.GetSchedule(restaurant.Idrestaurant);
        }

        private void modifyPassword(object sender, EventArgs e)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_new_password, null);
            EditText oldPass, newPass, newPass2;

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);
            Button mod = content.FindViewById<Button>(Resource.Id.idMod);
            oldPass = content.FindViewById<EditText>(Resource.Id.idActPass);
            newPass = content.FindViewById<EditText>(Resource.Id.idPass);
            newPass2 = content.FindViewById<EditText>(Resource.Id.idPassConf);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

            mod.Click += async delegate
            {
                if (oldPass.Text.Equals("") || newPass.Text.Equals("") || newPass2.Text.Equals(""))
                {
                    Toast.MakeText(context, "Alguno de los campos no ha sido llenado", ToastLength.Long).Show();
                } else if(!oldPass.Text.Equals(restaurant.Password))
                {
                    Toast.MakeText(context, "Contraseña actual incorrecta", ToastLength.Long).Show();
                } else if (!newPass.Text.Equals(newPass2.Text))
                {
                    Toast.MakeText(context, "Las contraseñas no coinciden", ToastLength.Long).Show();
                } else
                {
                    restaurant.Password = newPass.Text;
                    var response = await APIupdate.UpdateRestaurant(restaurant);
                    Toast.MakeText(context, response, ToastLength.Long).Show();
                    alertDialog.Dismiss();
                }
            };
        }

        private void modifyHoursRes(object sender, EventArgs e)
        {
            InitializeSchedule();

            View content = LayoutInflater.Inflate(Resource.Layout.layout_Schedule, null);

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);
            modSche = content.FindViewById<Button>(Resource.Id.idMod);

            monday1 = content.FindViewById<TextView>(Resource.Id.idMonday1);
            monday2 = content.FindViewById<TextView>(Resource.Id.idMonday2);
            tuesday1 = content.FindViewById<TextView>(Resource.Id.idTuesday1);
            tuesday2 = content.FindViewById<TextView>(Resource.Id.idTuesday2);
            wednesday1 = content.FindViewById<TextView>(Resource.Id.idWednesday1);
            wednesday2 = content.FindViewById<TextView>(Resource.Id.idWednesday2);
            thursday1 = content.FindViewById<TextView>(Resource.Id.idThursday1);
            thursday2 = content.FindViewById<TextView>(Resource.Id.idThursday2);
            friday1 = content.FindViewById<TextView>(Resource.Id.idFriday1);
            friday2 = content.FindViewById<TextView>(Resource.Id.idFriday2);
            saturday1 = content.FindViewById<TextView>(Resource.Id.idSaturday1);
            saturday2 = content.FindViewById<TextView>(Resource.Id.idSaturday2);
            sunday1 = content.FindViewById<TextView>(Resource.Id.idSunday1);
            sunday2 = content.FindViewById<TextView>(Resource.Id.idSunday2);

            checkMonday = content.FindViewById<CheckBox>(Resource.Id.idCheckMonday);
            checkTuesday = content.FindViewById<CheckBox>(Resource.Id.idCheckTuesday);
            checkWednesday = content.FindViewById<CheckBox>(Resource.Id.idCheckWednesday);
            checkThursday = content.FindViewById<CheckBox>(Resource.Id.idCheckThursday);
            checkFriday = content.FindViewById<CheckBox>(Resource.Id.idCheckFriday);
            checkSaturday = content.FindViewById<CheckBox>(Resource.Id.idCheckSaturday);
            checkSunday = content.FindViewById<CheckBox>(Resource.Id.idCheckSunday);

            printSchedule();

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                InitializeSchedule();
                alertDialog.Dismiss();
            };

            modSche.Click += async delegate
            {
                var response = await APIupdate.UpdateSchedule(schedule);
                Toast.MakeText(context, response, ToastLength.Long).Show();
                alertDialog.Dismiss();
            };

            monday1.Click += clickMonday1;
            monday2.Click += clickMonday2;
            tuesday1.Click += clickTuesday1;
            tuesday2.Click += clickTuesday2;
            wednesday1.Click += clickWednesday1;
            wednesday2.Click += clickWednesday2;
            thursday1.Click += clickThursday1;
            thursday2.Click += clickThursday2;
            friday1.Click += clickFriday1;
            friday2.Click += clickFriday2;
            saturday1.Click += clickSaturday1;
            saturday2.Click += clickSaturday2;
            sunday1.Click += clickSunday1;
            sunday2.Click += clickSunday2;

            checkMonday.Click += mondayChecked;
            checkTuesday.Click += tuesdayChecked;
            checkWednesday.Click += wednesdayChecked;
            checkThursday.Click += thursdayChecked;
            checkFriday.Click += fridayChecked;
            checkSaturday.Click += saturdayChecked;
            checkSunday.Click += sundayChecked;
        }

        private void printSchedule()
        {
            // Lunes
            if (schedule.Otmonday.HasValue && schedule.Ctmonday.HasValue)
            {
                monday1.Text = schedule.Otmonday.Value.ToString(@"hh\:mm");
                monday2.Text = schedule.Ctmonday.Value.ToString(@"hh\:mm");
                checkMonday.Checked = false;
            } else
            {
                monday1.Text = "--:--";
                monday2.Text = "--:--";
                checkMonday.Checked = true;
            }

            // Martes
            if (schedule.Ottuesday.HasValue && schedule.Cttuestday.HasValue)
            {
                tuesday1.Text = schedule.Ottuesday.Value.ToString(@"hh\:mm");
                tuesday2.Text = schedule.Cttuestday.Value.ToString(@"hh\:mm");
                checkTuesday.Checked = false;
            } else
            {
                tuesday1.Text = "--:--";
                tuesday2.Text = "--:--";
                checkTuesday.Checked = true;
            }

            // Miercoles
            if (schedule.Otwednesday.HasValue && schedule.Ctwednesday.HasValue)
            {
                wednesday1.Text = schedule.Otwednesday.Value.ToString(@"hh\:mm");
                wednesday2.Text = schedule.Ctwednesday.Value.ToString(@"hh\:mm");
                checkWednesday.Checked = false;
            } else
            {
                wednesday1.Text = "--:--";
                wednesday2.Text = "--:--";
                checkWednesday.Checked = true;
            }

            // Jueves
            if (schedule.Otthursday.HasValue && schedule.Ctthursday.HasValue)
            {
                thursday1.Text = schedule.Otthursday.Value.ToString(@"hh\:mm");
                thursday2.Text = schedule.Ctthursday.Value.ToString(@"hh\:mm");
                checkThursday.Checked = false;
            } else
            {
                thursday1.Text = "--:--";
                thursday2.Text = "--:--";
                checkThursday.Checked = true;
            }

            // Viernes
            if (schedule.Otfriday.HasValue && schedule.Ctfriday.HasValue)
            {
                friday1.Text = schedule.Otfriday.Value.ToString(@"hh\:mm");
                friday2.Text = schedule.Ctfriday.Value.ToString(@"hh\:mm");
                checkFriday.Checked = false;
            } else
            {
                friday1.Text = "--:--";
                friday2.Text = "--:--";
                checkFriday.Checked = true;
            }

            // Sabado
            if (schedule.Otsaturday.HasValue && schedule.Ctsaturday.HasValue)
            {
                saturday1.Text = schedule.Otsaturday.Value.ToString(@"hh\:mm");
                saturday2.Text = schedule.Ctsaturday.Value.ToString(@"hh\:mm");
                checkSaturday.Checked = false;
            } else
            {
                saturday1.Text = "--:--";
                saturday2.Text = "--:--";
                checkSaturday.Checked = true;
            }

            // Domingo
            if (schedule.Otsunday.HasValue && schedule.Ctsunday.HasValue)
            {
                sunday1.Text = schedule.Otsunday.Value.ToString(@"hh\:mm");
                sunday2.Text = schedule.Ctsunday.Value.ToString(@"hh\:mm");
                checkSunday.Checked = false;
            } else
            {
                sunday1.Text = "--:--";
                sunday2.Text = "--:--";
                checkSunday.Checked = true;
            }
        }

        private void clickMonday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                monday1.Text = time.ToString("hh:mm");
                schedule.Otmonday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickMonday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                monday2.Text = time.ToString("hh:mm");
                schedule.Ctmonday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickTuesday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                tuesday1.Text = time.ToString("hh:mm");
                schedule.Ottuesday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickTuesday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                tuesday2.Text = time.ToString("hh:mm");
                schedule.Cttuestday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickWednesday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                wednesday1.Text = time.ToString("hh:mm");
                schedule.Otwednesday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickWednesday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                wednesday2.Text = time.ToString("hh:mm");
                schedule.Ctwednesday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickThursday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                thursday1.Text = time.ToString("hh:mm");
                schedule.Otthursday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickThursday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                thursday2.Text = time.ToString("hh:mm");
                schedule.Ctthursday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickFriday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                friday1.Text = time.ToString("hh:mm");
                schedule.Otfriday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickFriday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                friday2.Text = time.ToString("hh:mm");
                schedule.Ctfriday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSaturday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                saturday1.Text = time.ToString("hh:mm");
                schedule.Otsaturday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSaturday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                saturday2.Text = time.ToString("hh:mm");
                schedule.Ctsaturday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSunday1(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                sunday1.Text = time.ToString("hh:mm");
                schedule.Otsunday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void clickSunday2(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
            {
                sunday2.Text = time.ToString("hh:mm");
                schedule.Ctsunday = time.TimeOfDay;
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void mondayChecked(object sender, EventArgs e)
        {
            if (checkMonday.Checked)
            {
                monday1.Text = "--:--";
                monday2.Text = "--:--";
                schedule.Otmonday = null;
                schedule.Ctmonday = null;
            } 
        }

        private void tuesdayChecked(object sender, EventArgs e)
        {
            if (checkTuesday.Checked)
            {
                tuesday1.Text = "--:--";
                tuesday2.Text = "--:--";
                schedule.Ottuesday = null;
                schedule.Cttuestday = null;
            }
        }

        private void wednesdayChecked(object sender, EventArgs e)
        {
            if (checkWednesday.Checked)
            {
                wednesday1.Text = "--:--";
                wednesday2.Text = "--:--";
                schedule.Otwednesday = null;
                schedule.Ctwednesday = null;
            }
        }

        private void thursdayChecked(object sender, EventArgs e)
        {
            if (checkThursday.Checked)
            {
                thursday1.Text = "--:--";
                thursday2.Text = "--:--";
                schedule.Otthursday = null;
                schedule.Ctthursday = null;
            }
        }

        private void fridayChecked(object sender, EventArgs e)
        {
            if (checkFriday.Checked)
            {
                friday1.Text = "--:--";
                friday2.Text = "--:--";
                schedule.Otfriday = null;
                schedule.Ctfriday = null;
            }
        }

        private void saturdayChecked(object sender, EventArgs e)
        {
            if (checkSaturday.Checked)
            {
                saturday1.Text = "--:--";
                saturday2.Text = "--:--";
                schedule.Otsaturday = null;
                schedule.Ctsaturday = null;
            }
        }

        private void sundayChecked(object sender, EventArgs e)
        {
            if (checkSunday.Checked)
            {
                sunday1.Text = "--:--";
                sunday2.Text = "--:--";
                schedule.Otsunday = null;
                schedule.Ctsunday = null;
            }
        }
        private void onAddWaiter(object sender, EventArgs e)
        {
            View content = LayoutInflater.Inflate(Resource.Layout.layout_add_waiter, null);

            ImageView dismiss = content.FindViewById<ImageView>(Resource.Id.idDismiss);
            EditText name = content.FindViewById<EditText>(Resource.Id.idName);
            EditText lastName = content.FindViewById<EditText>(Resource.Id.idLastName);
            Button button = content.FindViewById<Button>(Resource.Id.idButton);

            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(context).Create();
            alertDialog.SetCancelable(true);
            alertDialog.SetView(content);
            alertDialog.Show();

            dismiss.Click += delegate
            {
                alertDialog.Dismiss();
            };

            button.Click += async delegate {
                if (name.Text.Equals("") || lastName.Text.Equals(""))
                {
                    Toast.MakeText(context, "Falta algun campo", ToastLength.Long).Show();
                }
                else
                {
                    Waiters waiter = new Waiters();

                    waiter.Idrestaurant = restaurant.Idrestaurant;
                    waiter.WaiterFirstName = name.Text;
                    waiter.WaiterLasName = lastName.Text;

                    var response = await APIupdate.AddWaiter(waiter);
                    Toast.MakeText(context, response, ToastLength.Long).Show();

                    waiters.Clear();
                    waiters = API.GetWaiters(restaurant.Idrestaurant);

                    mAdapter = new WaitersAdapter(context, waiters);
                    mRecyclerView.SetAdapter(mAdapter);

                    alertDialog.Dismiss();
                }
            };

        }

        private void changeWaiter(object sender, EventArgs e)
        {
            mAdapter = new WaitersAdapter(context, waiters);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.ChangeItem += changeWaiter;
        }
    }
}