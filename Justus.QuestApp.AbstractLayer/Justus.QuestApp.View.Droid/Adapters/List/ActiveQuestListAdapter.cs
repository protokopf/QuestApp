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
    public class ActiveQuestListAdapter : BaseQuestListAdapter<ActiveQuestListViewModel, ActiveQuestItemViewHolder>
    {
        private readonly ActiveQuestsFragment _fragment;

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
        }

        #region BaseQuestListAdapter overriding

        ///<inheritdoc/>
        protected override Android.Views.View InflateView()
        {
           return _fragment.Activity.LayoutInflater.Inflate(Resource.Layout.ActiveQuestListItemHeader, null, false);
        }

        ///<inheritdoc/>
        protected override ActiveQuestItemViewHolder CreateViewHolder(Android.Views.View view, int position)
        {
            return new ActiveQuestItemViewHolder(view, position);
        }

        ///<inheritdoc/>
        protected override void FillViewHolder(ActiveQuestItemViewHolder holder, Quest questData, int position)
        {
            holder.Title.Text = questData.Title;
            holder.Description.Text = questData.Description;
            holder.TimeLeft.Text = FormLeftTime(questData.Deadline);
            holder.Progress.Progress = ListViewModel.CountProgress(questData);
            holder.ChildrenButton.Enabled = questData.Children != null;
            holder.ItemPosition = position;

            switch (questData.CurrentState)
            {
                case QuestState.Done:
                    holder.Title.SetTextColor(Color.Green);
                    break;
                case QuestState.Failed:
                    holder.Title.SetTextColor(Color.Red);
                    break;
                case QuestState.Idle:
                    holder.Title.SetTextColor(Color.Gray);
                    break;
                default:
                    holder.Title.SetTextColor(Color.Orange);
                    break;
            }
        }

        #endregion


        private string FormLeftTime(DateTime deadLine)
        {
            TimeSpan s = deadLine - DateTime.Now;
            return $"{s.Hours}:{s.Minutes}:{s.Seconds}";
        }


    }
}