using System;
using Android.App;
using Android.Views;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Abstract.Adapters;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.ViewHolders.QuestItem;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.Quests
{
    /// <summary>
    /// Adapter for available quest list view.
    /// </summary>
    public class AvailableQuestsAdapter : BaseQuestsAdapter<AvailableQuestViewHolder, AvailableQuestListViewModel>
    {
        /// <summary>
        /// Receives references to activity and list view model.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="questsViewModel"></param>
        /// <param name="holderClickManager"></param>
        public AvailableQuestsAdapter(Activity activity, 
            AvailableQuestListViewModel questsViewModel, 
            IViewHolderClickManager<AvailableQuestViewHolder> holderClickManager) :
            base(activity,questsViewModel, holderClickManager)
        {
        }

        #region BaseQuestsAdapter overriding

        ///<inheritdoc/>
        protected override int GetViewId()
        {
            return Resource.Layout.AvailableQuestListItemHeader;
        }

        ///<inheritdoc/>
        protected override AvailableQuestViewHolder CreateViewHolder(Android.Views.View view)
        {
            return new AvailableQuestViewHolder(view);
        }

        ///<inheritdoc/>
        protected override void FillViewHolder(AvailableQuestViewHolder holder, Quest questData, int position)
        {
            holder.Collapse();

            holder.HandleIsImportantButton(questData.IsImportant);
            if (questData.StartTime != null)
            {
                holder.StartTime.Text = FormTime(questData.StartTime.Value);
            }

            if (questData.Deadline != null)
            {
                holder.Deadline.Text = FormTime(questData.Deadline.Value);
            }
            

            holder.ItemPosition = position;
            holder.Title.Text = questData.Title;
            holder.Description.Text = questData.Description;
            holder.StartButton.Visibility = questData.Parent == null ? ViewStates.Visible : ViewStates.Gone;
            holder.ChildrenButton.Enabled = questData.Children != null;
        } 

        #endregion
        private string FormTime(DateTime time)
        {
            if (time != DateTime.MaxValue)
            {
                return $"{time.Year}.{time.Month}.{time.Day} {time.Hour}:{time.Minute}:{time.Second}";
            }
            return "Not specified";

        }
    }
}