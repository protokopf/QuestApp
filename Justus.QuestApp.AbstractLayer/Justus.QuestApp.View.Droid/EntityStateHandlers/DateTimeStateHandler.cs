using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;

namespace Justus.QuestApp.View.Droid.EntityStateHandlers
{
    /// <summary>
    /// Handles date time state.
    /// </summary>
    internal class DateTimeStateHandler : IEntityStateHandler<DateTime>
    {
        private static readonly CultureInfo DateTimeCultureInfo = CultureInfo.CurrentUICulture;

        #region IEntityStateHandler<DateTime> implementation

        ///<inheritdoc/>
        public bool Save(string key, DateTime entity, Bundle bundle)
        {
            if (bundle != null && !string.IsNullOrWhiteSpace(key))
            {
                bundle.PutString(key, StringifyDateTime(entity));
                return true;
            }
            return false;
        }

        ///<inheritdoc/>
        public bool Extract(string key, Bundle bundle, ref DateTime entity)
        {
            if (bundle != null && !string.IsNullOrWhiteSpace(key))
            {
                string dateTimeString = bundle.GetString(key);
                entity = ParseDateTimeString(dateTimeString);
                return true;
            }
            return false;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Stringify dateTime to string.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private string StringifyDateTime(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeCultureInfo);
        }

        /// <summary>
        /// Try parse string to dateTime.
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        private DateTime ParseDateTimeString(string dateTimeString)
        {
            DateTime dt = default(DateTime);
            if (!string.IsNullOrWhiteSpace(dateTimeString))
            {
                DateTime.TryParse(dateTimeString, DateTimeCultureInfo, DateTimeStyles.None, out dt);
            }
            return dt;
        }

        #endregion
    }
}