using System;
using Android.Runtime;
using Android.Widget;
using Justus.QuestApp.View.Droid.Abstract.ViewHolders;

namespace Justus.QuestApp.View.Droid.ViewHolders
{
    /// <summary>
    /// View holder for available quest item.
    /// </summary>
    public class AvailableQuestViewHolder : ToggledViewHolder
    {
        public AvailableQuestViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AvailableQuestViewHolder(Android.Views.View itemView) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.availableQuestTitle);
            StartTime = itemView.FindViewById<TextView>(Resource.Id.availableQuestStartTime);
            Deadline = itemView.FindViewById<TextView>(Resource.Id.availableQuestDeadline);

            Description = ExpandDetails.FindViewById<TextView>(Resource.Id.availableQuestDescription);
            ChildrenButton = ExpandDetails.FindViewById<Button>(Resource.Id.availableQuestChildrenButton);
            StartButton = ExpandDetails.FindViewById<Button>(Resource.Id.availabletQuestStart);
            EditButton = ExpandDetails.FindViewById<Button>(Resource.Id.availableQuestEdit);
            DeleteButton = ExpandDetails.FindViewById<Button>(Resource.Id.availableQuestDelete);
        }

        public TextView Title { get; set; }

        public TextView StartTime { get; set; }

        public TextView Deadline { get; set; }

        public TextView Description { get; set; }

        public Button ChildrenButton { get; set; }

        public Button StartButton { get; set; }

        public Button DeleteButton { get; set; }

        public Button EditButton { get; set; }

        #region ToggledViewHolder overriding

        protected override int GetExpandDetailsId()
        {
            return Resource.Id.availableQuestItemDetails;
        }

        #endregion
    }
}