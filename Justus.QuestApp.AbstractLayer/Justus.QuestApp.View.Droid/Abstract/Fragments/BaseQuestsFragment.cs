using Android.Support.V7.Widget;
using Justus.QuestApp.View.Droid.Abstract.Adapters;
using Justus.QuestApp.View.Droid.Abstract.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Abstract.Fragments
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

        #region ISelectable implementation

        ///<inheritdoc/>
        public virtual void OnSelect()
        {
            RedrawQuests();
        }

        ///<inheritdoc/>
        public virtual void OnUnselect()
        {
            
        }

        #endregion

        protected void RedrawQuests()
        {
            QuestsAdapter?.NotifyDataSetChanged();
        }
    }
}