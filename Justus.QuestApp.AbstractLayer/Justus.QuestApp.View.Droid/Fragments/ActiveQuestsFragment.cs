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
using Justus.QuestApp.View.Droid.Adapters;
using Justus.QuestApp.View.Droid.Adapters.List;
using Justus.QuestApp.View.Droid.Fragments.Abstracts;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment, that contains list of active quests.
    /// </summary>
    public class ActiveQuestsFragment : BaseTraverseQuestsFragment<ActiveQuestListViewModel, ActiveQuestItemViewHolder>
    {

        #region Fragment overriding


        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);

            QuestListView = view.FindViewById<ListView>(Resource.Id.questListId);
            TitleTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            TitleTextDefault = TitleTextView.Text;
            BackButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            BackButton.Enabled = ViewModel.CurrentQuest != null;
            BackButton.Click += BackButtonHandler;

            QuestListView.Adapter = QuestListAdapter = new ActiveQuestListAdapter(this.Activity, ViewModel);
            QuestListView.ItemClick += ItemClickHandler;
            QuestListView.ChildViewAdded += QuestAddedHandler;

            return view;
        }

        public override void OnPause()
        {
            base.OnPause();
            PullQuests();          
        }

        public override void OnResume()
        {
            base.OnResume();
            PushQuests();
        }

        #endregion

        #region Handlers

        private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs args)
        {
            ActiveQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(args.Child);
            holder.DoneButton.Click += (o, eventArgs) => { DoneClickHandler(holder.ItemPosition); };
            holder.FailButton.Click += (o, eventArgs) => { FailClickHandler(holder.ItemPosition); };
            holder.DeleteButton.Click += (o, eventArgs) => { DeleteClickHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenClickHandler(holder.ItemPosition); };
            holder.CancelButton.Click += (o, eventArgs) => { CancelClickHandler(holder.ItemPosition); };
        }

        private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs args)
        {
            ActiveQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(args.View);
            holder.ExpandDetails.Visibility = holder.ExpandDetails.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        }

        private void BackButtonHandler(object sender, EventArgs e)
        {
            this.TraverseToParent();
        }

        private void DoneClickHandler(int viewPosition)
        {
            Toast.MakeText(this.Context, $"Done of {viewPosition} clicked!", ToastLength.Short).Show();
        }

        private void FailClickHandler(int viewPosition)
        {
            Toast.MakeText(this.Context, $"Fail of {viewPosition} clicked!", ToastLength.Short).Show();
        }

        private void ChildrenClickHandler(int viewPosition)
        {
            this.TraverseToChild(viewPosition);
        }

        private void DeleteClickHandler(int viewPosition)
        {
            Toast.MakeText(this.Context, $"Delete of {viewPosition} clicked!", ToastLength.Short).Show();
        }

        private void CancelClickHandler(int viewPosition)
        {
            Toast.MakeText(this.Context, $"Cancel of {viewPosition} clicked!", ToastLength.Short).Show();
        }

        #endregion

        private async void PullQuests()
        {
            await ViewModel.PullQuests();
        }

        private async void PushQuests()
        {
            await ViewModel.PushQuests();
        }
    }
}