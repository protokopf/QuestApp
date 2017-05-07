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
using Justus.QuestApp.View.Droid.Abstract.Activities;
using Justus.QuestApp.View.Droid.Fragments;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Activities
{
    /// <summary>
    /// Activity for creating quest.
    /// </summary>
    [Activity(Label= "@string/QuestCreateActivityLabel")]
    public class QuestInfoActivity : AbstractFragmentActivity
    {
        #region AbstractFragmentActivity overriding

        ///<inhertidoc/>
        protected override Fragment CreateFragment()
        {
            return new QuestCreateFragment();
        }

        #endregion
    }
}