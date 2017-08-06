using System;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Abstract.ViewHolders;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Abstract.Adapters
{
    /// <summary>
    /// Base type for quest list adapters
    /// </summary>
    public abstract class BaseQuestsAdapter <TViewHolder, TViewModel> : RecyclerView.Adapter
        where TViewHolder : PositionedViewHolder
        where TViewModel : QuestListViewModel
    {
        protected readonly TViewModel QuestsViewModel;
        protected readonly Activity ActivityRef;
        protected readonly IViewHolderClickManager<TViewHolder> ClickManager;

        /// <summary>
        /// Get references to fragment and questsViewModel
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="questsViewModel"></param>
        /// <param name="clickManager"></param>
        protected BaseQuestsAdapter(Activity activity, TViewModel questsViewModel, IViewHolderClickManager<TViewHolder> clickManager)
        {
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }
            if (questsViewModel == null)
            {
                throw new ArgumentNullException(nameof(questsViewModel));
            }
            if (clickManager == null)
            {
                throw new ArgumentNullException(nameof(clickManager));
            }
            ActivityRef = activity;
            QuestsViewModel = questsViewModel;
            ClickManager = clickManager;
        }

        #region RecyclerViewRef.Adapter overriding


        ///<inheritdoc/>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Android.Views.View row = LayoutInflater.From(parent.Context).Inflate(GetViewId(), parent, false);
            TViewHolder holder = CreateViewHolder(row);
            ClickManager.BindClickListeners(holder);
            return holder;
        }

        ///<inheritdoc/>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TViewHolder myHolder = holder as TViewHolder;
            FillViewHolder(myHolder, QuestsViewModel.Leaves[position],position);
        }

        ///<inheritdoc/>
        public override int ItemCount => QuestsViewModel.Leaves.Count;


        #endregion

        /// <summary>
        /// Returns id of view.
        /// </summary>
        /// <returns></returns>
        protected abstract int GetViewId();

        /// <summary>
        /// Creates view holder for view.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        protected abstract TViewHolder CreateViewHolder(Android.Views.View view);

        /// <summary>
        /// Fill view holder with data from quest entity.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="questData"></param>
        /// <param name="position"></param>
        protected abstract void FillViewHolder(TViewHolder holder, Quest questData, int position);
    }
}