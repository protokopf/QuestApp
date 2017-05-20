using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.Abstract.Fragments
{
    /// <summary>
    /// Represents interface for types, which manages FAB.
    /// </summary>
    public interface IFabDecorator
    {
        /// <summary>
        /// Manages given FAB.
        /// </summary>
        /// <param name="fab"></param>
        void Decorate(FloatingActionButton fab);
    }

}