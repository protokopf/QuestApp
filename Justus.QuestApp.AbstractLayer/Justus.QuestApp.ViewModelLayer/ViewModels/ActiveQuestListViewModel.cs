using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for active quests from list.
    /// </summary>
    public class ActiveQuestListViewModel : QuestListViewModel
    {
        private readonly IQuestProgressCounter _progressCounter;

        public ActiveQuestListViewModel(IQuestRepository repository,
            IStateCommandsFactory stateCommandsFactory,
            IRepositoryCommandsFactory repositoryCommandsFactory,
            IQuestProgressCounter questProgressCounter) : 
            base(repository, stateCommandsFactory,repositoryCommandsFactory)
        {
            if (questProgressCounter == null)
            {
                throw new ArgumentNullException(nameof(questProgressCounter));
            }
            _progressCounter = questProgressCounter;
        }

        #region QuestListViewModel overriding

        ///<inheritdoc/>
        protected override List<Quest> HandleQuests(List<Quest> quests)
        {
            return quests.Where(FilterEachQuest).ToList();
        }

        #endregion

        /// <summary>
        /// Points, whether all quests are done or not.
        /// </summary>
        /// <returns></returns>
        public bool IsRootDone()
        {
            if (InTopRoot)
            {
                return false;
            }
            return Root.CurrentState == QuestState.Done;
        }

        /// <summary>
        /// Count progress of quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public int CountProgress(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            ProgressValue value = _progressCounter.CountProgress(quest);
            int result = (int)(value.Current / (double)value.Total * 100);
            return result;
        }

        /// <summary>
        /// Fails given quest.
        /// </summary>
        /// <param name="quest"></param>
        public async Task FailQuest(Quest quest)
        {
            LastCommand = StateCommads.FailQuest(quest);
            IsBusy = true;
            await Task.Run(() =>
            {
                LastCommand.Execute();
                QuestRepository.Save();
                ResetChildren();
            });
            IsBusy = false;
        }

        /// <summary>
        /// Make done given quest.
        /// </summary>
        /// <param name="quest"></param>
        public async Task DoneQuest(Quest quest)
        {
            LastCommand = StateCommads.DoneQuest(quest);
            IsBusy = true;
            await Task.Run(() =>
            {
                LastCommand.Execute();
                QuestRepository.Save();
                ResetChildren();
            });
            IsBusy = false;
        }

        /// <summary>
        /// Cancels given quest.
        /// </summary>
        /// <param name="quest"></param>
        public async Task CancelQuest(Quest quest)
        {
            LastCommand = StateCommads.CancelQuest(quest);
            IsBusy = true;
            await Task.Run(() =>
            {
                LastCommand.Execute();
                QuestRepository.Save();
                ResetChildren();
            });
            IsBusy = false;
        }

        /// <summary>
        /// Starts given quest.
        /// </summary>
        /// <param name="quest"></param>
        public async Task StartQuest(Quest quest)
        {
            LastCommand = StateCommads.StartQuest(quest);
            IsBusy = true;
            await Task.Run(() =>
            {
                LastCommand.Execute();
                QuestRepository.Save();
            });
            IsBusy = false;

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
