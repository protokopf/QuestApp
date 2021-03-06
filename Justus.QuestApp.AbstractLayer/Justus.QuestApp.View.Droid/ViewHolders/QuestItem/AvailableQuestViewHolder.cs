using System;
using Android.Runtime;
using Android.Widget;
using Justus.QuestApp.View.Droid.Abstract.ViewHolders;

namespace Justus.QuestApp.View.Droid.ViewHolders.QuestItem
{
    /// <summary>
    /// View holder for available quest item.
    /// </summary>
    public class AvailableQuestViewHolder : ToggledViewHolder
    {
        private bool _isImportantEnable = false;

        public AvailableQuestViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AvailableQuestViewHolder(Android.Views.View itemView) : base(itemView)
        {
            IsImportantButton = itemView.FindViewById<ImageView>(Resource.Id.availableImportantButtonId);
            Title = itemView.FindViewById<TextView>(Resource.Id.availableQuestTitle);
            StartTime = itemView.FindViewById<TextView>(Resource.Id.availableQuestStartTime);
            Deadline = itemView.FindViewById<TextView>(Resource.Id.availableQuestDeadline);

            Description = ExpandDetails.FindViewById<TextView>(Resource.Id.availableQuestDescription);
            ChildrenButton = ExpandDetails.FindViewById<Button>(Resource.Id.availableQuestChildrenButton);
            StartButton = ExpandDetails.FindViewById<Button>(Resource.Id.availabletQuestStart);
            EditButton = ExpandDetails.FindViewById<Button>(Resource.Id.availableQuestEdit);
            DeleteButton = ExpandDetails.FindViewById<Button>(Resource.Id.availableQuestDelete);

            IsImportantButton.Click += IsImportantButton_Click;
        }

        public ImageView IsImportantButton { get; set; }

        public TextView Title { get; set; }

        public TextView StartTime { get; set; }

        public TextView Deadline { get; set; }

        public TextView Description { get; set; }

        public Button ChildrenButton { get; set; }

        public Button StartButton { get; set; }

        public Button DeleteButton { get; set; }

        public Button EditButton { get; set; }

        public void HandleIsImportantButton(bool isImportant)
        {
            _isImportantEnable = isImportant;
            IsImportantButton_Click(IsImportantButton, EventArgs.Empty);
        }

        private void IsImportantButton_Click(object sender, EventArgs e)
        {
            IsImportantButton.SetImageResource(_isImportantEnable ? Resource.Drawable.star_enabled_16 : Resource.Drawable.star_disabled_16);
            _isImportantEnable = !_isImportantEnable;
        }

        #region ToggledViewHolder overriding

        ///<inehritdoc cref="ToggledViewHolder"/>
        protected override int GetExpandDetailsId()
        {
            return Resource.Id.availableQuestItemDetails;
        }

        ///<inehritdoc cref="ToggledViewHolder"/>
        public override void Refresh()
        {
            this.Title.Text = string.Empty;
            this.Description.Text = string.Empty;
            this.StartTime.Text = string.Empty;
            this.Deadline.Text = string.Empty;       
        }

        #endregion
    }
}