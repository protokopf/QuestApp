using System;
using Android.Runtime;
using Android.Widget;
using Justus.QuestApp.View.Droid.Abstract.ViewHolders;

namespace Justus.QuestApp.View.Droid.ViewHolders.QuestItem
{
    /// <summary>
    /// View holder for result quest item.
    /// </summary>
    public class ResultQuestViewHolder : ToggledViewHolder
    {
        public ResultQuestViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ResultQuestViewHolder(Android.Views.View itemView) : base(itemView)
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

        #region ToggledViewHolder overriding

        ///<inheritdoc cref="ToggledViewHolder"/>
        protected override int GetExpandDetailsId()
        {
            return Resource.Id.resultQuestItemDetails;
        }

        ///<inheritdoc cref="ToggledViewHolder"/>
        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}