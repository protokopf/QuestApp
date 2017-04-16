using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Adapters.Quests;
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
        #region BaseTraverseQuestsFragment overriding

        ///<inheritdoc/>
        protected override RecyclerView HandleRecyclerView(Android.Views.View fragmentView)
        {
            RecyclerView recView = fragmentView.FindViewById<RecyclerView>(Resource.Id.questRecyclerViewRefId);
            recView.SetLayoutManager(new LinearLayoutManager(this.Context));
            recView.SetAdapter(QuestsAdapter = new ResultQuestsAdapter(this.Activity, ViewModel));
            return recView;
        }

        #endregion

        #region Handlers

        //private void QuestAddedHandler(object sender, ViewGroup.ChildViewAddedEventArgs e)
        //{
        //    ResultQuestItemViewHolder holder = QuestsAdapter.GetViewHolderByView(e.Child);
        //    holder.RestartButton.Click += (s, args) => { RestartHandler(holder.ItemPosition); };
        //    holder.DeleteButton.Click += (s, args) => { DeleteHandler(holder.ItemPosition); };
        //    holder.ChildrenButton.Click += (s, args) => { ChildrenHandler(holder.ItemPosition); };
        //}

        //private void ItemClickHandler(object sender, AdapterView.ItemClickEventArgs e)
        //{
        //    ResultQuestItemViewHolder holder = QuestsAdapter.GetViewHolderByView(e.View);
        //    holder.ExpandDetails.Visibility = holder.ExpandDetails.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        //}


        //private void RestartHandler(int position)
        //{
        //    ViewModel.RestartQuest(ViewModel.Leaves[position]);
        //    QuestsAdapter.NotifyDataSetChanged();
        //}

        //private void ChildrenHandler(int position)
        //{
        //    TraverseToChild(position);
        //}

        #endregion
    }
}