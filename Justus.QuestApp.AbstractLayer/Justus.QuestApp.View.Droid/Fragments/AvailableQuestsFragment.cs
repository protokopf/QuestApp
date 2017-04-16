using System;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.Adapters.Quests;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Type for interacting with available quests.
    /// </summary>
    public class AvailableQuestsFragment : BaseTraverseQuestsFragment<AvailableQuestListViewModel, AvailableQuestViewHolder>,
        IViewHolderClickManager<AvailableQuestViewHolder>
    {
        #region BaseTraverseQuestsFragment overriding

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

        #endregion

        #region IViewHolderClickManager implementation

        ///<inheritdoc/>
        public void BindClickListeners(AvailableQuestViewHolder holder)
        {
            holder.ItemView.Click += (sender, e) => holder.Toggle();
            holder.DeleteButton.Click += (o, eventArgs) => { DeleteHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenHandler(holder.ItemPosition); };
            holder.StartButton.Click += (o, eventArgs) => { StartHandler(holder.ItemPosition); };
            holder.EditButton.Click += (o, eventArgs) => { EditHandler(holder.ItemPosition); };
        }

        ///<inheritdoc/>
        public void UnbindClickListeners(AvailableQuestViewHolder holder)
        {
            throw new NotImplementedException();
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