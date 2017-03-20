using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Adapters.List;
using Justus.QuestApp.View.Droid.Fragments.Abstracts;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Type for interacting with available quests.
    /// </summary>
    public class AvailableQuestsFragment : BaseTraverseQuestsFragment<AvailableQuestListViewModel, AvailableQuestItemViewHolder>
    {
        #region Fragment overriding

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);

            QuestListView = view.FindViewById<ListView>(Resource.Id.questListId);
            TitleTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            BackButton = view.FindViewById<Button>(Resource.Id.questsListBack);
            SyncButton = view.FindViewById<Button>(Resource.Id.syncButton);

            TitleTextDefault = Activity.GetString(Resource.String.QuestListTitle);

            BackButton.Enabled = !ViewModel.InRoot;
            BackButton.Click += BackButtonOnClick;
            SyncButton.Click += SyncButtonOnClick;

            QuestListView.Adapter = QuestListAdapter = new AvailableQuestListAdapter(Activity, ViewModel);
            QuestListView.ItemClick += QuestListViewOnItemClick;
            QuestListView.ChildViewAdded += QuestListViewOnChildViewAdded;

            return view;
        }

        #endregion

        private async void SyncButtonOnClick(object sender, EventArgs eventArgs)
        {
            await ViewModel.PullQuests();
            RedrawListView();
        }

        #region Handlers

        private void QuestListViewOnChildViewAdded(object sender, ViewGroup.ChildViewAddedEventArgs args)
        {
            AvailableQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(args.Child);
            holder.DeleteButton.Click += (o, eventArgs) => { DeleteHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenHandler(holder.ItemPosition); };
            holder.StartButton.Click += (o, eventArgs) => { StartHandler(holder.ItemPosition); };
            holder.EditButton.Click += (o, eventArgs) => { EditHandler(holder.ItemPosition); };
        }

        private void EditHandler(int itemPosition)
        {
            Toast.MakeText(this.Context, $"Edit of {itemPosition} clicked!", ToastLength.Short).Show();
        }

        private void StartHandler(int itemPosition)
        {
            ViewModel.StartQuest(QuestListAdapter[itemPosition]);
            QuestListAdapter.NotifyDataSetChanged();
        }

        private void ChildrenHandler(int itemPosition)
        {
            TraverseToChild(itemPosition);
        }

        private void QuestListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs args)
        {
            AvailableQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(args.View);
            holder.ExpandDetails.Visibility = holder.ExpandDetails.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        }

        private void BackButtonOnClick(object sender, EventArgs eventArgs)
        {
            TraverseToParent();
        }

        #endregion
    }
}