using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Services;
using Justus.QuestApp.View.Droid.Fragments.Abstracts;
using Justus.QuestApp.View.Droid.Services.ViewServices;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Justus.QuestApp.View.Droid.Adapters.Quests;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment, that contains list of active quests.
    /// </summary>
    public class ActiveQuestsFragment : BaseTraverseQuestsFragment<ActiveQuestListViewModel, ActiveQuestItemViewHolder>
    {
        //private IntervalAbstractService _intervalService;

        #region Fragment overriding

        ///<inheritdoc/>
        protected override RecyclerView HandleRecyclerView(Android.Views.View fragmentView)
        {
            RecyclerView recView = fragmentView.FindViewById<Android.Support.V7.Widget.RecyclerView>(Resource.Id.questRecyclerViewRefId);
            
            recView.SetLayoutManager(new LinearLayoutManager(this.Context));
            recView.SetAdapter(QuestsAdapter = new ActiveQuestsAdapter(this.Activity, ViewModel));
            return recView;
        }

        ///<inheritdoc/>
        public override void OnPause()
        {
            base.OnPause();
            //_intervalService.Stop();       
        }

        ///<inheritdoc/>
        public override void OnResume()
        {
            base.OnResume();
            //_intervalService.Start();
        }

        #endregion

        #region Handlers

        //private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs args)
        //{
        //    ActiveQuestItemViewHolder holder = QuestsAdapter.GetViewHolderByView(args.Child);

        //    holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenClickHandler(holder.ItemPosition); };

        //    holder.StartButton.Click += (o, eventArgs) => { StartHandler(holder.ItemPosition); };
        //    holder.DoneButton.Click += (o, eventArgs) => { DoneHandler(holder.ItemPosition); };
        //    holder.FailButton.Click += (o, eventArgs) => { FailHandler(holder.ItemPosition); };
        //    holder.DeleteButton.Click += (o, eventArgs) => { DeleteHandler(holder.ItemPosition); };
        //    holder.CancelButton.Click += (o, eventArgs) => { CancelHandler(holder.ItemPosition); };
        //}

        private void StartHandler(int itemPosition)
        {
            ViewModel.StartQuest(ViewModel.Leaves[itemPosition]);
            QuestsAdapter.NotifyDataSetChanged();
        }

        //private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs args)
        //{
        //    ActiveQuestItemViewHolder holder = QuestsAdapter.GetViewHolderByView(args.View);
        //    holder.ExpandDetails.Visibility = holder.ExpandDetails.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        //}



        private void DoneHandler(int viewPosition)
        {
            ViewModel.DoneQuest(ViewModel.Leaves[viewPosition]);
            QuestsAdapter.NotifyDataSetChanged();
        }

        private void Undo(Android.Views.View view)
        {
            ViewModel.UndoLastCommand();
        }

        private void FailHandler(int viewPosition)
        {
            ViewModel.FailQuest(ViewModel.Leaves[viewPosition]);
            QuestsAdapter.NotifyDataSetChanged();
        }

        private void ChildrenClickHandler(int viewPosition)
        {
            this.TraverseToChild(viewPosition);
        }

        private void CancelHandler(int viewPosition)
        {
            ViewModel.CancelQuest(ViewModel.Leaves[viewPosition]);
            QuestsAdapter.NotifyDataSetChanged();
        }

        #endregion
    }
}