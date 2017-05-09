using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for list of available quests.
    /// </summary>
    public class AvailableQuestListViewModel : QuestListViewModel
    {
        /// <summary>
        /// Receives references to repository, stateCommands and repositoryCommands factories.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="stateCommandsFactory"></param>
        /// <param name="repositoryCommandsFactory"></param>
        public AvailableQuestListViewModel(
            IQuestRepository repository,
            IStateCommandsFactory stateCommandsFactory,
            IRepositoryCommandsFactory repositoryCommandsFactory) : 
            base(repository, stateCommandsFactory, repositoryCommandsFactory)
        {

        }

        #region QuestListViewModel overriding

        ///<inheritdoc/>
        protected override List<Quest> FilterQuests(List<Quest> quests)
        {
            return
                quests.Where(FilterItem)
                    .ToList();
        }

        #endregion

        /// <summary>
        /// Starts given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void StartQuest(Quest quest)
        {
            LastCommand = StateCommads.StartQuest(quest);
            LastCommand.Execute();
            ResetChildren();
        }

        #region Private methods

        private bool FilterItem(Quest quest)
        {
            if (quest.Parent == null)
            {
                return quest.CurrentState == QuestState.Idle;
            }
            return true;
        } 

        #endregion
    }
}
