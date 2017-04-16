using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Justus.QuestApp.View.Droid.Abstract.ViewHolders;

namespace Justus.QuestApp.View.Droid.Abstract.Fragments
{
    /// <summary>
    /// Base class for fragment, which displays list of quests. 
    /// Contains references to bac button and current quest title textview.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TViewHolder"></typeparam>
    public abstract class BaseTraverseQuestsFragment<TViewModel, TViewHolder> : BaseQuestsFragment<TViewModel, TViewHolder>
        where TViewModel : QuestListViewModel
        where TViewHolder : ToggledViewHolder
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

        #region Fragment overriding

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container,
            Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);

            RecyclerViewRef = HandleRecyclerView(view);

            TitleTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            BackButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            TitleTextDefault = Activity.GetString(Resource.String.QuestListTitle);

            BackButton.Enabled = !ViewModel.InRoot;
            BackButton.Click += BackButtonHandler;

            return view;
        }

        #endregion

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
                ViewModel.TraverseToRoot();
                TitleTextView.Text = TitleTextDefault;
                BackButton.Enabled = false;
                RedrawList();
            }
        }

        /// <summary>
        /// Contains logic for traversing from child to parent.
        /// </summary>
        protected virtual void TraverseToParent()
        {
            ViewModel.TraverseToParent();
            TitleTextView.Text = ViewModel.QuestsListTitle ?? TitleTextDefault;
            BackButton.Enabled = !ViewModel.InRoot;
            RedrawList();
        }

        /// <summary>
        /// Contains logic for traversing from parent to child.
        /// </summary>
        /// <param name="childPosition"></param>
        protected virtual void TraverseToChild(int childPosition)
        {
            ViewModel.TraverseToLeaf(childPosition);
            TitleTextView.Text = ViewModel.QuestsListTitle ?? TitleTextDefault;
            BackButton.Enabled = !ViewModel.InRoot;
            RedrawList();
        }

        protected abstract RecyclerView HandleRecyclerView(Android.Views.View fragmentView);

        #region Handlers

        private void BackButtonHandler(object sender, EventArgs e)
        {
            this.TraverseToParent();
        }

        /// <summary>
        /// Handles deleting quest from list.
        /// </summary>
        /// <param name="position"></param>
        protected virtual async void DeleteHandler(int position)
        {
            await ViewModel.DeleteQuest(position);
            ViewModel.ResetChildren();
            RedrawList();
            Toast.MakeText(this.Context, $"Quest in {position} position was deleted.", ToastLength.Short).Show();
        }

        #endregion
    } 
}