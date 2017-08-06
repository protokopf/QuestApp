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
    /// View model for editing quest.
    /// </summary>
    public class QuestEditViewModel : QuestAbstractActionViewModel
    {
        private readonly ITreeCommandsFactory _commandsFactory;
        private readonly IQuestTree _questTree;

        /// <summary>
        /// Receives quest model, quest validator and reference to repository command.
        /// </summary>
        /// <param name="questTree"></param>
        /// <param name="questViewModelFactory"></param>
        /// <param name="questValidator"></param>
        /// <param name="treeCommands"></param>
        public QuestEditViewModel(
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

        #region QuestAbstractActionViewModel overriding

        public int QuestId { get; set; }

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        public override void Action()
        {
            ICommand updateCommand = _commandsFactory.UpdateQuest(QuestViewModel.Model);
            if (updateCommand.Execute())
            {
                updateCommand.Commit();
            }
        }

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        public override void Initialize()
        {
            base.Initialize();
            Quest model = _questTree.Get(q => q.Id == QuestId);
            QuestViewModel.Model = model;
            QuestViewModel.UseStartTime = model.StartTime != null;
            QuestViewModel.UseDeadline = model.Deadline != null;
        }

        #endregion
    }
}
