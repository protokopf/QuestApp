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

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestCreateViewModel(
            Quest questModel, 
            IQuestValidator<ClarifiedResponse<int>> questValidator, 
            IRepositoryCommandsFactory repositoryCommands) : base(questModel, questValidator)
        {
            repositoryCommands.ThrowIfNull(nameof(repositoryCommands));
            _commandsFactory = repositoryCommands;
        }

        #region QuestAbstractActionViewModel overriding

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        public override void Action()
        {
            if (!QuestDetails.UseStartTime)
            {
                QuestDetails.StartTime = _defaultDateTime;
            }
            if (!QuestDetails.UseDeadline)
            {
                QuestDetails.Deadline = _defaultDateTime;
            }

            _commandsFactory.
                AddQuest(QuestDetails.QuestModel).
                Execute();
        }

        #endregion
    }
}
