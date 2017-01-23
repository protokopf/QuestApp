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
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Adapters.List;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    public class ResultQuestsFragment : BaseQuestListFragment<ResultsQuestListVIewModel, ResultQuestItemViewHolder>
    {
        private TextView _headerTextView;
        private Button _backButton;
        private string _headerDefault = String.Empty;

        public override void OnResume()
        {
            base.OnResume();
            QuestListView.Adapter = QuestListView.Adapter;
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);
            QuestListView = view.FindViewById<ListView>(Resource.Id.questListId);

            _headerTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            _backButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            QuestListView.Adapter = QuestListAdapter = new ResultQuestListAdapter(this, ViewModel);
            QuestListView.ItemClick += ItemClickHandler;
            QuestListView.ChildViewAdded += QuestAddedHandler;

            _backButton.Click += BackButtonOnClick;
            _backButton.Enabled = ViewModel.CurrentQuest != null;

            _headerDefault = _headerTextView.Text;

            return view;
        }

        private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            ResultQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(e.Child);
            holder.StartButton.Click += (s, args) => { StartHandler(holder.ItemPosition); };
            holder.DeleteButton.Click += (s, args) => { DeleteHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (s, args) => { ChildrenHandler(holder.ItemPosition); };
        }

        #region Handlers

        private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs e)
        {
            ResultQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(e.View);
            holder.Details.Visibility = holder.Details.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;

        }

        private void BackButtonOnClick(object sender, EventArgs eventArgs)
        {
            for (int i = 0; i < QuestListAdapter.Count; ++i)
            {
                ResultQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(QuestListView.GetChildAt(i));
                holder.Details.Visibility = ViewStates.Gone;
            }
            ViewModel.CurrentQuest = ViewModel.CurrentQuest.Parent;
            _headerTextView.Text = ViewModel.QuestsListTitle ?? _headerDefault;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            QuestListView.Adapter = QuestListAdapter;
        }

        private void StartHandler(int position)
        {
            Toast.MakeText(this.Context, $"Start of {position} clicked!", ToastLength.Short).Show();
        }

        private void DeleteHandler(int position)
        {
            Toast.MakeText(this.Context, $"Delete of {position} clicked!", ToastLength.Short).Show();
        }

        private void ChildrenHandler(int position)
        {
            for (int i = 0; i < QuestListAdapter.Count; ++i)
            {
                ResultQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(QuestListView.GetChildAt(i));
                holder.Details.Visibility = ViewStates.Gone;
            }
            ViewModel.CurrentQuest = ViewModel.CurrentChildren[position];
            _headerTextView.Text = ViewModel.QuestsListTitle;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            QuestListView.Adapter = QuestListAdapter;
        }

        #endregion
    }
}