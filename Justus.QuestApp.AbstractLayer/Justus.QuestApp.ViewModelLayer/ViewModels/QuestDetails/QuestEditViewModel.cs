using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model for editing quest.
    /// </summary>
    public class QuestEditViewModel : QuestAbstractActionViewModel
    {
        private readonly IRepositoryCommandsFactory _commandsFactory;

        /// <summary>
        /// Receives quest model, quest validator and reference to repository command.
        /// </summary>
        /// <param name="questModel"></param>
        /// <param name="questValidator"></param>
        /// <param name="repositoryCommands"></param>
        public QuestEditViewModel(
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
            _commandsFactory.
                UpdateQuest(QuestDetails.QuestModel).
                Execute();
        }

        #endregion


    }
}
