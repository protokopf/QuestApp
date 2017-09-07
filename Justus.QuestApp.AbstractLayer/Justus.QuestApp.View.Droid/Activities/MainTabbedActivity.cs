using System.Collections.Generic;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Justus.QuestApp.View.Droid.Abstract.Activities;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Abstract.Fragments.Factories;
using Justus.QuestApp.View.Droid.Adapters.Fragments;
using Justus.QuestApp.View.Droid.Fragments;
using Justus.QuestApp.View.Droid.Fragments.Factories;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Justus.QuestApp.View.Droid.Activities
{
    /// <summary>
    /// Main activity, that hosts fragments in separate tabs.
    /// </summary>
    [Activity(Label = "@string/MainActivityLabel")]
    public class MainTabbedActivity : BaseTabbedActivity
    {
        private CoordinatorLayout _coordinatorLayout;
        private FloatingActionButton _floatingActionButton;

        private ISelectable _lastSelected = null;

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
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.mainActivityViewPager);

            viewPager.Adapter = new GenericFragmentPagerAdapter(SupportFragmentManager, GetFragmentFactories());
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

        private IList<IFragmentFactory> GetFragmentFactories()
        {
            return new List<IFragmentFactory>
            {
                new ParametrizedFragmentFactory(() => new ActiveQuestsFragment(),
                    Resources.GetString(Resource.String.ActiveQuestsLabel)),
                new ParametrizedFragmentFactory(() => new ResultQuestsFragment(),
                    Resources.GetString(Resource.String.ResultQuestsLabel)),
                new ParametrizedFragmentFactory(() => new AvailableQuestsFragment(),
                    Resources.GetString(Resource.String.IdleQuestsLabel))
            };
        }

        private void PageChanged(object sender, ViewPager.PageSelectedEventArgs e)
        { 
            _lastSelected?.OnUnselect();

            int position = e.Position;

            if (_floatingActionButton.IsShown)
            {
                _floatingActionButton.Hide();
            }

            Fragment current = SupportFragmentManager.Fragments[position];

            (current as IFabDecorator)?.Decorate(_floatingActionButton);
            (_lastSelected = current as ISelectable)?.OnSelect();
        }

        #endregion
    }
}