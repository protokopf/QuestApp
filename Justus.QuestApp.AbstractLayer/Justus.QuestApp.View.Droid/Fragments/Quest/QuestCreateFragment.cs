using Android.OS;
using Justus.QuestApp.View.Droid.Abstract.Fragments.QuestDetails;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;

namespace Justus.QuestApp.View.Droid.Fragments.Quest
{
    /// <summary>
    /// Fragment for editing quest info.
    /// </summary>
    public class QuestCreateFragment : QuestAbstractActionFragment<QuestCreateViewModel>
    {
        private const string ParentIdKey = "ParentIdKey";

        #region Public static methods

        /// <summary>
        /// Creates instance of QuestCrateFragment.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static QuestCreateFragment NewInstance(int parentId)
        {
            QuestCreateFragment fragment = new QuestCreateFragment();
            Bundle arguments = new Bundle();
            arguments.PutInt(ParentIdKey, parentId);

            fragment.Arguments = arguments;

            return fragment;
        }

        #endregion

        #region Fragment overriding

        ///<inehritdoc/>
        protected override void ParseArguments(Bundle arguments)
        {
            if (arguments != null)
            {
                int parentId = arguments.GetInt(ParentIdKey);
                ViewModel.ParentId = parentId;
            }
        }

        #endregion
    }
}