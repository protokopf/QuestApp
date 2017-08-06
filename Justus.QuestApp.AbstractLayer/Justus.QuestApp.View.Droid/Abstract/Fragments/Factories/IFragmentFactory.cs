using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Abstract.Fragments.Factories
{
    /// <summary>
    /// Represents interface for fragments factories, that also returns fragment titles.
    /// </summary>
    public interface IFragmentFactory
    {
        /// <summary>
        /// Returns new instance of fragment.
        /// </summary>
        /// <returns></returns>
        Fragment GetFragment();

        /// <summary>
        /// Returns title of fragment.
        /// </summary>
        /// <returns></returns>
        string GetFragmentTitle();
    }
}