using System;
using Android.Runtime;
using Android.Support.V7.Widget;

namespace Justus.QuestApp.View.Droid.ViewHolders.Abstracts
{
    /// <summary>
    /// Base holder class for all positioned views.
    /// </summary>
    public abstract class PositionedViewHolder : RecyclerView.ViewHolder
    {
        /// <summary>
        /// Need for Xamarin platform.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        protected PositionedViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        /// <summary>
        /// Get view
        /// </summary>
        /// <param name="itemView"></param>
        /// <param name="position"></param>
        protected PositionedViewHolder(Android.Views.View itemView) : base(itemView)
        {
        }

        public int ItemPosition { get; set; }
    }
}