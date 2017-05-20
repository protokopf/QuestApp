using System;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content.Res;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.Activities;
using Justus.QuestApp.View.Droid.Adapters.Quests;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Type for interacting with available quests.
    /// </summary>
    public class AvailableQuestsFragment : BaseTraverseQuestsFragment<AvailableQuestListViewModel, AvailableQuestViewHolder>,
        IViewHolderClickManager<AvailableQuestViewHolder>,
        IFabDecorator
    {
        private const int OkCancelRequestCode = 0;

        private FloatingActionButton _fab = null;
        
        #region BaseTraverseQuestsFragment overriding

        ///<inheritdoc/>
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == OkCancelRequestCode)
            {
                switch (resultCode)
                {
                    case (int)Result.Ok:
                        ViewModel.ResetChildren();
                        QuestsAdapter.NotifyDataSetChanged();
                        Toast.MakeText(this.Context, "New quest has been saved!", ToastLength.Short).Show();
                        break;
                    case (int)Result.Canceled:
                        Toast.MakeText(this.Context, "Cancel creating quest!", ToastLength.Short).Show();
                        break;
                }
            }
        }

        ///<inehritdoc/>
        protected override RecyclerView HandleRecyclerView(Android.Views.View fragmentView)
        {
            RecyclerView recView = fragmentView.FindViewById<RecyclerView>(Resource.Id.questRecyclerViewRefId);
            recView.SetLayoutManager(new LinearLayoutManager(this.Context));
            recView.SetAdapter(QuestsAdapter = new AvailableQuestsAdapter(this.Activity, ViewModel, this));

            DividerItemDecoration decor = new DividerItemDecoration(this.Context, DividerItemDecoration.Vertical);
            Drawable dr = ResourcesCompat.GetDrawable(this.Resources,
                Resource.Drawable.questItemDivider, null);
            decor.SetDrawable(dr);
            recView.AddItemDecoration(decor);
            return recView;
        }

        ///<inheritdoc/>
        protected override int GetLayoutId()
        {
            return Resource.Layout.QuestListFragmentLayout;
        }

        ///<inheritdoc/>
        public override void OnUnselect()
        {
            if (_fab != null)
            {
                _fab.Click -= FabOnClick;
                _fab = null;
            }
            base.OnUnselect();
        }

        #endregion

        #region IViewHolderClickManager implementation

        ///<inheritdoc/>
        public void BindClickListeners(AvailableQuestViewHolder holder)
        {
            holder.ItemView.Click += (sender, e) => holder.Toggle();
            holder.DeleteButton.Click += (o, eventArgs) =>  DeleteHandler(holder.ItemPosition);
            holder.ChildrenButton.Click += (o, eventArgs) => ChildrenHandler(holder.ItemPosition); 
            holder.StartButton.Click += (o, eventArgs) => StartHandler(holder.ItemPosition); 
            holder.EditButton.Click += (o, eventArgs) => EditHandler(holder.ItemPosition);
        }

        ///<inheritdoc/>
        public void UnbindClickListeners(AvailableQuestViewHolder holder)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFabDecorator implementation

        ///<inheritdoc/>
        public void Decorate(FloatingActionButton fab)
        {
            if (fab != null)
            {
                _fab = fab;
                _fab.Click += FabOnClick;
                _fab.Show();
            }
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            FragmentActivity activity = this.Activity;
            if (activity != null)
            {
                Intent startQuestInfo = QuestCreateActivity.GetStartIntent(ViewModel.RootId, Context);
                this.StartActivityForResult(startQuestInfo, OkCancelRequestCode);
            }
        }

        #endregion

        #region Handlers

        private void EditHandler(int itemPosition)
        {
            Toast.MakeText(this.Context, $"Edit of {itemPosition} clicked!", ToastLength.Short).Show();
        }

        private void StartHandler(int itemPosition)
        {
            ViewModel.StartQuest(ViewModel.Leaves[itemPosition]);
            QuestsAdapter.NotifyItemRemoved(itemPosition);
            QuestsAdapter.NotifyItemRangeChanged(itemPosition, QuestsAdapter.ItemCount);
        }

        private void ChildrenHandler(int itemPosition)
        {
            TraverseToChild(itemPosition);
        }

        #endregion
    }
}