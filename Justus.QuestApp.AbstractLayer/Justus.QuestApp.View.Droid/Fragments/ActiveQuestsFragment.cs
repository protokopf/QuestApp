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
using Justus.QuestApp.View.Droid.Entities;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment, that contains list of active quests.
    /// </summary>
    public class ActiveQuestsFragment : BaseFragment<ActiveQuestListViewModel>
    {
        private ListView _listView;
        private TextView _headerTextView;
        private Button _backButton;
        private ActiveQuestListAdapter _adapter;
        private string HeaderDefault = String.Empty;

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);


            _listView = view.FindViewById<ListView>(Resource.Id.questListId);
            _headerTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);
            HeaderDefault = _headerTextView.Text;
            _backButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            _backButton.Enabled = ViewModel.CurrentQuest != null;
            _backButton.Click += BackButtonHandler;

            _listView.Adapter = _adapter = new ActiveQuestListAdapter(this, ViewModel);
            _listView.ItemClick += ItemClickHandler;
            _listView.ChildViewAdded += QuestAddedHandler;

            return view;
        }




        #region Handlers

        private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs args)
        {
            ActiveQuestItemViewHolder holder = _adapter.GetHolderByView(args.Child);
            holder.DoneButton.Click += (o, eventArgs) => { DoneClickHandler(holder.ItemPosition); };
            holder.EditButton.Click += (o, eventArgs) => { EditClickHandler(holder.ItemPosition); };
            holder.FailButton.Click += (o, eventArgs) => { FailClickHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenClickHandler(holder.ItemPosition); };
        }

        private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs args)
        {
            LinearLayout layout = args.View.FindViewById<LinearLayout>(Resource.Id.childItemLayout);
            layout.Visibility = layout.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        }

        private void BackButtonHandler(object sender, EventArgs e)
        {
            ViewModel.CurrentQuest = ViewModel.CurrentQuest.Parent;
            _headerTextView.Text = ViewModel.QuestsListTitle ?? HeaderDefault;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            _adapter.NotifyDataSetChanged();
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
            _headerTextView.Text = ViewModel.QuestsListTitle ?? HeaderDefault;
            _backButton.Enabled = ViewModel.CurrentQuest != null;
            _adapter.NotifyDataSetChanged();
        } 

        #endregion
    }
}