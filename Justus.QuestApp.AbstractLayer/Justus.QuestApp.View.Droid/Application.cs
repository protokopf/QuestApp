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

namespace Justus.QuestApp.View.Droid
{
    [Application(Theme="@android:style/Theme.Holo.Light")]
    public class Application : Android.App.Application
    {
        /// <summary>
        /// Need for base class.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        public Application(IntPtr javaReference, JniHandleOwnership transfer):
            base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
        }


    }
}