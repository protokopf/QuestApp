using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Support.V4.App;
using Java.Lang;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.View.Droid.Abstract.Fragments.Factories;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace Justus.QuestApp.View.Droid.Adapters.Fragments
{
    /// <summary>
    /// Generic parametrized adapter for viewPager.
    /// </summary>
    public class GenericFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly IList<IFragmentFactory> _fragmentFactories;

        public GenericFragmentPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        /// <summary>
        /// Receives reference to fragment manager and list to fragment factories.
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="fragmentFactories"></param>
        public GenericFragmentPagerAdapter(FragmentManager fm, IList<IFragmentFactory> fragmentFactories) : base(fm)
        {
            fragmentFactories.ThrowIfNull(nameof(fragmentFactories));

            _fragmentFactories = fragmentFactories;
        }

        #region FragmentPagerAdapter overriding

        ///<inehritdoc/>
        public override int Count => _fragmentFactories.Count;

        ///<inehritdoc/>
        public override Fragment GetItem(int position)
        {
            return _fragmentFactories[position]?.GetFragment();
        }

        ///<inehritdoc/>
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(
                _fragmentFactories[position]?.GetFragmentTitle());
        }

        #endregion
    }
}