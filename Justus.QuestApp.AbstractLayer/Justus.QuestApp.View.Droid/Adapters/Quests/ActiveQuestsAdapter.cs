using System;
using Android.App;
using Android.Graphics;
using Android.Views;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Abstract.Adapters;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.View.Droid.ViewHolders.QuestItem;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.Quests
{
    /// <summary>
    /// Adapter for active quests.
    /// </summary>
    public class ActiveQuestsAdapter : BaseQuestsAdapter<ActiveQuestViewHolder, ActiveQuestListViewModel>
    {
        private readonly string _startLabel;
        private readonly string _restartLabel;

        /// <summary>
        /// Get references to activity and viewModel
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="questsViewModel"></param>
        /// <param name="clickManager"></param>
        public ActiveQuestsAdapter(Activity activity, 
            ActiveQuestListViewModel questsViewModel,
            IViewHolderClickManager<ActiveQuestViewHolder> clickManager) 
            : base(activity,questsViewModel, clickManager)
        {
            _startLabel = activity.GetString(Resource.String.StartButtonText);
            _restartLabel = activity.GetString(Resource.String.RestartButtonText);
        }

        #region BaseQuestsAdapter overriding

        ///<inheritdoc/>
        protected override int GetViewId()
        {
           return Resource.Layout.ActiveQuestListItemHeader;
        }

        ///<inheritdoc/>
        protected override ActiveQuestViewHolder CreateViewHolder(Android.Views.View view)
        {
            return new ActiveQuestViewHolder(view);
        }

        ///<inheritdoc/>
        protected override void FillViewHolder(ActiveQuestViewHolder holder, Quest questData, int position)
        {
            holder.Collapse();
            holder.Title.Text = questData.Title;
            holder.Description.Text = questData.Description;
            holder.TimeLeft.Text = FormLeftTime(questData.Deadline);
            holder.Progress.Progress = QuestsViewModel.CountProgress(questData);
            holder.ChildrenButton.Enabled = questData.Children != null;
            holder.ItemPosition = position;

            switch (questData.CurrentState)
            {
                case QuestState.Done:
                    holder.Title.SetTextColor(Color.Green);
                    HandleButtonsForDone(holder, questData);
                    break;
                case QuestState.Failed:
                    holder.Title.SetTextColor(Color.Red);
                    HandleButtonsForFailed(holder, questData);
                    break;
                case QuestState.Idle:
                    holder.Title.SetTextColor(Color.Gray);
                    HandleButtonsForIdle(holder, questData);
                    break;
                case QuestState.Progress:
                    holder.Title.SetTextColor(Color.Orange);
                    HandleButtonsForProgress(holder, questData);
                    break;
            }
        }

        #endregion

        #region Private methods

        private void HandleButtonsForProgress(ActiveQuestViewHolder holder, Quest questData)
        {
            ViewStates doneFailState = questData.Children != null && questData.Children.Count != 0 ? ViewStates.Gone : ViewStates.Visible;

            holder.StartButton.Visibility = ViewStates.Gone;
            holder.CancelButton.Visibility = ViewStates.Visible;
            holder.DeleteButton.Visibility = ViewStates.Visible;
            holder.DoneButton.Visibility = doneFailState;
            holder.FailButton.Visibility = doneFailState;
        }

        private void HandleButtonsForIdle(ActiveQuestViewHolder holder, Quest questData)
        {
            holder.StartButton.Text = _startLabel;
            holder.StartButton.Visibility = ViewStates.Visible;

            holder.CancelButton.Visibility = ViewStates.Gone;
            holder.DeleteButton.Visibility = ViewStates.Visible;
            holder.DoneButton.Visibility = ViewStates.Gone;
            holder.FailButton.Visibility = ViewStates.Gone;
        }

        private void HandleButtonsForFailed(ActiveQuestViewHolder holder, Quest questData)
        {
            holder.StartButton.Text = _restartLabel;
            holder.StartButton.Visibility = ViewStates.Visible;

            holder.CancelButton.Visibility = ViewStates.Gone;
            holder.DeleteButton.Visibility = ViewStates.Visible;
            holder.DoneButton.Visibility = ViewStates.Gone;
            holder.FailButton.Visibility = ViewStates.Gone;
        }

        private void HandleButtonsForDone(ActiveQuestViewHolder holder, Quest questData)
        {
            holder.StartButton.Visibility = ViewStates.Gone;
            holder.CancelButton.Visibility = ViewStates.Gone;
            holder.DeleteButton.Visibility = ViewStates.Visible;
            holder.DoneButton.Visibility = ViewStates.Gone;
            holder.FailButton.Visibility = ViewStates.Gone;
        }

        private string FormLeftTime(DateTime deadLine)
        {
            TimeSpan s = deadLine - DateTime.Now;
            return $"{s.Hours}:{s.Minutes}:{s.Seconds}";
        }

        #endregion
    }
}