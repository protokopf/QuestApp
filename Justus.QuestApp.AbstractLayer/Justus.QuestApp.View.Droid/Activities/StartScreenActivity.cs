using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.Activities
{
    /// <summary>
    /// Activity, which will be shown during loading.
    /// </summary>
    [Activity(Label = "@string/LoadingActivityLabel", MainLauncher = true)]
    public class StartScreenActivity : AppCompatActivity
    {
        #region AppCompatActivity overriding

        ///<inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoadingScreenLayout);
        }

        ///<inheritdoc/>
        protected override void OnStart()
        {
            base.OnStart();
            InitializeComponents();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes components from service locator.
        /// </summary>
        private async void InitializeComponents()
        {
            await Task.Run(() => Thread.Sleep(3000));
            StartActivity(typeof(MainTabbedActivity));
        }

        #endregion
    }
}