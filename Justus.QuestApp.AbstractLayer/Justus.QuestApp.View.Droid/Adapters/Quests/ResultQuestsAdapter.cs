using Android.App;
using Android.Graphics;
using Android.Views;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Abstract.Adapters;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.Quests
{
    /// <summary>
    /// Adapter for providing result quest list.
    /// </summary>
    public class ResultQuestsAdapter : BaseQuestsAdapter<ResultQuestViewHolder, ResultsQuestListViewModel>
    {
        private readonly string _failedStatus;
        private readonly string _doneStatus;

        /// <summary>
        /// Receives references to activity and list view model.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="questsViewModel"></param>
        /// <param name="clickManager"></param>
        public ResultQuestsAdapter(Activity activity, 
            ResultsQuestListViewModel questsViewModel,
            IViewHolderClickManager<ResultQuestViewHolder> clickManager) 
            : base(activity,questsViewModel, clickManager)
        {
            _doneStatus = ActivityRef.Resources.GetString(Resource.String.DoneStatus);
            _failedStatus = ActivityRef.Resources.GetString(Resource.String.FailedStatus);
        }

        #region BaseQuestsAdapter overriding

        ///<inheritdoc/>
        protected override int  GetViewId()
        {
            return Resource.Layout.ResultQuestListItemHeader;
        }

        ///<inheritdoc/>
        protected override ResultQuestViewHolder CreateViewHolder(Android.Views.View view)
        {
            return new ResultQuestViewHolder(view);
        }

        ///<inheritdoc/>
        protected override void FillViewHolder(ResultQuestViewHolder holder, Quest questData, int position)
        {
            holder.Collapse();
            holder.ItemPosition = position;
            holder.RestartButton.Visibility = questData.Parent == null ? ViewStates.Visible : ViewStates.Gone;
            holder.Title.Text = questData.Title;
            holder.Description.Text = questData.Description;
            holder.ChildrenButton.Enabled = questData.Children != null;
            switch (questData.CurrentState)
            {
                case QuestState.Done:
                    holder.Status.Text = _doneStatus;
                    holder.Status.SetTextColor(Color.Green);
                    break;
                case QuestState.Failed:
                    holder.Status.Text = _failedStatus;
                    holder.Status.SetTextColor(Color.Red);
                    break;
                default:
                    holder.Status.Text = "Not defined status";
                    holder.Status.SetTextColor(Color.Orange);
                    break;
            }
        }
        #endregion
    }
}