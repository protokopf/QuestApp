using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.Abstract.ViewHolders
{
    /// <summary>
    /// Type for view, which can expand and have position.
    /// </summary>
    public abstract class ToggledViewHolder : PositionedViewHolder
    {
        protected ToggledViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected ToggledViewHolder(Android.Views.View itemView) : base(itemView)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            ExpandDetails = itemView.FindViewById<LinearLayout>(GetExpandDetailsId());
        }

        /// <summary>
        /// Container for collapsing/expanding part of view.
        /// </summary>
        protected LinearLayout ExpandDetails { get; set; }

        /// <summary>
        /// Expands details.
        /// </summary>
        public void Expand()
        {
            if (ExpandDetails.Visibility != ViewStates.Visible)
            {
                ExpandDetails.Visibility = ViewStates.Visible;
            }
        }

        /// <summary>
        /// Collapse details.
        /// </summary>
        public void Collapse()
        {
            if (ExpandDetails.Visibility != ViewStates.Gone)
            {
                ExpandDetails.Visibility = ViewStates.Gone;
            }
        }

        /// <summary>
        /// Toggles details.
        /// </summary>
        public void Toggle()
        {
            ExpandDetails.Visibility = ExpandDetails.Visibility == ViewStates.Visible
                ? ViewStates.Gone
                : ViewStates.Visible;
        }

        /// <summary>
        /// Returns id for ExpandDetails element of view.
        /// </summary>
        /// <returns></returns>
        protected abstract int GetExpandDetailsId();
    }
}