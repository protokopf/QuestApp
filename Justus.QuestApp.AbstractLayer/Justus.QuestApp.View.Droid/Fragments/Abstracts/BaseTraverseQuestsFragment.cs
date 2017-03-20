using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.ViewHolders.Abstracts;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments.Abstracts
{
    /// <summary>
    /// Base class for fragment, which displays list of quests. 
    /// Contains references to bac button and current quest title textview.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TViewHolder"></typeparam>
    public class BaseTraverseQuestsFragment<TViewModel, TViewHolder> : BaseQuestsFragment<TViewModel, TViewHolder>
        where TViewModel : QuestListViewModel
        where TViewHolder : ExpandingPositionedViewHolder
    {
        /// <summary>
        /// Reference to text view which displays current title.
        /// </summary>
        protected TextView TitleTextView;

        /// <summary>
        /// Reference to back button.
        /// </summary>
        protected Button BackButton;

        /// <summary>
        /// Reference to sync button.
        /// </summary>
        protected Button SyncButton;

        /// <summary>
        /// Defaul quest title.
        /// </summary>
        protected string TitleTextDefault;

        #region ISelectable overriding

        ///<inheritdoc/>
        public override void OnSelect()
        {
            if (ViewModel.InRoot)
            {
                ViewModel.ResetChildren();
            }
            else
            {
                TraverseToRoot();
            }
            base.OnSelect();
        }

        #endregion

        /// <summary>
        /// Traverse to root.
        /// </summary>
        protected void TraverseToRoot()
        {
            if (!ViewModel.InRoot)
            {
                CollapsChildren();
                ViewModel.TraverseToRoot();
                TitleTextView.Text = TitleTextDefault;
                BackButton.Enabled = false;
                RedrawListView();
            }
        }

        /// <summary>
        /// Contains logic for traversing from child to parent.
        /// </summary>
        protected virtual void TraverseToParent()
        {
            CollapsChildren();

            ViewModel.TraverseToParent();
            TitleTextView.Text = ViewModel.QuestsListTitle ?? TitleTextDefault;
            BackButton.Enabled = !ViewModel.InRoot;
            RedrawListView();
        }

        /// <summary>
        /// Contains logic for traversing from parent to child.
        /// </summary>
        /// <param name="childPosition"></param>
        protected virtual void TraverseToChild(int childPosition)
        {
            CollapsChildren();

            ViewModel.TraverseToLeaf(childPosition);
            TitleTextView.Text = ViewModel.QuestsListTitle ?? TitleTextDefault;
            BackButton.Enabled = !ViewModel.InRoot;
            RedrawListView();
        }

        /// <summary>
        /// Collaps all children of ListView.
        /// </summary>
        protected virtual void CollapsChildren()
        {
            IEnumerable<TViewHolder> holders = QuestListAdapter.GetViewHolders();
            foreach (TViewHolder holder in holders)
            {
                if (holder.ExpandDetails.Visibility == ViewStates.Visible)
                {
                    holder.ExpandDetails.Visibility = ViewStates.Gone;
                }            
            }
        }

        #region Handlers

        /// <summary>
        /// Handles deleting quest from list.
        /// </summary>
        /// <param name="position"></param>
        protected virtual async void DeleteHandler(int position)
        {
            await ViewModel.DeleteQuest(position);
            ViewModel.ResetChildren();
            RedrawListView();
            Toast.MakeText(this.Context, $"Quest in {position} position was deleted.", ToastLength.Short).Show();
        }

        #endregion
    } 
}