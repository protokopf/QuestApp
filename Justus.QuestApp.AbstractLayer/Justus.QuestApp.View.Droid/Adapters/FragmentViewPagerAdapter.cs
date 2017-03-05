using System.Collections.Generic;
using Android.Support.V4.App;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace Justus.QuestApp.View.Droid.Adapters
{
    /// <summary>
    /// Adapter for tabbed activity.
    /// </summary>
    public class FragmentViewPagerAdapter : FragmentPagerAdapter
    {
        private readonly List<Fragment> _fragments = new List<Fragment>(); 
        private readonly List<string> _fragmentsTitles = new List<string>();

        /// <summary>
        /// Receives reference to fragment manager.
        /// </summary>
        /// <param name="manager"></param>
        public FragmentViewPagerAdapter(FragmentManager manager) : base(manager)
        {
            
        }

        #region FragmentPagerAdapter overriding

        ///<inheritdoc/>
        public override int Count => _fragments.Count;

        ///<inheritdoc/>
        public override Fragment GetItem(int position)
        {
            return _fragments[position];
        }

        ///<inheritdoc/>
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(_fragmentsTitles[position]);
        }

        #endregion

        /// <summary>
        /// Add fragment and its title.
        /// </summary>
        /// <param name="fragment"></param>
        /// <param name="title"></param>
        public void AddFragment(Fragment fragment, string title)
        {
            _fragments.Add(fragment);
            _fragmentsTitles.Add(title);
        }
    }

}