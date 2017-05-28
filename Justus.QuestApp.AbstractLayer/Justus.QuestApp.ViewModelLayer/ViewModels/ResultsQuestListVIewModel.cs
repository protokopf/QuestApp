using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands.Repository;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for list of done and failed quests.
    /// </summary>
    public class ResultsQuestListViewModel : QuestListViewModel
    {
        /// <summary>
        /// Receives references to repository, stateCommands and repositoryCommands factories.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="stateCommandsFactory"></param>
        /// <param name="repositoryCommandsFactory"></param>
        public ResultsQuestListViewModel(
            IQuestRepository repository,
            IStateCommandsFactory stateCommandsFactory,
            IRepositoryCommandsFactory repositoryCommandsFactory) : 
            base(repository, stateCommandsFactory, repositoryCommandsFactory)
        {
            
        }

        #region QuestListViewModel overriding

        ///<inheritdoc/>
        protected override List<Quest> HandleQuests(List<Quest> quests)
        {
            return
                quests.Where(FilterItem)
                    .ToList();
        }

        #endregion

        /// <summary>
        /// Restarts quest.
        /// </summary>
        /// <param name="quest"></param>
        public void RestartQuest(Quest quest)
        {
            Command cancel = StateCommads.CancelQuest(quest);
            Command start = StateCommads.StartQuest(quest);
            LastCommand = new CompositeCommand(new[] { cancel, start });
            LastCommand.Execute();
            ResetChildren();
        }

        #region Private methods

        private bool FilterItem(Quest quest)
        {
            if (quest.Parent == null)
            {
                return quest.CurrentState == QuestState.Done || quest.CurrentState == QuestState.Failed;
            }
            return true;
        }

        #endregion
    }
}
