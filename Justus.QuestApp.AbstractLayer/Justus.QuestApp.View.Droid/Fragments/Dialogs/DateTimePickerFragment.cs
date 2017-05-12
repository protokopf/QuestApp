using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace Justus.QuestApp.View.Droid.Fragments.Dialogs
{
    /// <summary>
    /// Fragment used to pick some date.
    /// </summary>
    public class DateTimePickerFragment : DialogFragment
    {
        /// <summary>
        /// Id fot identifying value of datetime picker.
        /// </summary>
        public static readonly string DateTimeValueId =
            "Justus.QuestApp.View.Droid.Fragments.Dialogs.DateTimePickerFragment.DateTimeValueId";

        private DatePicker _datePicker;
        private TimePicker _timePicker;

        public static DateTimePickerFragment NewInstance(DateTime dateTime)
        {
            DateTimePickerFragment fragment = new DateTimePickerFragment();
            Bundle arguments = new Bundle();
            arguments.PutString(DateTimeValueId, dateTime.ToString(GetParsingCulture()));
            fragment.Arguments = arguments;
            return fragment;
        }

        #region DialogFragment overriding

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Android.Views.View dialogView = LayoutInflater.From(Activity).Inflate(Resource.Layout.DateTimePicker,null);

            _datePicker = dialogView.FindViewById<DatePicker>(Resource.Id.datePicker);
            _timePicker = dialogView.FindViewById<TimePicker>(Resource.Id.timePicker);

            if (Arguments != null)
            {
                DateTime dateTime = TryGetDateTime(Arguments);
                HandleDatePicker(_datePicker, dateTime);
                HandleTimePicker(_timePicker, dateTime);
            }

            return new Android.Support.V7.App.AlertDialog.
                    Builder(Activity).
                SetTitle(Resource.String.DatePickerTitle).
                SetView(dialogView).
                SetPositiveButton(Android.Resource.String.Ok, DatePickerOkHandler).
                Create();
        }


        #endregion

        #region Private methods

        private DateTime TryGetDateTime(Bundle bundle)
        {
            string dateTimeString = bundle.GetString(DateTimeValueId);

            DateTime dt = DateTime.MinValue;

            if (!string.IsNullOrWhiteSpace(dateTimeString))
            {
                DateTime.TryParse(dateTimeString, GetParsingCulture(), DateTimeStyles.None, out dt);
            }
            return dt;
        }

        private void HandleTimePicker(TimePicker timePicker, DateTime dateTime)
        {
            if (dateTime != DateTime.MinValue)
            {
                timePicker.CurrentHour = new Integer(dateTime.Hour);
                timePicker.CurrentMinute = new Integer(dateTime.Minute);
            }
        }

        private void HandleDatePicker(DatePicker datePicker, DateTime dateTime)
        {
            if (dateTime != DateTime.MinValue)
            {
                datePicker.DateTime = dateTime;
            }
        }

        private static CultureInfo GetParsingCulture()
        {
            return CultureInfo.CurrentUICulture;;
        }

        #region Handlers

        /// <summary>
        /// Handles date pickers ok click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePickerOkHandler(object sender, DialogClickEventArgs e)
        {
        }

        #endregion


        #endregion
    }
}