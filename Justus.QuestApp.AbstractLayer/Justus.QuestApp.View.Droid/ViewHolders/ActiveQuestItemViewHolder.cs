using System;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.ViewHolders
{
    public class ActiveQuestItemViewHolder : RecyclerView.ViewHolder
    {
        public ActiveQuestItemViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ActiveQuestItemViewHolder(Android.Views.View view, int position) : base(view)
        {
            Title = view.FindViewById<TextView>(Resource.Id.questTitle);
            TimeLeft = view.FindViewById<TextView>(Resource.Id.questTimeLeft);
            Progress = view.FindViewById<ProgressBar>(Resource.Id.questProgress);
            Progress.ProgressDrawable.SetColorFilter(Color.Green, PorterDuff.Mode.SrcIn);
            Details = view.FindViewById<LinearLayout>(Resource.Id.childItemLayout);

            Description = Details.FindViewById<TextView>(Resource.Id.questDescription);

            DoneButton = Details.FindViewById<Button>(Resource.Id.questDone);
            FailButton = Details.FindViewById<Button>(Resource.Id.questFailed);
            EditButton = Details.FindViewById<Button>(Resource.Id.questEdit);
            ChildrenButton = Details.FindViewById<Button>(Resource.Id.questChildrenButton);

            ItemPosition = position;
        }

        public TextView Title { get; set; }
        public TextView TimeLeft { get; set; }
        public ProgressBar Progress { get; set; }

        public LinearLayout Details { get; set; }

        public TextView Description { get; set; }

        public Button DoneButton { get; set; }
        public Button FailButton { get; set; }
        public Button EditButton { get; set; }
        public Button ChildrenButton { get; set; }

        public int ItemPosition { get; set; }
    }
}