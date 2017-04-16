using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Justus.QuestApp.View.Droid.Abstract.Activities;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Adapters;
using Justus.QuestApp.View.Droid.Fragments;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Justus.QuestApp.View.Droid.Activities
{

    /// <summary>
    /// Main activity, that hosts fragments in separates tabs.
    /// </summary>
    [Activity(Label = "@string/MainActivityLabel"/*, MainLauncher = true*/)]
    public class MainTabbedActivity : BaseTabbedActivity
    {
        private FragmentViewPagerAdapter _fragmentAdapter;
        private CoordinatorLayout _coordinatorLayout;

        #region BaseTabbedActivity overriding

        ///<inheritdoc/>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainActivityMenu, menu);
            return true;
        }

        ///<inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            switch (id)
            {
                case Resource.Id.menuFirstOption:
                    Snackbar.Make(_coordinatorLayout, "First menu item clicked!", Snackbar.LengthShort).Show();
                    break;
                case Resource.Id.menuSecondOption:
                    Snackbar.Make(_coordinatorLayout, "Second menu item clicked!", Snackbar.LengthShort).Show();
                    break;
                case Resource.Id.menuExit:
                    Snackbar.Make(_coordinatorLayout, "No exit, son of a bitch.", Snackbar.LengthShort).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        ///<inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _coordinatorLayout = FindViewById<CoordinatorLayout>(Resource.Id.mainActivityCoordinatorLayout);
            ViewPager.PageSelected += PageChanged;
        }

        ///<inheritdoc/>
        protected override void OnDestroy()
        {
            ViewPager.PageSelected -= PageChanged;
            base.OnDestroy();
        }

        ///<inheritdoc/>
        protected override Toolbar InitializeToolbar()
        {
            Toolbar toolBar = FindViewById<Toolbar>(Resource.Id.mainActivityToolbar);
            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            return toolBar;
        }

        ///<inheritdoc/>
        protected override TabLayout InitializeTabLayout()
        {
            TabLayout tabLayout = FindViewById<TabLayout>(Resource.Id.mainActivityTabLayout);
            tabLayout.SetupWithViewPager(ViewPager);
            return tabLayout;

        }

        ///<inheritdoc/>
        protected override ViewPager InitializeViewPager()
        {
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.mainActivityViewPager);
            SetupViewPager(viewPager);
            return viewPager;
        }

        ///<inheritdoc/>
        protected override void SetView()
        {
            SetContentView(Resource.Layout.MainActivityLayout);
        }

        #endregion

        #region Private methods

        private void SetupViewPager(ViewPager viewPager)
        {
            _fragmentAdapter = new FragmentViewPagerAdapter(SupportFragmentManager);

            _fragmentAdapter.AddFragment(new ActiveQuestsFragment(),  Resources.GetString(Resource.String.ActiveQuestsLabel));
            _fragmentAdapter.AddFragment(new ResultQuestsFragment(), Resources.GetString(Resource.String.ResultQuestsLabel));
            _fragmentAdapter.AddFragment(new AvailableQuestsFragment(), Resources.GetString(Resource.String.IdleQuestsLabel));

            viewPager.Adapter = _fragmentAdapter;
        }

        private void PageChanged(object sender, ViewPager.PageSelectedEventArgs e)
        {
            ISelectable updateable = _fragmentAdapter.GetItem(e.Position) as ISelectable;
            if(updateable != null)
            {
                updateable.OnSelect();
            }

        }

        #endregion
    }
}