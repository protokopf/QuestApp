using System;
using System.Collections.Generic;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.View.Droid.ViewHolders.Abstracts;

namespace Justus.QuestApp.View.Droid.Adapters.Quests
{
    /// <summary>
    /// Base type for quest list adapters
    /// </summary>
    public abstract class BaseQuestsAdapter <TViewHolder, TViewModel> : RecyclerView.Adapter
        where TViewHolder : PositionedViewHolder
        where TViewModel : IQuestCompositeModel
    {
        protected readonly TViewModel QuestsViewModel;
        protected readonly Dictionary<Android.Views.View, TViewHolder> HoldersDictionary;
        protected readonly Activity ActivityRef;

        /// <summary>
        /// Get references to fragment and questsViewModel
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="questsViewModel"></param>
        protected BaseQuestsAdapter(Activity activity, TViewModel questsViewModel)
        {
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }
            if (questsViewModel == null)
            {
                throw new ArgumentNullException(nameof(questsViewModel));
            }
            ActivityRef = activity;
            QuestsViewModel = questsViewModel;
            HoldersDictionary = new Dictionary<Android.Views.View, TViewHolder>();
        }

        #region RecyclerViewRef.Adapter overriding

        /////<inheritdoc/>
        //public override long GetItemId(int position)
        //{
        //    return position;
        //}

        ///<inheritdoc/>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Android.Views.View row = LayoutInflater.From(parent.Context).Inflate(GetViewId(), parent, false);
            TViewHolder holder = CreateViewHolder(row);
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

        ///// <summary>
        ///// Returns associated view holder with view.
        ///// </summary>
        ///// <param name="view"></param>
        ///// <returns></returns>
        //public TViewHolder GetViewHolderByView(Android.Views.View view)
        //{
        //    TViewHolder holder = null;
        //    HoldersDictionary.TryGetValue(view, out holder);
        //    return holder;
        //}

        /// <summary>
        /// Returns all view holders.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TViewHolder> GetViewHolders()
        {
            return HoldersDictionary.Values;
        }

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