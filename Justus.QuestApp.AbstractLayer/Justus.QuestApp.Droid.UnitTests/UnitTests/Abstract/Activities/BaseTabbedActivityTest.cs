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
using Justus.QuestApp.View.Droid.Abstract.Activities;
using NUnit.Framework;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Justus.QuestApp.Droid.UnitTests.UnitTests.Abstract.Activities
{
    [TestFixture]
    class BaseTabbedActivityTest
    {
        private class MockBaseTabbedActivity : BaseTabbedActivity
        {
            protected override Toolbar InitializeToolbar()
            {
                return OnInitializeToolbar?.Invoke();
            }

            protected override TabLayout InitializeTabLayout()
            {
                return OnInitializeTabLayout?.Invoke();
            }

            protected override ViewPager InitializeViewPager()
            {
                return OnInitializeViewPager?.Invoke();
            }

            protected override void SetView()
            {
                OnSetView?.Invoke();
            }

            public event Action OnSetView;

            public event Func<Toolbar> OnInitializeToolbar;
            public event Func<TabLayout> OnInitializeTabLayout;
            public event Func<ViewPager> OnInitializeViewPager;
        }

        [Test]
        public void OnCreateTest()
        {
            //Arrange
            MockBaseTabbedActivity activity = new MockBaseTabbedActivity();

            //Act

            //Assert
        }
    }
}