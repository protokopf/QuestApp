using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model for editing quest.
    /// </summary>
    public class QuestEditViewModel : QuestAbstractActionViewModel
    {
        private readonly IRepositoryCommandsFactory _commandsFactory;
        private readonly IQuestRepository _questRepository;

        /// <summary>
        /// Receives quest model, quest validator and reference to repository command.
        /// </summary>
        /// <param name="questRepository"></param>
        /// <param name="questValidator"></param>
        /// <param name="repositoryCommands"></param>
        public QuestEditViewModel(
            IQuestRepository questRepository, 
            IQuestValidator<ClarifiedResponse<int>> questValidator, 
            IRepositoryCommandsFactory repositoryCommands) : base(questValidator)
        {
            questRepository.ThrowIfNull(nameof(questRepository));
            repositoryCommands.ThrowIfNull(nameof(repositoryCommands));
            _questRepository = questRepository;
            _commandsFactory = repositoryCommands;
        }

        #region QuestAbstractActionViewModel overriding

        public int QuestId { get; set; }

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        public override void Action()
        {
            _commandsFactory.
                UpdateQuest(QuestViewModel.Model).
                Execute();
        }

        ///<inheritdoc cref="QuestAbstractActionViewModel"/>
        protected override IQuestViewModel InitializeQuestViewModel()
        {
            Quest model = _questRepository.Get(q => q.Id == QuestId);
            QuestViewModel questViewModel = new QuestViewModel(model)
            {
                UseStartTime = model.StartTime != default(DateTime),
                UseDeadline = model.Deadline != default(DateTime)
            };
            return questViewModel;
        }

        #endregion
    }
}
