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
using Justus.QuestApp.View.Droid.Fragments;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Adapters
{
    public class ActiveQuestListAdapter : BaseQuestListAdapter<ActiveQuestListViewModel>
    {
        private readonly ActiveQuestsFragment _fragment;
        private readonly Dictionary<Android.Views.View, ActiveQuestItemViewHolder> _holdersDictionary; 

        /// <summary>
        /// Get references to fragment and viewModel
        /// </summary>
        /// <param name="fragment"></param>
        /// <param name="listViewModel"></param>
        public ActiveQuestListAdapter(ActiveQuestsFragment fragment, ActiveQuestListViewModel listViewModel) : base(listViewModel)
        {
            if (fragment == null)
            {
                throw new NullReferenceException("ActiveQuestListAdapter.ctor fragment is null!");
            }
            _fragment = fragment;
            _holdersDictionary = new Dictionary<Android.Views.View, ActiveQuestItemViewHolder>();
        }

        #region BaseQuestListAdapter overriding

        ///<inheritdoc/>
        protected override Android.Views.View ConstructViewFromQuest(int position, Android.Views.View view, ViewGroup parent, Quest quest)
        {
            ActiveQuestItemViewHolder viewHolder = null;
            if (view == null)
            {
                view = _fragment.Activity.LayoutInflater.Inflate(Resource.Layout.QuestListItemHeader, null, false);
                viewHolder = new ActiveQuestItemViewHolder(view, position);
            }
            else
            {
                viewHolder = _holdersDictionary[view];
            }

            FillHolder(viewHolder, quest, position);

            if (!_holdersDictionary.ContainsKey(view))
            {
                _holdersDictionary.Add(view,viewHolder);
            }

            return view;
        }

        #endregion

        /// <summary>
        /// Releases all resources.
        /// </summary>

        public ActiveQuestItemViewHolder GetHolderByView(Android.Views.View view)
        {
            ActiveQuestItemViewHolder holder = null;
            _holdersDictionary.TryGetValue(view, out holder);
            return holder;
        }

        private void FillHolder(ActiveQuestItemViewHolder viewHolder, Quest quest, int position)
        {
            viewHolder.Title.Text = quest.Title;
            viewHolder.TimeLeft.Text = FormLeftTime(quest.Deadline);
            viewHolder.Progress.Progress = 25;
            viewHolder.ChildrenButton.Enabled = quest.Children != null;
            viewHolder.ItemPosition = position;
        }

        private string FormLeftTime(DateTime deadLine)
        {
            TimeSpan s = deadLine - DateTime.Now;
            return $"{s.Hours}:{s.Minutes}:{s.Seconds}";
        }
    }
}