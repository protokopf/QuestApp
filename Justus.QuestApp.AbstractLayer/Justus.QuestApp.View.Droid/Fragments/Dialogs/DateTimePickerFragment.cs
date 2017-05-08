using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace Justus.QuestApp.View.Droid.Fragments.Dialogs
{
    /// <summary>
    /// Fragment used to pick some date.
    /// </summary>
    public class DateTimePickerFragment : DialogFragment
    {
        private DatePicker _datePicker;
        private TimePicker _timePicker;

        #region DialogFragment overriding

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Android.Views.View dialogView = LayoutInflater.From(Activity).Inflate(Resource.Layout.DateTimePicker,null);

            _datePicker = dialogView.FindViewById<DatePicker>(Resource.Id.datePicker);
            _timePicker = dialogView.FindViewById<TimePicker>(Resource.Id.timePicker);

            return new Android.Support.V7.App.AlertDialog.
                    Builder(Activity).
                SetTitle(Resource.String.DatePickerTitle).
                SetView(dialogView).
                SetPositiveButton(Android.Resource.String.Ok, DatePickerOkHandler).
                Create();
        }

        #endregion

        #region Private methods


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