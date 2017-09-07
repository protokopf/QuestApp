using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for list of available quests.
    /// </summary>
    public class AvailableQuestListViewModel : QuestListViewModel
    {
        /// <summary>
        /// Receives references to repository, stateCommands and treeCommands factories.
        /// </summary>
        /// <param name="questListModel"></param>
        /// <param name="stateCommandsFactory"></param>
        /// <param name="treeCommandsFactory"></param>
        public AvailableQuestListViewModel(
            IQuestListModel questListModel,
            IStateCommandsFactory stateCommandsFactory,
            ITreeCommandsFactory treeCommandsFactory) : 
            base(questListModel, stateCommandsFactory, treeCommandsFactory)
        {

        }
    }
}
