using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model for creating quest.
    /// </summary>
    public class QuestCreateViewModel : QuestAbstractActionViewModel
    {
        private readonly DateTime _defaultDateTime = default(DateTime);

        private readonly IRepositoryCommandsFactory _commandsFactory;
        private readonly IQuestCreator _questCreator;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestCreateViewModel(
            IQuestCreator questCreator, 
            IQuestValidator<ClarifiedResponse<int>> questValidator, 
            IRepositoryCommandsFactory repositoryCommands) : base( questValidator)
        {
            repositoryCommands.ThrowIfNull(nameof(repositoryCommands));
            questCreator.ThrowIfNull(nameof(questCreator));
            _commandsFactory = repositoryCommands;
            _questCreator = questCreator;
        }

        /// <summary>
        /// Id of quest, that will be parent of current creating quest.
        /// </summary>
        public int ParentId { get; set; }

        #region QuestAbstractActionViewModel overriding

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        public override void Action()
        {
            if (!QuestViewModel.UseStartTime)
            {
                QuestViewModel.StartTime = _defaultDateTime;
            }
            if (!QuestViewModel.UseDeadline)
            {
                QuestViewModel.Deadline = _defaultDateTime;
            }

            Quest model = QuestViewModel.Model;
            model.ParentId = ParentId;

            _commandsFactory.
                AddQuest(model).
                Execute();
        }

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        protected override IQuestViewModel InitializeQuestViewModel()
        {
            Quest model = _questCreator.Create();
            model.ParentId = ParentId;
            return new QuestViewModel(model);
        }

        #endregion
    }
}
