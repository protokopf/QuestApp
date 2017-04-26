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
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace Justus.QuestApp.View.Droid.Abstract.Activities
{
    /// <summary>
    /// Abstract type for activities, which host fragment inside.
    /// </summary>
    [Activity]
    public abstract class AbstractFragmentActivity : AppCompatActivity
    {
        #region AppCompatActivity overriding

        ///<inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AbstractFragmentActivityLayout);
            FragmentManager manager = SupportFragmentManager;
            Fragment fragment = manager.FindFragmentById(Resource.Id.fragmentContainerId);
            if (fragment == null)
            {
                manager.
                    BeginTransaction().
                    Add(Resource.Id.fragmentContainerId, CreateFragment()).
                    Commit();
            }
        }

        #endregion

        /// <summary>
        /// Factory method, that creates fragment.
        /// </summary>
        /// <returns></returns>
        protected abstract Fragment CreateFragment();
    }
}