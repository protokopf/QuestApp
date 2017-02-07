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

namespace Justus.QuestApp.View.Droid.Fragments.Abstracts
{
    /// <summary>
    /// Interface for types, which can be updated.
    /// </summary>
    public interface IStateResettable
    {
        /// <summary>
        /// Updates.
        /// </summary>
        void ResetState();
    }
}