using System;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.List
{
    /// <summary>
    /// Base type for quest list adapters
    /// </summary>
    public abstract class BaseQuestListAdapter <TViewModel> : BaseAdapter<Quest>
        where TViewModel : QuestListViewModel
    {
        protected readonly TViewModel ListViewModel;

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
            return ConstructViewFromQuest(position, convertView, parent, questData);
        }

        #endregion

        /// <summary>
        /// Construct view from quest data.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="view"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        protected abstract Android.Views.View ConstructViewFromQuest(int position, Android.Views.View view, ViewGroup parent, Quest quest);
    }
}