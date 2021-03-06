using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Justus.QuestApp.AbstractLayer.Helpers.Behaviours;

namespace Justus.QuestApp.View.Droid.Abstract.ViewHolders
{
    /// <summary>
    /// Base holder class for all positioned views.
    /// </summary>
    public abstract class PositionedViewHolder : RecyclerView.ViewHolder, IRefreshable
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
        protected PositionedViewHolder(Android.Views.View itemView) : base(itemView)
        {
        }

        public int ItemPosition { get; set; }

        #region IRefreshable implementation

        ///<inheritdoc cref="IRefreshable"/>
        public abstract void Refresh();

        #endregion
    }
}