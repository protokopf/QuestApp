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
        private FragmentViewPagerAdapter mFragmentAdapter;
        private CoordinatorLayout mCoordinatorLayout;

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
                    Snackbar.Make(mCoordinatorLayout, "First menu item clicked!",Snackbar.LengthShort).Show();
                    break;
                case Resource.Id.menuSecondOption:
                    Snackbar.Make(mCoordinatorLayout, "Second menu item clicked!", Snackbar.LengthShort).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mCoordinatorLayout = FindViewById<CoordinatorLayout>(Resource.Id.mainActivityCoordinatorLayout);
        }

        private void SetupViewPager(ViewPager viewPager)
        {
            mFragmentAdapter = new FragmentViewPagerAdapter(SupportFragmentManager);

            mFragmentAdapter.AddFragment(new StubQuestListFragment(),  Resources.GetString(Resource.String.ActiveQuestsLabel));
            mFragmentAdapter.AddFragment(new NotImplementedFragment(), Resources.GetString(Resource.String.IdleQuestsLabel));
            mFragmentAdapter.AddFragment(new NotImplementedFragment(), Resources.GetString(Resource.String.FinishedQuestsLabel));

            viewPager.Adapter = mFragmentAdapter;
        }
    }
}