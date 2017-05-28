using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model that represents action on quest.
    /// </summary>
    public abstract class QuestAbstractActionViewModel : BaseViewModel
    {
        private readonly IQuestValidator<ClarifiedResponse<int>> _questValidator;

        /// <summary>
        /// Receives references to quest model and quest validator.
        /// </summary>
        /// <param name="questModel"></param>
        /// <param name="questValidator"></param>
        protected QuestAbstractActionViewModel(Quest questModel, IQuestValidator<ClarifiedResponse<int>> questValidator)
        {
            questModel.ThrowIfNull(nameof(questModel));
            questValidator.ThrowIfNull(nameof(questValidator));

            _questValidator = questValidator;
            QuestDetails = new QuestViewModel(questModel);
        }

        /// <summary>
        /// Reference to quest view model.
        /// </summary>
        public QuestViewModel QuestDetails { get; }

        /// <summary>
        /// Abstract action on quest.
        /// </summary>
        public abstract void Action();

        /// <summary>
        /// Validates quest.
        /// </summary>
        /// <returns></returns>
        public ClarifiedResponse<int> Validate()
        {
            return _questValidator.Validate(QuestDetails.QuestModel);
        }
    }
}
