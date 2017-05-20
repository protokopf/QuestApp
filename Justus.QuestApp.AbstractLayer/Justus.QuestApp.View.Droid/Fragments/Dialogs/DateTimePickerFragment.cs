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
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.EntityStateHandlers;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using Fragment = Android.Support.V4.App.Fragment;

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

        private static readonly IEntityStateHandler<DateTime> DateTimeHandler = new DateTimeStateHandler();

        public DateTimePickerFragment()
        {
            
        }

        public DateTimePickerFragment(DateTime dateTime)
        {
            Bundle arguments = new Bundle();
            PutItsDateTime(arguments, dateTime);
            Arguments = arguments;
        }

        #region Public static members

        /// <summary>
        /// Creates new instance of DateTimePickerFragment
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="targetFragment">Fragment, that will receive date after ok click.</param>
        /// <param name="requestCode">Request code, used to identify result.</param>
        /// <returns></returns>
        public static DateTimePickerFragment NewInstance(DateTime dateTime, Fragment targetFragment, int requestCode)
        {
            if (targetFragment == null)
            {
                throw new ArgumentNullException(nameof(targetFragment));
            }
            DateTimePickerFragment fragment = new DateTimePickerFragment(dateTime);
            fragment.SetTargetFragment(targetFragment, requestCode);
            return fragment;
        }

        /// <summary>
        /// Tries to retrieve date time, that was placed there by instance this class.
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public static DateTime GetItsDateTime(Bundle bundle)
        {
            DateTime dateTime = DateTime.MinValue;
            DateTimeHandler.Extract(DateTimeValueId, bundle, ref dateTime);
            return dateTime;
        }

        #endregion

        #region DialogFragment overriding

        ///<inheritdoc/>
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Android.Views.View dialogView = LayoutInflater.From(Activity).Inflate(Resource.Layout.DateTimePicker,null);

            _datePicker = dialogView.FindViewById<DatePicker>(Resource.Id.datePicker);
            _timePicker = dialogView.FindViewById<TimePicker>(Resource.Id.timePicker);

            DateTime dateTime = GetItsDateTime(Arguments);
            HandleDatePicker(_datePicker, dateTime);
            HandleTimePicker(_timePicker, dateTime);
            
            return new Android.Support.V7.App.AlertDialog.
                    Builder(Activity).
                SetTitle(Resource.String.DatePickerTitle).
                SetView(dialogView).
                SetPositiveButton(Android.Resource.String.Ok, DatePickerOkHandler).
                Create();
        }

        ///<inheritdoc/>
        public override void OnDestroyView()
        {
            _timePicker.TimeChanged -= TimePickerOnTimeChanged;
            base.OnDestroyView();
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

        private void PutItsDateTime(Bundle bundle, DateTime dateTime)
        {
            DateTimeHandler.Save(DateTimeValueId, dateTime, bundle);
        }

        #endregion

        #endregion
    }
}