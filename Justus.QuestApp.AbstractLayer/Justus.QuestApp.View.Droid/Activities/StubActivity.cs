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
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Adapters;
using Justus.QuestApp.View.Droid.Fragments;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Justus.QuestApp.View.Droid.Activities
{
    [Activity(Label = "StubActivity", MainLauncher = true)]
    public class StubActivity : AppCompatActivity
    {
        private Toolbar _toolBar;
        private TabLayout _tabLayout;
        private ViewPager _viewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TabbedActivityLayout);

            _toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(_toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            SetupViewPager(_viewPager);

            _tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            _tabLayout.SetupWithViewPager(_viewPager);

        }

        #region Private methods

        private void SetupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(this.SupportFragmentManager);

            adapter.AddFragment(new StubFragment(), "ONE");
            adapter.AddFragment(new StubFragment(), "TWO");
            adapter.AddFragment(new StubFragment(), "THREE");

            viewPager.Adapter = adapter;
        }

        #endregion
    }
}