using System;
using System.Threading.Tasks;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.Adapters.Quests;
using Justus.QuestApp.View.Droid.ViewHolders.QuestItem;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment, that contains list of active quests.
    /// </summary>
    public class ActiveQuestsFragment : BaseTraverseQuestsFragment<ActiveQuestListViewModel, ActiveQuestViewHolder>, 
        IViewHolderClickManager<ActiveQuestViewHolder>
    {
        //private IntervalAbstractService _intervalService;

        #region Fragment overriding

        ///<inheritdoc/>
        protected override RecyclerView HandleRecyclerView(Android.Views.View fragmentView)
        {
            RecyclerView recView = fragmentView.FindViewById<RecyclerView>(Resource.Id.questRecyclerViewRefId);
            
            recView.SetLayoutManager(new LinearLayoutManager(this.Context));
            recView.SetAdapter(QuestsAdapter = new ActiveQuestsAdapter(this.Activity, ViewModel, this));

            DividerItemDecoration decor = new DividerItemDecoration(this.Context, DividerItemDecoration.Vertical);

            Drawable divider = ResourcesCompat.GetDrawable(Resources, Resource.Drawable.questItemDivider, null);

            decor.SetDrawable(divider);

            recView.AddItemDecoration(decor);

            return recView;
        }

        ///<inheritdoc/>
        protected override int GetLayoutId()
        {
            return Resource.Layout.QuestListFragmentLayout;
        }

        ///<inheritdoc/>
        public override void OnStop()
        {
            base.OnStop();
            //_intervalService.Stop();       
        }

        ///<inheritdoc/>
        public override void OnResume()
        {
            base.OnResume();
            //_intervalService.Start();
        }

        #endregion

        #region IViewHolderClickManager implementation

        ///<inheritdoc/>
        public void BindClickListeners(ActiveQuestViewHolder holder)
        {
            holder.ItemView.Click += (sender, e) => { holder.Toggle(); };

            holder.ChildrenButton.Click += (o, eventArgs) => { TraverseToChild(holder.ItemPosition); };

            holder.StartButton.Click += (o, eventArgs) => { StartHandler(holder.ItemPosition); };
            holder.DoneButton.Click += (o, eventArgs) => { DoneHandler(holder.ItemPosition); };
            holder.FailButton.Click += (o, eventArgs) => { FailHandler(holder.ItemPosition); };
            holder.DeleteButton.Click += (o, eventArgs) => { DeleteHandler(holder.ItemPosition); };
            holder.CancelButton.Click += (o, eventArgs) => { CancelHandler(holder.ItemPosition); };
        }

        ///<inheritdoc/>
        public void UnbindClickListeners(ActiveQuestViewHolder holder)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Handlers


        private async void StartHandler(int itemPosition)
        {
            await ViewModel.StartQuest(itemPosition);
            //Staring quest won't removing it from sequence, so we just notifying adapter about changes.
            QuestsAdapter.NotifyItemChanged(itemPosition);
        }

        private async void DoneHandler(int viewPosition)
        {
            await ViewModel.DoneQuest(viewPosition);
            ReactOnChangeItemThatRemovedOnlyFromRoot(viewPosition);
            while(ViewModel.IsRootHasState(State.Done))
            {
                TraverseToParent();
            }
        }

        private async void FailHandler(int viewPosition)
        {
            await ViewModel.FailQuest(viewPosition);
            ReactOnChangeItemThatRemovedOnlyFromRoot(viewPosition);
            while (ViewModel.IsRootHasState(State.Failed))
            {
                TraverseToParent();
            }
        }

        private async void CancelHandler(int viewPosition)
        {
            await ViewModel.CancelQuest(viewPosition);
            ReactOnChangeItemThatRemovedOnlyFromRoot(viewPosition);
        }

        protected override async void DeleteHandler(int position)
        {
            await ViewModel.DeleteQuest(position);
            ViewModel.Refresh();
            QuestsAdapter.NotifyItemRemoved(position);
            QuestsAdapter.NotifyItemRangeChanged(position, QuestsAdapter.ItemCount);
            while (ViewModel.IsRootHasState(State.Failed) || ViewModel.IsRootHasState(State.Done))
            {
                TraverseToParent();
            }
        }

        private void ReactOnChangeItemThatRemovedOnlyFromRoot(int position)
        {
            if (ViewModel.InTopRoot)
            {
                ViewModel.Refresh();
                QuestsAdapter.NotifyItemRemoved(position);
                QuestsAdapter.NotifyItemRangeChanged(position, QuestsAdapter.ItemCount);
            }
            else
            {
                QuestsAdapter.NotifyItemChanged(position);
            }
        }

        #endregion
    }
}