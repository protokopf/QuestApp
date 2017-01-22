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
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    public class ResultQuestsFragment : BaseFragment<ResultsQuestListVIewModel>
    {
        private TextView _headerTextView;
        private Button _backButton;
        private ListView _resultsQuestListView;

        private ResultQuestListAdapter _adapter;
        private string _headerDefault = String.Empty;

        public override void OnResume()
        {
            base.OnResume();
            _adapter.NotifyDataSetChanged();
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);
            _resultsQuestListView = view.FindViewById<ListView>(Resource.Id.questListId);

            _headerTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            _backButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            _resultsQuestListView.Adapter = _adapter = new ResultQuestListAdapter(this, ViewModel);
            _resultsQuestListView.ItemClick += ItemClickHandler;
            _resultsQuestListView.ChildViewAdded += QuestAddedHandler;

            _backButton.Click += BackButtonOnClick;
            _backButton.Enabled = ViewModel.CurrentQuest != null;

            _headerDefault = _headerTextView.Text;

            return view;
        }

        private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            ResultQuestItemViewHolder holder = _adapter.GetHolderByView(e.Child);
            holder.StartButton.Click += (s, args) => { StartHandler(holder.ItemPosition); };
            holder.DeleteButton.Click += (s, args) => { DeleteHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (s, args) => { ChildrenHandler(holder.ItemPosition); };
        }

        #region Handlers

        private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs e)
        {
            ResultQuestItemViewHolder holder = _adapter.GetHolderByView(e.View);
            holder.Details.Visibility = holder.Details.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;

        }

        private void BackButtonOnClick(object sender, EventArgs eventArgs)
        {
            ViewModel.CurrentQuest = ViewModel.CurrentQuest.Parent;
            _headerTextView.Text = ViewModel.QuestsListTitle ?? _headerDefault;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            _adapter.NotifyDataSetChanged();
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
            ViewModel.CurrentQuest = ViewModel.CurrentChildren[position];
            _headerTextView.Text = ViewModel.QuestsListTitle;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            for (int i = 0; i < _adapter.Count; ++i)
            {
                ResultQuestItemViewHolder holder = _adapter.GetHolderByView(_resultsQuestListView.GetChildAt(i));
                holder.Details.Visibility = ViewStates.Gone;
            }
            _adapter.NotifyDataSetChanged();
        }

        #endregion
    }
}