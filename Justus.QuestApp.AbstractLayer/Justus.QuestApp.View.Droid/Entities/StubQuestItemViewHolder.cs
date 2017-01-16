using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.Entities
{
    class StubQuestItemViewHolder : RecyclerView.ViewHolder
    {
        public StubQuestItemViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public StubQuestItemViewHolder(Android.Views.View view) : base(view)
        {
            Title = view.FindViewById<TextView>(Resource.Id.questTitle);
            TimeLeft = view.FindViewById<TextView>(Resource.Id.questTimeLeft);
            Progress = view.FindViewById<ProgressBar>(Resource.Id.questProgress);
        }

        public TextView Title { get; set; }
        public TextView TimeLeft { get; set; }
        public ProgressBar Progress { get; set; }
    }
}