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
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Abstract.Fragments.Factories
{
    /// <summary>
    /// Represents interface for fragments factories, that also returns fragment titles.
    /// </summary>
    public interface IFragmentFactory
    {
        /// <summary>
        /// Returns new instance of fragment.
        /// </summary>
        /// <returns></returns>
        Fragment GetFragment();

        /// <summary>
        /// Returns title of fragment.
        /// </summary>
        /// <returns></returns>
        string GetFragmentTitle();
    }
}