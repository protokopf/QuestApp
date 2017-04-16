using Android.Support.V7.Widget;
using Android.Widget;
using Justus.QuestApp.View.Droid.Adapters.Quests;
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
        protected BaseQuestsAdapter<TViewHolder, TViewModel> QuestsAdapter;

        /// <summary>
        /// Reference to recycler view.
        /// </summary>
        protected RecyclerView RecyclerViewRef;

        #region ISelectable implmentation

        ///<inheritdoc/>
        public virtual void OnSelect()
        {
            QuestsAdapter?.NotifyDataSetChanged();
        } 

        #endregion

        protected void RedrawListView()
        {
            
        }
    }
}