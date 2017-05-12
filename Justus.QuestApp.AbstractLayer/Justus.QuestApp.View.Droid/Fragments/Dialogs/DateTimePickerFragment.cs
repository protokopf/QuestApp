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
        private static readonly string DateTimeValueId =
            "Justus.QuestApp.View.Droid.Fragments.Dialogs.DateTimePickerFragment.DateTimeValueId";

        private DatePicker _datePicker;
        private TimePicker _timePicker;
        private TimeSpan _currentTime;

        #region Public static members

        /// <summary>
        /// Creates new instance of DateTimePickerFragment
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimePickerFragment NewInstance(DateTime dateTime)
        {
            DateTimePickerFragment fragment = new DateTimePickerFragment();
            Bundle arguments = new Bundle();
            PutItsDateTime(arguments, dateTime);
            fragment.Arguments = arguments;
            return fragment;
        }

        /// <summary>
        /// Tries to retrieve date time, that was placed there by instance this class.
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public static DateTime GetItsDateTime(Bundle bundle)
        {
            DateTime dt = DateTime.MinValue;

            if (bundle != null)
            {
                string dateTimeString = bundle.GetString(DateTimeValueId);

                if (!string.IsNullOrWhiteSpace(dateTimeString))
                {
                    DateTime.TryParse(dateTimeString, ParsingCulture, DateTimeStyles.None, out dt);
                }
            }
            return dt;
        }

        #endregion

        #region DialogFragment overriding

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Android.Views.View dialogView = LayoutInflater.From(Activity).Inflate(Resource.Layout.DateTimePicker,null);

            _datePicker = dialogView.FindViewById<DatePicker>(Resource.Id.datePicker);
            _timePicker = dialogView.FindViewById<TimePicker>(Resource.Id.timePicker);

            DateTime dateTime = DateTimePickerFragment.GetItsDateTime(Arguments);
            HandleDatePicker(_datePicker, dateTime);
            HandleTimePicker(_timePicker, dateTime);
            
            return new Android.Support.V7.App.AlertDialog.
                    Builder(Activity).
                SetTitle(Resource.String.DatePickerTitle).
                SetView(dialogView).
                SetPositiveButton(Android.Resource.String.Ok, DatePickerOkHandler).
                Create();
        }

        public override void OnDestroy()
        {
            _timePicker.TimeChanged -= TimePickerOnTimeChanged;
            base.OnDestroy();
        }

        #endregion

        #region Private methods

        private void HandleTimePicker(TimePicker timePicker, DateTime dateTime)
        {
            _timePicker.TimeChanged += TimePickerOnTimeChanged;
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

        private static CultureInfo ParsingCulture => CultureInfo.CurrentUICulture;

        #region Event handlers

        /// <summary>
        /// Handles date pickers ok click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePickerOkHandler(object sender, DialogClickEventArgs e)
        {
            if (TargetFragment != null)
            {
                DateTime currentDateTime = _datePicker.DateTime.Add(_currentTime);

                Bundle bundle = new Bundle();
                PutItsDateTime(bundle, currentDateTime);

                Intent intent = new Intent();
                intent.PutExtras(bundle);

                TargetFragment.OnActivityResult(TargetRequestCode, (int)Result.Ok, intent);
            }
        }

        /// <summary>
        /// Fires, when time from time picker is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="timeChangedEventArgs"></param>
        private void TimePickerOnTimeChanged(object sender, TimePicker.TimeChangedEventArgs timeChangedEventArgs)
        {
            _currentTime = new TimeSpan(timeChangedEventArgs.HourOfDay, timeChangedEventArgs.Minute,0);
        }

        #endregion

        #region Static methods

        private static void PutItsDateTime(Bundle bundle, DateTime dateTime)
        {
            bundle?.PutString(DateTimeValueId, dateTime.ToString(ParsingCulture));
        }

        #endregion

        #endregion
    }
}