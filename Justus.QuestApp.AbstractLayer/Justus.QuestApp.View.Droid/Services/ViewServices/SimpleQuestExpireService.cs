using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Services;
using Justus.QuestApp.View.Droid.Abstract.Adapters;
using Justus.QuestApp.View.Droid.Adapters.Quests;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Services.ViewServices
{
    public class SimpleQuestExpireService : IntervalAbstractService
    {
        private readonly TaskScheduler _sheduler;
        private readonly QuestListViewModel _viewModel;
        private readonly BaseQuestsAdapter<ActiveQuestViewHolder, ActiveQuestListViewModel> _adapter;

        private readonly HashSet<int> _toUpdate;

        public SimpleQuestExpireService(int intervalMilliseconds, TaskScheduler sheduler, ActiveQuestListViewModel vm,
            BaseQuestsAdapter<ActiveQuestViewHolder, ActiveQuestListViewModel> adapter) 
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
            int length = _adapter.ItemCount;
            for (int i = 0; i < length; ++i)
            {
                Quest currentQuest = _viewModel.Leaves[i];
                if (currentQuest.Deadline > DateTime.Now && currentQuest.CurrentState == QuestState.Progress)
                {
                    _toUpdate.Add(i);
                }
            }
        }

        private void UpdateViews(Task task)
        {
            //int i = 0;
            //foreach (var viewHolder in _adapter.GetViewHolders())
            //{
            //    if (_toUpdate.Contains(i))
            //    {
            //        viewHolder.TimeLeft.Text = FormLeftTime(_viewModel.Leaves[i].Deadline);
            //    }
            //}
            //_toUpdate.Clear();
        }

        private string FormLeftTime(DateTime deadLine)
        {
            TimeSpan s = deadLine - DateTime.Now;
            return $"{s.Hours}:{s.Minutes}:{s.Seconds}";
        }
    }
}