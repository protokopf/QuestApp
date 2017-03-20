using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for active quests from list.
    /// </summary>
    public class ActiveQuestListViewModel : QuestListViewModel
    {
        private readonly IQuestProgressCounter _progressCounter;

        public ActiveQuestListViewModel()
        {
            _progressCounter = ServiceLocator.Resolve<IQuestProgressCounter>();
        }

        #region QuestListViewModel overriding

        ///<inheritdoc/>
        protected override List<Quest> FilterQuests(List<Quest> quests)
        {
            return quests.Where(FilterEachQuest).ToList();
        }

        #endregion

        /// <summary>
        /// Count progress of quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public int CountProgress(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            ProgressValue value = _progressCounter.CountProgress(quest);
            int result = (int)(value.Current / (double)value.Total * 100);
            return result;
        }

        /// <summary>
        /// Fails given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void FailQuest(Quest quest)
        {
            LastCommand = StateCommads.FailQuest(quest);
            LastCommand.Execute();
            ResetChildren();
        }

        /// <summary>
        /// Make done given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void DoneQuest(Quest quest)
        {
            LastCommand = StateCommads.DoneQuest(quest);
            LastCommand.Execute();
            ResetChildren();
        }

        /// <summary>
        /// Cancels given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void CancelQuest(Quest quest)
        {
            LastCommand = StateCommads.CancelQuest(quest);
            LastCommand.Execute();
            ResetChildren();
        }

        /// <summary>
        /// Starts given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void StartQuest(Quest quest)
        {
            LastCommand = StateCommads.StartQuest(quest);
            LastCommand.Execute();
        }

        #region Private methods

        private bool FilterEachQuest(Quest quest)
        {
            QuestState state = quest.CurrentState;
            if (quest.Parent == null)
            {
                return state == QuestState.Progress;
            }
            return true;
        }

        #endregion
    }
}
