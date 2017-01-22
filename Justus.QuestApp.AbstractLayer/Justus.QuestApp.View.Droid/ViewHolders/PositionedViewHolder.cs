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

namespace Justus.QuestApp.View.Droid.ViewHolders
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
        /// Get view and its position.
        /// </summary>
        /// <param name="itemView"></param>
        /// <param name="position"></param>
        protected PositionedViewHolder(Android.Views.View itemView, int position) : base(itemView)
        {
            ItemPosition = position;
        }

        public int ItemPosition { get; set; }
    }
}