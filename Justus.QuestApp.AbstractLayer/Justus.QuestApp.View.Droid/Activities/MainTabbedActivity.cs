using System.Collections.Generic;
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
        private FloatingActionButton _floatingActionButton;

        private readonly List<ISelectable> _selectables = new List<ISelectable>();
        private readonly List<IFabManager> _fabManagers = new List<IFabManager>();

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
            _selectables.Clear();
            _fabManagers.Clear();

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.mainActivityViewPager);

            SetupViewPager(viewPager);

            viewPager.PageSelected += PageChanged;

            return viewPager;
        }

        ///<inheritdoc/>
        protected override void SetView()
        {
            SetContentView(Resource.Layout.MainActivityLayout);
            _coordinatorLayout = FindViewById<CoordinatorLayout>(Resource.Id.mainActivityCoordinatorLayout);
            _floatingActionButton = FindViewById<FloatingActionButton>(Resource.Id.floatButtonId);
            _floatingActionButton.Visibility = ViewStates.Gone;
        }

        #endregion

        #region Private methods

        private void SetupViewPager(ViewPager viewPager)
        {
            _fragmentAdapter = new FragmentViewPagerAdapter(SupportFragmentManager);

            ActiveQuestsFragment active = new ActiveQuestsFragment();
            ResultQuestsFragment result = new ResultQuestsFragment();
            AvailableQuestsFragment available = new AvailableQuestsFragment();

            _fragmentAdapter.AddFragment(active,  Resources.GetString(Resource.String.ActiveQuestsLabel));
            _fragmentAdapter.AddFragment(result, Resources.GetString(Resource.String.ResultQuestsLabel));
            _fragmentAdapter.AddFragment(available, Resources.GetString(Resource.String.IdleQuestsLabel));

            _selectables.Add(active);
            _selectables.Add(result);
            _selectables.Add(available);

            _fabManagers.Add(active);
            _fabManagers.Add(result);
            _fabManagers.Add(available);

            viewPager.Adapter = _fragmentAdapter;
        }

        private void PageChanged(object sender, ViewPager.PageSelectedEventArgs e)
        {
            int position = e.Position;
            if (position >= 0 && position < _selectables.Count)
            {
                _selectables[position].OnSelect();
            }
            if (position >= 0 && position < _fabManagers.Count)
            {
                _fabManagers[position].Manage(_floatingActionButton);
            }
        }

        #endregion
    }
}