using Android.Widget;
using Justus.QuestApp.View.Droid.Adapters.List;
using Justus.QuestApp.View.Droid.ViewHolders.Abstracts;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments.Abstracts
{
    /// <summary>
    /// Base class for quest list fragments.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TViewHolder"></typeparam>
    public abstract class BaseQuestsFragment<TViewModel, TViewHolder> : BaseFragment<TViewModel>, ISelectable
        where TViewModel : QuestListViewModel
        where TViewHolder : PositionedViewHolder
    {
        /// <summary>
        /// Adapter for list view.
        /// </summary>
        protected BaseQuestListAdapter<TViewHolder, TViewModel> QuestListAdapter;

        /// <summary>
        /// Reference to list view.
        /// </summary>
        protected ListView QuestListView;

        #region ISelectable implmentation

        ///<inheritdoc/>
        public virtual void OnSelect()
        {
            QuestListAdapter?.NotifyDataSetChanged();
        } 

        #endregion

        /// <summary>
        /// Redraws list view.
        /// </summary>
        protected void RedrawListView()
        {
            if (QuestListView != null && QuestListAdapter != null)
            {
                QuestListView.Adapter = QuestListAdapter;
            }
        }
    }
}