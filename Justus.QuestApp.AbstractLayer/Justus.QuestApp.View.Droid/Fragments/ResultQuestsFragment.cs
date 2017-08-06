using System;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Android.Views;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers;
using Justus.QuestApp.View.Droid.Adapters.Quests;
using Justus.QuestApp.View.Droid.ViewHolders.QuestItem;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment for displaying result quests.
    /// </summary>
    public class ResultQuestsFragment : BaseTraverseQuestsFragment<ResultsQuestListViewModel, ResultQuestViewHolder>,
        IViewHolderClickManager<ResultQuestViewHolder>
    {
        #region BaseTraverseQuestsFragment overriding

        ///<inheritdoc/>
        protected override RecyclerView HandleRecyclerView(Android.Views.View fragmentView)
        {
            RecyclerView recView = fragmentView.FindViewById<RecyclerView>(Resource.Id.questRecyclerViewRefId);
            recView.SetLayoutManager(new LinearLayoutManager(this.Context));
            recView.SetAdapter(QuestsAdapter = new ResultQuestsAdapter(this.Activity, ViewModel, this));

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

        #endregion

        #region IViewHolderClickManager implementation

        ///<inheritdoc/>
        public void BindClickListeners(ResultQuestViewHolder holder)
        {
            holder.ItemView.Click += (o, e) => holder.Toggle();
            holder.RestartButton.Click += (s, args) => { RestartHandler(holder.ItemPosition); };
            holder.DeleteButton.Click += (s, args) => { DeleteHandler(holder.ItemPosition); };
            holder.ChildrenButton.Click += (s, args) => { ChildrenHandler(holder.ItemPosition); };
        }

        ///<inheritdoc/>
        public void UnbindClickListeners(ResultQuestViewHolder holder)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFabDecorator implementation

        ///<inheritdoc/>
        public void Manage(FloatingActionButton fab)
        {
            if (fab != null)
            {
                fab.Visibility = ViewStates.Gone;
            }
        }

        #endregion

        #region Handlers

        private void RestartHandler(int position)
        {
            ViewModel.StartQuest(position);

            //Only top level quests can be restarted, so they will be removed from Result category.
            QuestsAdapter.NotifyItemRemoved(position);
            QuestsAdapter.NotifyItemRangeChanged(position, QuestsAdapter.ItemCount);
        }

        private void ChildrenHandler(int position)
        {
            TraverseToChild(position);
        }

        #endregion
    }
}