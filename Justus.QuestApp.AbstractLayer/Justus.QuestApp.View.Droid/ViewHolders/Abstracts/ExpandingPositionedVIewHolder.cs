using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.ViewHolders.Abstracts
{
    /// <summary>
    /// Type for view, which can expand and have definit position.
    /// </summary>
    public abstract class ExpandingPositionedViewHolder : PositionedViewHolder
    {
        protected ExpandingPositionedViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected ExpandingPositionedViewHolder(Android.Views.View itemView, int position) : base(itemView, position)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            ExpandDetails = itemView.FindViewById<LinearLayout>(GetExpandDetailsId());
        }

        /// <summary>
        /// Container for collapsing/expanding part of view.
        /// </summary>
        public LinearLayout ExpandDetails { get; set; }

        /// <summary>
        /// Returns id for ExpandDetails element of view.
        /// </summary>
        /// <returns></returns>
        protected abstract int GetExpandDetailsId();
    }
}