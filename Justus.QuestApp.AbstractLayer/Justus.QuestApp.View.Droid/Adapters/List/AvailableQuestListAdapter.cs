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
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.List
{
    /// <summary>
    /// Adapter for available quest list view.
    /// </summary>
    public class AvailableQuestListAdapter : BaseQuestListAdapter<AvailableQuestItemViewHolder>
    {
        /// <summary>
        /// Receives references to activity and list view model.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="listViewModel"></param>
        public AvailableQuestListAdapter(Activity activity, QuestListViewModel listViewModel) : base(activity,listViewModel)
        {
        }

        #region BaseQuestListAdapter overriding

        ///<inheritdoc/>
        protected override int GetViewId()
        {
            return Resource.Layout.AvailableQuestListItemHeader;
        }

        ///<inheritdoc/>
        protected override AvailableQuestItemViewHolder CreateViewHolder(Android.Views.View view, int position)
        {
            return new AvailableQuestItemViewHolder(view, position);
        }

        ///<inheritdoc/>
        protected override void FillViewHolder(AvailableQuestItemViewHolder holder, Quest questData, int position)
        {
            holder.ItemPosition = position;
            holder.Title.Text = questData.Title;
            holder.Description.Text = questData.Description;
            holder.StartButton.Visibility = questData.Parent == null ? ViewStates.Visible : ViewStates.Gone;
            holder.ChildrenButton.Enabled = questData.Children != null;
        } 

        #endregion
    }
}