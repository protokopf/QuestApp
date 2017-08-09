using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ViewModelLayer.Factories;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model for creating quest.
    /// </summary>
    public class QuestCreateViewModel : QuestAbstractActionViewModel
    {
        private readonly IQuestTree _questTree;
        private readonly ITreeCommandsFactory _commandsFactory;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestCreateViewModel(
            IQuestViewModelFactory questViewModelFactory,
            IQuestValidator<ClarifiedResponse<int>> questValidator, 
            IQuestTree questTree,
            ITreeCommandsFactory treeCommands) : base(questViewModelFactory, questValidator)
        {
            questTree.ThrowIfNull(nameof(questTree));
            treeCommands.ThrowIfNull(nameof(treeCommands));

            _questTree = questTree;
            _commandsFactory = treeCommands;
        }

        /// <summary>
        /// Id of quest, that will be parent of current creating quest.
        /// </summary>
        public int ParentId { get; set; }

        #region QuestAbstractActionViewModel overriding

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        public override void Action()
        {
            Quest model = QuestViewModel.Model;

            if (model != null)
            {
                Quest parentOfModel = _questTree.Get(q => q.Id == ParentId);
                ExecuteCommand(_commandsFactory.AddQuest(parentOfModel, model));
            }
        }

        #endregion
    }
}
