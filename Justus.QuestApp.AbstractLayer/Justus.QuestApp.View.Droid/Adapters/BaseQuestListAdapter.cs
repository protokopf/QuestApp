using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Entities;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Fragment = Android.Support.V4.App.Fragment;
using Justus.QuestApp.View.Droid.Fragments;

namespace Justus.QuestApp.View.Droid.Adapters
{
    /// <summary>
    /// Base type for quest list adapters
    /// </summary>
    public abstract class BaseQuestListAdapter <TViewModel> : BaseAdapter<Quest>
        where TViewModel : QuestListViewModel
    {
        private readonly TViewModel _listViewModel;

        /// <summary>
        /// Get references to fragment and listViewModel
        /// </summary>
        /// <param name="fragment"></param>
        /// <param name="listViewModel"></param>
        protected BaseQuestListAdapter(TViewModel listViewModel)
        {
            if (listViewModel == null)
            {
                throw new ArgumentNullException(nameof(listViewModel));
            }
            _listViewModel = listViewModel;
        }

        #region BaseAdapter<Quest> overriding

        ///<inheritdoc/>
        public override Quest this[int position] => _listViewModel.CurrentChildren[position];

        ///<inheritdoc/>
        public override int Count => _listViewModel.CurrentChildren.Count;

        ///<inheritdoc/>
        public override long GetItemId(int position)
        {
            return position;
        }

        ///<inheritdoc/>
        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            Quest questData = _listViewModel.CurrentChildren[position];
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