using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.ViewHolders
{
    public class ResultQuestItemViewHolder : RecyclerView.ViewHolder
    {
        public ResultQuestItemViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ResultQuestItemViewHolder(Android.Views.View itemView, int position) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.resultQuestTitle);
            Status = itemView.FindViewById<TextView>(Resource.Id.resultQuestStatus);

            Details = itemView.FindViewById<LinearLayout>(Resource.Id.resultQuestItemDetails);

            Description = Details.FindViewById<TextView>(Resource.Id.resultQuestDescription);
            StartButton = Details.FindViewById<Button>(Resource.Id.resultQuestStart);
            DeleteButton = Details.FindViewById<Button>(Resource.Id.resultQuestDelete);
            ChildrenButton = Details.FindViewById<Button>(Resource.Id.resultQuestChildrenButton);

            ItemPosition = position;
        }

        public TextView Title { get; set; }
        public TextView Status { get; set; }

        public LinearLayout Details { get; set; }

        public TextView Description { get; set; }

        public Button ChildrenButton { get; set; }

        public Button StartButton { get; set; }

        public Button DeleteButton { get; set; }

        public int ItemPosition { get; set; }
    }
}