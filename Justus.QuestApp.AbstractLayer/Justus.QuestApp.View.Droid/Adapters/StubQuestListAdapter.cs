using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Entities;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Adapters
{
    public class StubQuestListAdapter : BaseAdapter<Quest>
    {
        private Fragment mFragment;
        private List<Quest> mQuests; 

        public StubQuestListAdapter(Fragment fragment, List<Quest> quests)
        {
            mFragment = fragment;
            mQuests = quests;
        }

        public override Quest this[int position] => mQuests[position];

        public override int Count => mQuests.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            StubQuestItemViewHolder viewHolder;
            if (convertView == null)
            {
                convertView = mFragment.Activity.LayoutInflater.Inflate(
                    Resource.Layout.QuestListItemHeader, null, false);
                viewHolder = new StubQuestItemViewHolder(convertView);
                convertView.SetTag(Resource.Id.questListItemHeader, viewHolder);
            }
            else
            {
                viewHolder = convertView.GetTag(Resource.Id.questListItemHeader) as StubQuestItemViewHolder;
            }
            if (viewHolder != null)
            {
                viewHolder.Title.Text = mQuests[position].Title;
                viewHolder.TimeLeft.Text = FormLeftTime(mQuests[position].Deadline);
                viewHolder.Progress.Progress = 25;
            }

            return convertView;
        }

        private string FormLeftTime(DateTime deadLine)
        {
            TimeSpan s = deadLine - DateTime.Now;
            return $"{s.Hours}:{s.Minutes}:{s.Seconds}";
        }
    }
}