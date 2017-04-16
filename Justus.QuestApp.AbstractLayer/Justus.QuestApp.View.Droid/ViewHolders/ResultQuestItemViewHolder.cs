using System;
using Android.Runtime;
using Android.Widget;
using Justus.QuestApp.View.Droid.ViewHolders.Abstracts;

namespace Justus.QuestApp.View.Droid.ViewHolders
{
    /// <summary>
    /// View holder for result quest item.
    /// </summary>
    public class ResultQuestItemViewHolder : ExpandingPositionedViewHolder
    {
        public ResultQuestItemViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ResultQuestItemViewHolder(Android.Views.View itemView) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.resultQuestTitle);
            Status = itemView.FindViewById<TextView>(Resource.Id.resultQuestStatus);

            Description = ExpandDetails.FindViewById<TextView>(Resource.Id.resultQuestDescription);
            RestartButton = ExpandDetails.FindViewById<Button>(Resource.Id.resultQuestRestart);
            DeleteButton = ExpandDetails.FindViewById<Button>(Resource.Id.resultQuestDelete);
            ChildrenButton = ExpandDetails.FindViewById<Button>(Resource.Id.resultQuestChildrenButton);
        }

        public TextView Title { get; set; }
        public TextView Status { get; set; }

        public TextView Description { get; set; }

        public Button ChildrenButton { get; set; }

        public Button RestartButton { get; set; }

        public Button DeleteButton { get; set; }

        #region ExpandingPositionedViewHolder overridng

        ///<inhericdoc/>
        protected override int GetExpandDetailsId()
        {
            return Resource.Id.resultQuestItemDetails;
        }

        #endregion
    }
}