using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for list of done and failed quests.
    /// </summary>
    public class ResultsQuestListViewModel : QuestListViewModel
    {
        /// <summary>
        /// Receives references to list model, stateCommands and treeCommands factories.
        /// </summary>
        /// <param name="questListModel"></param>
        /// <param name="stateCommandsFactory"></param>
        /// <param name="treeCommandsFactory"></param>
        public ResultsQuestListViewModel(
            IQuestListModel questListModel,
            IStateCommandsFactory stateCommandsFactory,
            ITreeCommandsFactory treeCommandsFactory) : 
            base(questListModel, stateCommandsFactory, treeCommandsFactory)
        {
            
        }
    }
}
