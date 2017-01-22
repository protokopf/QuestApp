using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Media;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Fragments;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters.List
{
    public class ResultQuestListAdapter : BaseQuestListAdapter<ResultsQuestListVIewModel, ResultQuestItemViewHolder>
    {
        private readonly ResultQuestsFragment _fragment;
        private readonly Dictionary<Android.Views.View, ResultQuestItemViewHolder> _holders;

        private readonly string _failedStatus;
        private readonly string _doneStatus;

        public ResultQuestListAdapter(ResultQuestsFragment fragment, ResultsQuestListVIewModel listViewModel) : base(listViewModel)
        {
            if (fragment == null)
            {
                throw new ArgumentNullException(nameof(fragment));
            }
            _fragment = fragment;
            _holders = new Dictionary<Android.Views.View, ResultQuestItemViewHolder>();

            _doneStatus = _fragment.Activity.Resources.GetString(Resource.String.DoneStatus);
            _failedStatus = _fragment.Activity.Resources.GetString(Resource.String.FailedStatus);
        }

        #region BaseQuestListAdapter overriding

        ///<inheritdoc/>
        protected override Android.Views.View InflateView()
        {
            return _fragment.Activity.LayoutInflater.Inflate(Resource.Layout.ResultQuestListItemHeader, null, false);
        }

        ///<inheritdoc/>
        protected override ResultQuestItemViewHolder CreateViewHolder(Android.Views.View view, int position)
        {
            return new ResultQuestItemViewHolder(view, position);
        }

        ///<inheritdoc/>
        protected override void FillViewHolder(ResultQuestItemViewHolder holder, Quest questData, int position)
        {
            holder.ItemPosition = position;
            holder.StartButton.Visibility = questData.Parent == null ? ViewStates.Visible : ViewStates.Gone;
            holder.Title.Text = questData.Title;
            holder.Description.Text = questData.Description;
            holder.ChildrenButton.Enabled = questData.Children != null;
            switch (questData.CurrentState)
            {
                case QuestState.Done:
                    holder.Status.Text = _doneStatus;
                    holder.Status.SetTextColor(Color.Green);
                    break;
                case QuestState.Failed:
                    holder.Status.Text = _failedStatus;
                    holder.Status.SetTextColor(Color.Red);
                    break;
                default:
                    holder.Status.Text = "Not defined status";
                    holder.Status.SetTextColor(Color.Orange);
                    break;
            }
        }

        #endregion
    }
}