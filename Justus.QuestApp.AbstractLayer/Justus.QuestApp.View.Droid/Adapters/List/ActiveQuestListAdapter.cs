using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Views;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Fragments;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.List
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
                throw new NullReferenceException(nameof(fragment));
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
                view = _fragment.Activity.LayoutInflater.Inflate(Resource.Layout.ActiveQuestListItemHeader, null, false);
                viewHolder = new ActiveQuestItemViewHolder(view, position);
                _holdersDictionary.Add(view, viewHolder);
            }
            else
            {
                viewHolder = _holdersDictionary[view];
            }

            FillHolder(viewHolder, quest, position);
            return view;
        }

        #endregion

        public ActiveQuestItemViewHolder GetHolderByView(Android.Views.View view)
        {
            ActiveQuestItemViewHolder holder = null;
            _holdersDictionary.TryGetValue(view, out holder);
            return holder;
        }

        private void FillHolder(ActiveQuestItemViewHolder viewHolder, Quest quest, int position)
        {           
            viewHolder.Title.Text = quest.Title;
            viewHolder.Description.Text = quest.Description;
            viewHolder.TimeLeft.Text = FormLeftTime(quest.Deadline);
            viewHolder.Progress.Progress = ListViewModel.CountProgress(quest);
            viewHolder.ChildrenButton.Enabled = quest.Children != null;
            viewHolder.ItemPosition = position;

            switch (quest.CurrentState)
            {
                case QuestState.Done:
                    viewHolder.Title.SetTextColor(Color.Green);
                    break;
                case QuestState.Failed:
                    viewHolder.Title.SetTextColor(Color.Red);
                    break;
                case QuestState.Idle:
                    viewHolder.Title.SetTextColor(Color.Gray);
                    break;
                default:
                    viewHolder.Title.SetTextColor(Color.Orange);
                    break;
            }
        }

        private string FormLeftTime(DateTime deadLine)
        {
            TimeSpan s = deadLine - DateTime.Now;
            return $"{s.Hours}:{s.Minutes}:{s.Seconds}";
        }
    }
}