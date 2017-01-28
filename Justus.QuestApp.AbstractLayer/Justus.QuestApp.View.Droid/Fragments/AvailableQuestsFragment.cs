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

            TitleTextDefault = Activity.GetString(Resource.String.QuestDefaultTitle);

            BackButton.Enabled = ViewModel.CurrentQuest != null;
            BackButton.Click += BackButtonOnClick;

            QuestListView.Adapter = QuestListAdapter = new AvailableQuestListAdapter(Activity, ViewModel);
            QuestListView.ItemClick += QuestListViewOnItemClick;
            QuestListView.ChildViewAdded += QuestListViewOnChildViewAdded;

            return view;
        }

        private void QuestListViewOnChildViewAdded(object sender, ViewGroup.ChildViewAddedEventArgs args)
        {
            AvailableQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(args.Child);
            holder.DeleteButton.Click += (o, eventArgs) => { DeleteClickHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenClickHandler(holder.ItemPosition); };
            holder.StartButton.Click += (o, eventArgs) => { StartClickHandler(holder.ItemPosition); };
            holder.EditButton.Click += (o, eventArgs) => { EditClickHandler(holder.ItemPosition); };
        }

        private void EditClickHandler(int itemPosition)
        {
            Toast.MakeText(this.Context, $"Edit of {itemPosition} clicked!", ToastLength.Short).Show();
        }

        private void StartClickHandler(int itemPosition)
        {
            Toast.MakeText(this.Context, $"Start of {itemPosition} clicked!", ToastLength.Short).Show();
        }

        private void DeleteClickHandler(int itemPosition)
        {
            Toast.MakeText(this.Context, $"Delete of {itemPosition} clicked!", ToastLength.Short).Show();
        }

        private void ChildrenClickHandler(int itemPosition)
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