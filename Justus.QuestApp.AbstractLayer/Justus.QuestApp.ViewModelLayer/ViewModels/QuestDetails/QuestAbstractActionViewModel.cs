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
        protected QuestAbstractActionViewModel(IQuestValidator<ClarifiedResponse<int>> questValidator)
        {
            questValidator.ThrowIfNull(nameof(questValidator));

            _questValidator = questValidator;
        }

        /// <summary>
        /// Reference to quest view model.
        /// </summary>
        public IQuestViewModel QuestViewModel { get; private set; }

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
            return _questValidator.Validate(QuestViewModel.Model);
        }

        /// <summary>
        /// Initializes current view model.
        /// </summary>
        public virtual void Initialize()
        {
            QuestViewModel = InitializeQuestViewModel();
        }

        /// <summary>
        /// Returns initialized quest details view model.
        /// </summary>
        /// <returns></returns>
        protected abstract IQuestViewModel InitializeQuestViewModel();
    }
}
