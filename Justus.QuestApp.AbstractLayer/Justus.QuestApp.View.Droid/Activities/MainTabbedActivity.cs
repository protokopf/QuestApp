using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Adapters;
using Justus.QuestApp.View.Droid.Fragments;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Justus.QuestApp.View.Droid.Activities
{

    /// <summary>
    /// Main activity, that hosts fragments in separates tabs.
    /// </summary>
    [Activity(Label = "@string/MainActivityLabel", MainLauncher = true)]
    public class MainTabbedActivity : BaseTabbedActivity
    {
        private FragmentViewPagerAdapter _fragmentAdapter;
        private CoordinatorLayout _coordinatorLayout;

        #region BaseTabbedActivity overriding

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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainActivityMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            switch (id)
            {
                case Resource.Id.menuFirstOption:
                    Snackbar.Make(_coordinatorLayout, "First menu item clicked!",Snackbar.LengthShort).Show();
                    break;
                case Resource.Id.menuSecondOption:
                    Snackbar.Make(_coordinatorLayout, "Second menu item clicked!", Snackbar.LengthShort).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _coordinatorLayout = FindViewById<CoordinatorLayout>(Resource.Id.mainActivityCoordinatorLayout);
        }

        private void SetupViewPager(ViewPager viewPager)
        {
            _fragmentAdapter = new FragmentViewPagerAdapter(SupportFragmentManager);

            _fragmentAdapter.AddFragment(new ActiveQuestsFragment(),  Resources.GetString(Resource.String.ActiveQuestsLabel));
            _fragmentAdapter.AddFragment(new NotImplementedFragment(), Resources.GetString(Resource.String.IdleQuestsLabel));
            _fragmentAdapter.AddFragment(new NotImplementedFragment(), Resources.GetString(Resource.String.FinishedQuestsLabel));

            viewPager.Adapter = _fragmentAdapter;
        }
    }
}