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
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment, that contains list of active quests.
    /// </summary>
    public class ActiveQuestsFragment : BaseQuestListFragment<ActiveQuestListViewModel, ActiveQuestItemViewHolder>
    {
        private TextView _headerTextView;
        private Button _backButton;
        private string _headerDefault = String.Empty;

        #region Fragment overriding


        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);

            QuestListView = view.FindViewById<ListView>(Resource.Id.questListId);
            _headerTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            _headerDefault = _headerTextView.Text;
            _backButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            _backButton.Enabled = ViewModel.CurrentQuest != null;
            _backButton.Click += BackButtonHandler;

            QuestListView.Adapter = QuestListAdapter = new ActiveQuestListAdapter(this, ViewModel);
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
            holder.EditButton.Click += (o, eventArgs) => { EditClickHandler(holder.ItemPosition); };
            holder.FailButton.Click += (o, eventArgs) => { FailClickHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenClickHandler(holder.ItemPosition); };
        }

        private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs args)
        {
            ActiveQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(args.View);
            holder.Details.Visibility = holder.Details.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        }

        private void BackButtonHandler(object sender, EventArgs e)
        {
            ViewModel.CurrentQuest = ViewModel.CurrentQuest.Parent;
            _headerTextView.Text = ViewModel.QuestsListTitle ?? _headerDefault;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            QuestListAdapter.NotifyDataSetChanged();
        }

        private void DoneClickHandler(int viewPosition)
        {
            Toast.MakeText(this.Context, $"Done of {viewPosition} clicked!", ToastLength.Short).Show();
        }

        private void FailClickHandler(int viewPosition)
        {
            Toast.MakeText(this.Context, $"Fail of {viewPosition} clicked!", ToastLength.Short).Show();
        }

        private void EditClickHandler(int viewPosition)
        {
            Toast.MakeText(this.Context, $"Edit of {viewPosition} clicked!", ToastLength.Short).Show();
        }

        private void ChildrenClickHandler(int viewPosition)
        {
            ViewModel.CurrentQuest = ViewModel.CurrentChildren[viewPosition];
            _headerTextView.Text = ViewModel.QuestsListTitle ?? _headerDefault;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            for (int i = 0; i < QuestListView.Count; ++i)
            {
                ActiveQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(QuestListView.GetChildAt(i));
                holder.Details.Visibility = ViewStates.Gone;
            }
            QuestListAdapter.NotifyDataSetChanged();
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