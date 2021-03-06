using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Justus.QuestApp.AbstractLayer.Helpers.Behaviours;
using Justus.QuestApp.ModelLayer.Helpers;

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
            await Task.Run(() => InitializeWork());
            StartActivity(typeof(MainTabbedActivity));
            this.Finish();
        }

        private void InitializeWork()
        {
            IEnumerable<IInitializable> implementations = ServiceLocator.ResolveAll<IInitializable>();
            foreach (IInitializable toInit in implementations)
            {
                if (!toInit.IsInitialized())
                {
                    toInit.Initialize();
                }       
            }
        }

        #endregion
    }
}