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
using Android.Support.V4.App;

namespace MrPiattoRestaurant.Pickers
{
    public class DatePickerFragment : Android.Support.V4.App.DialogFragment, DatePickerDialog.IOnDateSetListener 
    {
        public static readonly string TAG = "X: " + typeof(DatePickerFragment).Name.ToUpper();

        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month - 1, currently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            DateTime SelectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _dateSelectedHandler(SelectedDate);
        }
    }
}