using System;
using Android.Graphics;
using Android.Runtime;
using Android.Widget;
using Justus.QuestApp.View.Droid.ViewHolders.Abstracts;

namespace Justus.QuestApp.View.Droid.ViewHolders
{
    /// <summary>
    /// View holder for active quest item.
    /// </summary>
    public class ActiveQuestItemViewHolder : ExpandingPositionedViewHolder
    {
        public ActiveQuestItemViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ActiveQuestItemViewHolder(Android.Views.View view) : base(view)
        {
            Title = view.FindViewById<TextView>(Resource.Id.questTitle);
            TimeLeft = view.FindViewById<TextView>(Resource.Id.questTimeLeft);
            Progress = view.FindViewById<ProgressBar>(Resource.Id.questProgress);
            Progress.ProgressDrawable.SetColorFilter(Color.Green, PorterDuff.Mode.SrcIn);

            Description = ExpandDetails.FindViewById<TextView>(Resource.Id.questDescription);
            ChildrenButton = ExpandDetails.FindViewById<Button>(Resource.Id.questChildrenButton);

            DoneButton = ExpandDetails.FindViewById<Button>(Resource.Id.questDone);
            FailButton = ExpandDetails.FindViewById<Button>(Resource.Id.questFailed);
            StartButton = ExpandDetails.FindViewById<Button>(Resource.Id.questStart);
            DeleteButton = ExpandDetails.FindViewById<Button>(Resource.Id.questDelete);
            CancelButton = ExpandDetails.FindViewById<Button>(Resource.Id.questCancel);
        }

        public TextView Title { get; set; }
        public TextView TimeLeft { get; set; }
        public ProgressBar Progress { get; set; }
    
        public TextView Description { get; set; }

        public Button ChildrenButton { get; set; }

        #region Action buttons

        public Button DoneButton { get; set; }
        public Button FailButton { get; set; }
        public Button DeleteButton { get; set; }
        public Button StartButton { get; set; }
        public Button CancelButton { get; set; }

        #endregion

        #region ExpandingPositionedViewHolder overridng

        ///<inheritdoc/>
        protected override int GetExpandDetailsId()
        {
            return Resource.Id.childItemLayout;
        }

        #endregion
    }
}