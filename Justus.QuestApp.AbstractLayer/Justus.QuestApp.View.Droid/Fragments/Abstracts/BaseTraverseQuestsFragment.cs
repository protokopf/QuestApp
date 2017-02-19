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
using Justus.QuestApp.View.Droid.ViewHolders;
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
        /// Defaul quest title.
        /// </summary>
        protected string TitleTextDefault;

        /// <summary>
        /// Contains logic for traversing from child to parent.
        /// </summary>
        protected virtual void TraverseToParent()
        {
            CollapsChildren();

            ViewModel.CurrentQuest = ViewModel.CurrentQuest.Parent;
            TitleTextView.Text = ViewModel.QuestsListTitle ?? TitleTextDefault;
            BackButton.Enabled = ViewModel.CurrentQuest != null;
            QuestListView.Adapter = QuestListAdapter;

            ViewModel.ResetChildren();
        }

        /// <summary>
        /// Contains logic for traversing from parent to child.
        /// </summary>
        /// <param name="childPosition"></param>
        protected virtual void TraverseToChild(int childPosition)
        {
            CollapsChildren();

            ViewModel.CurrentQuest = ViewModel.CurrentChildren[childPosition];
            TitleTextView.Text = ViewModel.QuestsListTitle ?? TitleTextDefault;
            BackButton.Enabled = ViewModel.CurrentQuest != null;
            QuestListView.Adapter = QuestListAdapter;

            ViewModel.ResetChildren();
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
    } 
}