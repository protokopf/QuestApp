using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace Justus.QuestApp.View.Droid.Abstract.Activities
{
    public abstract class BaseTabbedActivity : AppCompatActivity
    {
        #region Protected fields

        protected Toolbar Toolbar;
        protected TabLayout TabLayout;
        protected ViewPager ViewPager;

        #endregion

        #region Activity overriding

        ///<inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetView();

            Toolbar = InitializeToolbar();
            ViewPager = InitializeViewPager();
            TabLayout = InitializeTabLayout();
        }

        #endregion

        #region Abstract methods

        protected abstract Toolbar InitializeToolbar();

        protected abstract TabLayout InitializeTabLayout();

        protected abstract ViewPager InitializeViewPager();

        protected abstract void SetView();

        #endregion
    }
}