using System;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content.Res;
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
    /// Type for interacting with available quests.
    /// </summary>
    public class AvailableQuestsFragment : BaseTraverseQuestsFragment<AvailableQuestListViewModel, AvailableQuestItemViewHolder>
    {
        #region BaseTraverseQuestsFragment overriding

        ///<inehritdoc/>
        protected override RecyclerView HandleRecyclerView(Android.Views.View fragmentView)
        {
            RecyclerView recView = fragmentView.FindViewById<RecyclerView>(Resource.Id.questRecyclerViewRefId);
            recView.SetLayoutManager(new LinearLayoutManager(this.Context));
            recView.SetAdapter(QuestsAdapter = new AvailableQuestsAdapter(this.Activity, ViewModel));
            DividerItemDecoration decor = new DividerItemDecoration(this.Context, DividerItemDecoration.Vertical);
            Drawable dr = ResourcesCompat.GetDrawable(this.Resources,
                Resource.Drawable.questItemDivider, null);
            decor.SetDrawable(dr);
            recView.AddItemDecoration(decor);
            return recView;
        }

        #endregion

        #region Handlers

        //private void RecyclerViewOnChildViewAdded(object sender, ViewGroup.ChildViewAddedEventArgs args)
        //{
        //    AvailableQuestItemViewHolder holder = QuestsAdapter.GetViewHolderByView(args.Child);
        //    holder.DeleteButton.Click += (o, eventArgs) => { DeleteHandler(holder.ItemPosition); };
        //    holder.ChildrenButton.Click += (o, eventArgs) => { ChildrenHandler(holder.ItemPosition); };
        //    holder.StartButton.Click += (o, eventArgs) => { StartHandler(holder.ItemPosition); };
        //    holder.EditButton.Click += (o, eventArgs) => { EditHandler(holder.ItemPosition); };
        //}

        //private void EditHandler(int itemPosition)
        //{
        //    Toast.MakeText(this.Context, $"Edit of {itemPosition} clicked!", ToastLength.Short).Show();
        //}

        //private void StartHandler(int itemPosition)
        //{
        //    ViewModel.StartQuest(QuestsAdapter[itemPosition]);
        //    QuestsAdapter.NotifyDataSetChanged();
        //}

        //private void ChildrenHandler(int itemPosition)
        //{
        //    TraverseToChild(itemPosition);
        //}

        //private void QuestListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs args)
        //{
        //    AvailableQuestItemViewHolder holder = QuestsAdapter.GetViewHolderByView(args.View);
        //    holder.ExpandDetails.Visibility = holder.ExpandDetails.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
        //}

        //private void BackButtonOnClick(object sender, EventArgs eventArgs)
        //{
        //    TraverseToParent();
        //}

        #endregion
    }
}