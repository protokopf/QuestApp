using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Services;
using Justus.QuestApp.View.Droid.Adapters.List;
using Justus.QuestApp.View.Droid.Fragments.Abstracts;
using Justus.QuestApp.View.Droid.Services.ViewServices;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using System.Threading.Tasks;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment, that contains list of active quests.
    /// </summary>
    public class ActiveQuestsFragment : BaseTraverseQuestsFragment<ActiveQuestListViewModel, ActiveQuestItemViewHolder>
    {
        private IntervalAbstractService _intervalService;

        #region Fragment overriding

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);

            QuestListView = view.FindViewById<ListView>(Resource.Id.questListId);
            TitleTextView = view.FindViewById<TextView>(Resource.Id.questsListTitle);          
            BackButton = view.FindViewById<Button>(Resource.Id.questsListBack);

            TitleTextDefault = Activity.GetString(Resource.String.QuestListTitle);

            BackButton.Enabled = !ViewModel.InRoot;
            BackButton.Click += BackButtonHandler;

            QuestListView.Adapter = QuestListAdapter = new ActiveQuestListAdapter(this.Activity, ViewModel);
            QuestListView.ItemClick += ItemClickHandler;
            QuestListView.ChildViewAdded += QuestAddedHandler;

            _intervalService = new SimpleQuestExpireService(1000, TaskScheduler.FromCurrentSynchronizationContext(),ViewModel, QuestListAdapter);

            return view;
        }

        ///<inheritdoc/>
        public override void OnPause()
        {
            base.OnPause();
            _intervalService.Stop();       
        }

        ///<inheritdoc/>
        public override void OnResume()
        {
            base.OnResume();
            _intervalService.Start();
        }

        #endregion

        #region Handlers

        private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs args)
        {
            ActiveQuestItemViewHolder holder = QuestListAdapter.GetViewHolderByView(args.Child);

            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenClickHandler(holder.ItemPosition); };

            holder.StartButton.Click += (o, eventArgs) => { StartHandler(holder.ItemPosition); };
            holder.DoneButton.Click += (o, eventArgs) => { DoneHandler(holder.ItemPosition); };
            holder.FailButton.Click += (o, eventArgs) => { FailHandler(holder.ItemPosition); };
            holder.DeleteButton.Click += (o, eventArgs) => { DeleteHandler(holder.ItemPosition); };
            holder.CancelButton.Click += (o, eventArgs) => { CancelHandler(holder.ItemPosition); };
        }

        private void StartHandler(int itemPosition)
        {
            ViewModel.StartQuest(ViewModel.CurrentChildren[itemPosition]);
            QuestListAdapter.NotifyDataSetChanged();
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

        private void DoneHandler(int viewPosition)
        {
            ViewModel.DoneQuest(ViewModel.CurrentChildren[viewPosition]);
            QuestListAdapter.NotifyDataSetChanged();
        }

        private void Undo(Android.Views.View view)
        {
            ViewModel.UndoLastCommand();
        }

        private void FailHandler(int viewPosition)
        {
            ViewModel.FailQuest(ViewModel.CurrentChildren[viewPosition]);
            QuestListAdapter.NotifyDataSetChanged();
        }

        private void ChildrenClickHandler(int viewPosition)
        {
            this.TraverseToChild(viewPosition);
        }

        private void CancelHandler(int viewPosition)
        {
            ViewModel.CancelQuest(ViewModel.CurrentChildren[viewPosition]);
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