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
using Justus.QuestApp.View.Droid.Fragments.Abstracts;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment for displaying result quests.
    /// </summary>
    public class ResultQuestsFragment : BaseTraverseQuestsFragment<ResultsQuestListViewModel, ResultQuestItemViewHolder>
    {
        #region Fragment overriding

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);
            QuestListView = view.FindViewById<ListView>(Resource.Id.questListId);

            TitleTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            BackButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            QuestListView.Adapter = QuestListAdapter = new ResultQuestListAdapter(this.Activity, ViewModel);
            QuestListView.ItemClick += ItemClickHandler;
            QuestListView.ChildViewAdded += QuestAddedHandler;

            BackButton.Click += BackButtonOnClick;
            BackButton.Enabled = !ViewModel.InRoot;

            TitleTextDefault = Activity.GetString(Resource.String.QuestListTitle);

            return view;
        }

        #endregion

        #region Handlers

        private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            ResultQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(e.Child);
            holder.RestartButton.Click += (s, args) => { RestartHandler(holder.ItemPosition); };
            holder.DeleteButton.Click += (s, args) => { DeleteHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (s, args) => { ChildrenHandler(holder.ItemPosition); };
        }

        private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs e)
        {
            ResultQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(e.View);
            holder.ExpandDetails.Visibility = holder.ExpandDetails.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        }

        private void BackButtonOnClick(object sender, EventArgs eventArgs)
        {
            TraverseToParent();
        }

        private void RestartHandler(int position)
        {
            ViewModel.RestartQuest(ViewModel.CurrentChildren[position]);
            QuestListAdapter.NotifyDataSetChanged();
        }

        private void DeleteHandler(int position)
        {
            Toast.MakeText(this.Context, $"Delete of {position} clicked!", ToastLength.Short).Show();
        }

        private void ChildrenHandler(int position)
        {
            TraverseToChild(position);
        }

        #endregion
    }
}