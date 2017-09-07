using System;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.View.Droid.Abstract.Fragments.Factories;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Fragments.Factories
{
    /// <summary>
    /// Uses constructor parameters for creating fragments.
    /// </summary>
    public class ParametrizedFragmentFactory : IFragmentFactory
    {
        private readonly Func<Fragment> _fragmentInitializer;
        private readonly string _fragmentTitle;

        /// <summary>
        /// Receives fragment initializer and title.
        /// </summary>
        /// <param name="fragmentInitializer"></param>
        /// <param name="fragmentTitle"></param>
        public ParametrizedFragmentFactory(Func<Fragment> fragmentInitializer, string fragmentTitle)
        {
            fragmentInitializer.ThrowIfNull(nameof(fragmentInitializer));

            if (string.IsNullOrWhiteSpace(fragmentTitle))
            {
                throw new ArgumentException("Fragment title should not be empty or whitespace.", nameof(fragmentTitle));
            }
            _fragmentInitializer = fragmentInitializer;
            _fragmentTitle = fragmentTitle;
        }

        #region IFragmentFactory implementation

        ///<inheritdoc/>
        public Fragment GetFragment()
        {
            return _fragmentInitializer();
        }

        ///<inheritdoc/>
        public string GetFragmentTitle()
        {
            return _fragmentTitle;
        }

        #endregion
    }
}