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
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Adapters
{
    public class QuestListAdapter : BaseAdapter<Quest>
    {
        private readonly Fragment _fragment;
        private readonly QuestListViewModel _listViewModel;

        public QuestListAdapter(Fragment fragment, QuestListViewModel listViewModel)
        {
            if (fragment == null)
            {
                throw new ArgumentNullException(nameof(fragment));
            }
            if (listViewModel == null)
            {
                throw new ArgumentNullException(nameof(listViewModel));
            }
            _fragment = fragment;
            _listViewModel = listViewModel;
        }

        public override Quest this[int position] => _listViewModel.CurrentChildren[position];

        public override int Count => _listViewModel.CurrentChildren.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            StubQuestItemViewHolder viewHolder;
            if (convertView == null)
            {
                convertView = _fragment.Activity.LayoutInflater.Inflate(
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
                List<Quest> quests = _listViewModel.CurrentChildren;
                viewHolder.Title.Text = quests[position].Title;
                viewHolder.TimeLeft.Text = FormLeftTime(quests[position].Deadline);
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