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

        public ResultQuestItemViewHolder(Android.Views.View itemView, int position) : base(itemView, position)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.resultQuestTitle);
            Status = itemView.FindViewById<TextView>(Resource.Id.resultQuestStatus);

            Description = ExpandDetails.FindViewById<TextView>(Resource.Id.resultQuestDescription);
            StartButton = ExpandDetails.FindViewById<Button>(Resource.Id.resultQuestStart);
            DeleteButton = ExpandDetails.FindViewById<Button>(Resource.Id.resultQuestDelete);
            ChildrenButton = ExpandDetails.FindViewById<Button>(Resource.Id.resultQuestChildrenButton);
        }

        public TextView Title { get; set; }
        public TextView Status { get; set; }

        public TextView Description { get; set; }

        public Button ChildrenButton { get; set; }

        public Button StartButton { get; set; }

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