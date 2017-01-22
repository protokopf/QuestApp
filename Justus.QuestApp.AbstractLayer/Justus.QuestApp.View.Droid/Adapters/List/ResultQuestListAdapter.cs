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
    public class ResultQuestListAdapter : BaseQuestListAdapter<ResultsQuestListVIewModel>
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

        protected override Android.Views.View ConstructViewFromQuest(int position, Android.Views.View view, ViewGroup parent, Quest quest)
        {
            ResultQuestItemViewHolder holder = null;
            if (view == null)
            {
                view = _fragment.Activity.LayoutInflater.Inflate(Resource.Layout.ResultQuestListItemHeader, null,false);
                holder = new ResultQuestItemViewHolder(view, position);
                _holders.Add(view, holder);
            }
            else
            {
                holder = _holders[view];
            }
            FillViewHolder(holder, quest, position);
            return view;
        }

        public ResultQuestItemViewHolder GetHolderByView(Android.Views.View view)
        {
            ResultQuestItemViewHolder holder = null;
            _holders.TryGetValue(view, out holder);
            return holder;
        }

        private void FillViewHolder(ResultQuestItemViewHolder holder, Quest quest, int position)
        {
            holder.ItemPosition = position;
            holder.StartButton.Visibility = quest.Parent == null ? ViewStates.Visible : ViewStates.Gone;
            holder.Title.Text = quest.Title;
            holder.Description.Text = quest.Description;
            holder.ChildrenButton.Enabled = quest.Children != null;
            switch (quest.CurrentState)
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
    }
}