using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Toolbar = Android.Support.V7.Widget.Toolbar;

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

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.questCreateToolbar));

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