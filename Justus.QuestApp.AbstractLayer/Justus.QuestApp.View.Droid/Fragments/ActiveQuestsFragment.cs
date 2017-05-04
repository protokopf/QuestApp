using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Services;
using Justus.QuestApp.View.Droid.Services.ViewServices;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.Adapters.Quests;

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

            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenClickHandler(holder.ItemPosition); };

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


        private void StartHandler(int itemPosition)
        {
            ViewModel.StartQuest(ViewModel.Leaves[itemPosition]);
            //Staring quest won't removing it from sequence, so we just notifying adapter about changes.
            QuestsAdapter.NotifyItemChanged(itemPosition);
        }

        private void DoneHandler(int viewPosition)
        {
            ViewModel.DoneQuest(ViewModel.Leaves[viewPosition]);
            if (ViewModel.InRoot)
            {
                //If we are in the root of quests hierarchy, quest will be removed after marked as done 
                //(moved to another category), so we need notify adapter about removing item.
                QuestsAdapter.NotifyItemRemoved(viewPosition);
                QuestsAdapter.NotifyItemRangeChanged(viewPosition, QuestsAdapter.ItemCount);
            }
            else
            {
                //In other cases quest should be just changed, about what we are notifying
                //adapter.
                QuestsAdapter.NotifyItemChanged(viewPosition);
            }          
        }

        private void Undo(Android.Views.View view)
        {
            ViewModel.UndoLastCommand();
        }

        private void FailHandler(int viewPosition)
        {
            ViewModel.FailQuest(ViewModel.Leaves[viewPosition]);
            if (ViewModel.InRoot)
            {
                //If we are in the root of quests hierarchy, quest will be removed after marked as failed 
                //(moved to another category), so we need notify adapter about removing item.
                QuestsAdapter.NotifyItemRemoved(viewPosition);
                QuestsAdapter.NotifyItemRangeChanged(viewPosition, QuestsAdapter.ItemCount);
            }
            else
            {
                //In other cases quest should be just changed, about what we are notifying
                //adapter.
                QuestsAdapter.NotifyItemChanged(viewPosition);
            }
        }

        private void ChildrenClickHandler(int viewPosition)
        {
            this.TraverseToChild(viewPosition);
        }

        private void CancelHandler(int viewPosition)
        {
            ViewModel.CancelQuest(ViewModel.Leaves[viewPosition]);
            QuestsAdapter.NotifyItemRemoved(viewPosition);
            QuestsAdapter.NotifyItemRangeChanged(viewPosition, QuestsAdapter.ItemCount);
        }

        #endregion
    }
}