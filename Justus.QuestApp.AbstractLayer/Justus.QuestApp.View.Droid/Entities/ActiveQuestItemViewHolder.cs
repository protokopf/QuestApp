using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.Entities
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

            DoneButton = view.FindViewById<Button>(Resource.Id.questDone);
            FailButton = view.FindViewById<Button>(Resource.Id.questFailed);
            EditButton = view.FindViewById<Button>(Resource.Id.questEdit);
            ChildrenButton = view.FindViewById<Button>(Resource.Id.questChildrenButton);

            ItemPosition = position;
        }

        public TextView Title { get; set; }
        public TextView TimeLeft { get; set; }
        public ProgressBar Progress { get; set; }

        public LinearLayout Details { get; set; }

        public Button DoneButton { get; set; }
        public Button FailButton { get; set; }
        public Button EditButton { get; set; }
        public Button ChildrenButton { get; set; }

        public int ItemPosition { get; set; }
    }
}