using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Services;
using Justus.QuestApp.View.Droid.Adapters.List;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Services.ViewServices
{
    public class SimpleQuestExpireService : IntervalAbstractService
    {
        private readonly TaskScheduler _sheduler;
        private readonly QuestListViewModel _viewModel;
        private readonly BaseQuestListAdapter<ActiveQuestItemViewHolder> _adapter;

        private HashSet<int> _toUpdate;

        public SimpleQuestExpireService(int intervalMilliseconds, TaskScheduler sheduler, QuestListViewModel vm,
            BaseQuestListAdapter<ActiveQuestItemViewHolder> adapter) 
            : base(intervalMilliseconds)
        {
            _sheduler = sheduler;
            _viewModel = vm;
            _adapter = adapter;
            _toUpdate = new HashSet<int>();
        }

        protected override Task IntervalTaskOperation()
        {
            return Task.Run(() => FindQuestsToUpdate()).ContinueWith(UpdateViews, _sheduler);
        }

        private void FindQuestsToUpdate()
        {
            int length = _adapter.Count;
            for (int i = 0; i < length; ++i)
            {
                Quest currentQuest = _viewModel.CurrentChildren[i];
                if (currentQuest.Deadline > DateTime.Now && currentQuest.CurrentState == QuestState.Progress)
                {
                    _toUpdate.Add(i);
                }
            }
        }

        private void UpdateViews(Task task)
        {
            int i = 0;
            foreach (var viewHolder in _adapter.GetViewHolders())
            {
                if (_toUpdate.Contains(i))
                {
                    viewHolder.TimeLeft.Text = FormLeftTime(_viewModel.CurrentChildren[i].Deadline);
                }
            }
            _toUpdate.Clear();
        }

        private string FormLeftTime(DateTime deadLine)
        {
            TimeSpan s = deadLine - DateTime.Now;
            return $"{s.Hours}:{s.Minutes}:{s.Seconds}";
        }
    }
}