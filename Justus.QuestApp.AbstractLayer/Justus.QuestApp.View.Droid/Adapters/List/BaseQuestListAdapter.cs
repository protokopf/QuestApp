using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.List
{
    /// <summary>
    /// Base type for quest list adapters
    /// </summary>
    public abstract class BaseQuestListAdapter <TViewModel, TViewHolder> : BaseAdapter<Quest>
        where TViewModel : QuestListViewModel
        where TViewHolder : PositionedViewHolder
    {
        protected readonly TViewModel ListViewModel;
        protected readonly Dictionary<Android.Views.View, TViewHolder> HoldersDictionary; 

        /// <summary>
        /// Get references to fragment and listViewModel
        /// </summary>
        /// <param name="listViewModel"></param>
        protected BaseQuestListAdapter(TViewModel listViewModel)
        {
            if (listViewModel == null)
            {
                throw new ArgumentNullException(nameof(listViewModel));
            }
            ListViewModel = listViewModel;
            HoldersDictionary = new Dictionary<Android.Views.View, TViewHolder>();
        }

        #region BaseAdapter<Quest> overriding

        ///<inheritdoc/>
        public override Quest this[int position] => ListViewModel.CurrentChildren[position];

        ///<inheritdoc/>
        public override int Count => ListViewModel.CurrentChildren.Count;

        ///<inheritdoc/>
        public override long GetItemId(int position)
        {
            return position;
        }

        ///<inheritdoc/>
        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            Quest questData = ListViewModel.CurrentChildren[position];
            TViewHolder viewHolder = null;
            if (convertView == null)
            {
                convertView = InflateView();
                viewHolder = CreateViewHolder(convertView, position);
                HoldersDictionary.Add(convertView, viewHolder);
            }
            else
            {
                viewHolder = HoldersDictionary[convertView];
            }
            FillViewHolder(viewHolder, questData, position);
            return convertView;
        }

        #endregion

        /// <summary>
        /// Returns associated view holder with view.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public TViewHolder GetViewHolderByView(Android.Views.View view)
        {
            TViewHolder holder = null;
            HoldersDictionary.TryGetValue(view, out holder);
            return holder;
        }

        /// <summary>
        /// Inflate view.
        /// </summary>
        /// <returns></returns>
        protected abstract Android.Views.View InflateView();

        /// <summary>
        /// Creates view holder for view.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        protected abstract TViewHolder CreateViewHolder(Android.Views.View view, int position);

        /// <summary>
        /// Fill view holder with data from quest entity.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="questData"></param>
        /// <param name="position"></param>
        protected abstract void FillViewHolder(TViewHolder holder, Quest questData, int position);
    }
}