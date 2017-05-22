using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.ViewHolders.Error
{
    class DefaultValidationErrorViewHolder : RecyclerView.ViewHolder
    {
        public DefaultValidationErrorViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public DefaultValidationErrorViewHolder(Android.Views.View itemView) : base(itemView)
        {
            ErrorTextView = itemView.FindViewById<TextView>(Resource.Id.errorTextViewId);
            ClarificationTextView = itemView.FindViewById<TextView>(Resource.Id.clarificationTextViewId);
        }

        /// <summary>
        /// Text view for holding error data.
        /// </summary>
        public TextView ErrorTextView { get; set; }

        /// <summary>
        /// Text view for holding clarification data.
        /// </summary>
        public TextView ClarificationTextView { get; set; }
    }
}