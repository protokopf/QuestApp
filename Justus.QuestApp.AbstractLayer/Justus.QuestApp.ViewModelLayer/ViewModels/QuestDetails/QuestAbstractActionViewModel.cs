using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ViewModelLayer.Factories;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model that represents action on quest.
    /// </summary>
    public abstract class QuestAbstractActionViewModel : BaseViewModel
    {
        private readonly IQuestViewModelFactory _questViewModelFactory;
        private readonly IQuestValidator<ClarifiedResponse<int>> _questValidator;

        /// <summary>
        /// Receives references to the quest view models factory and quest validator.
        /// </summary>
        /// <param name="questViewModelFactory"></param>
        /// <param name="questValidator"></param>
        protected QuestAbstractActionViewModel(IQuestViewModelFactory questViewModelFactory, 
            IQuestValidator<ClarifiedResponse<int>> questValidator)
        {
            questViewModelFactory.ThrowIfNull(nameof(questViewModelFactory));
            questValidator.ThrowIfNull(nameof(questValidator));

            _questViewModelFactory = questViewModelFactory;
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
            QuestViewModel = _questViewModelFactory.CreateQuestViewModel();
        }

        /// <summary>
        /// Executes given command.
        /// </summary>
        /// <param name="command"></param>
        protected void ExecuteCommand(ICommand command)
        {
            if (command != null)
            {
                if (command.Execute())
                {
                    command.Commit();
                }
            }
        }
    }
}
