using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for creating quest.
    /// </summary>
    public class QuestCreateViewModel : BaseViewModel
    {
        private readonly DateTime _defaultDateTime = default(DateTime);
        private readonly Quest _innerQuest = null;

        private readonly IQuestCreator _questCreator;
        private readonly IRepositoryCommandsFactory _commandsFactory;
        private readonly IQuestRepository _questRepository;
        private readonly IQuestValidator<ClarifiedResponse<string>> _questValidtor;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestCreateViewModel(IQuestCreator questCreator, IRepositoryCommandsFactory repCommandsFactory, IQuestRepository repository,
            IQuestValidator<ClarifiedResponse<string>> questValidator)
        {
            if (questCreator == null)
            {
                throw new ArgumentNullException(nameof(questCreator));
            }
            if (repCommandsFactory == null)
            {
                throw new ArgumentNullException(nameof(repCommandsFactory));
            }
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (questValidator == null)
            {
                throw new ArgumentNullException(nameof(questValidator));
            }
            _questCreator = questCreator;
            _commandsFactory = repCommandsFactory;
            _questRepository = repository;
            _questValidtor = questValidator;
            _innerQuest = _questCreator.Create();
        }

        /// <summary>
        /// Id of parent quest.
        /// </summary>
        public int ParentId
        {
            get { return _innerQuest.ParentId; }
            set { _innerQuest.ParentId = value; }
        }

        /// <summary>
        /// Title of current quest.
        /// </summary>
        public string Title
        {
            get { return _innerQuest.Title; }
            set { _innerQuest.Title = value; }
        }

        /// <summary>
        /// Description of current quest.
        /// </summary>
        public string Description
        {
            get { return _innerQuest.Description; }
            set { _innerQuest.Description = value; }
        }

        /// <summary>
        /// Points, whether current quest important or not.
        /// </summary>
        public bool IsImportant
        {
            get { return _innerQuest.IsImportant; }
            set { _innerQuest.IsImportant = value; }
        }

        /// <summary>
        /// Points, whether start time should be used.
        /// </summary>
        public bool UseStartTime { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime StartTime
        {
            get { return _innerQuest.StartTime; }
            set { _innerQuest.StartTime = value; }
        }

        /// <summary>
        /// Points, whether deadline should be used.
        /// </summary>
        public bool UseDeadline { get; set; }
        
        /// <summary>
        /// Deadline.
        /// </summary>
        public DateTime Deadline
        {
            get { return _innerQuest.Deadline; }
            set { _innerQuest.Deadline = value; }
        }

        /// <summary>
        /// Validates quest and returns validation response.
        /// </summary>
        /// <returns></returns>
        public ClarifiedResponse<string> Validate()
        {
            return _questValidtor.Validate(_innerQuest);
        }

        /// <summary>
        /// Resets view model state.
        /// </summary>
        public void Save()
        {
            if (!UseStartTime)
            {
                _innerQuest.StartTime = _defaultDateTime;
            }
            if (!UseDeadline)
            {
                _innerQuest.Deadline = _defaultDateTime;
            }

            Quest parent = _questRepository.Get(qt => qt.Id == ParentId);

            Command addCommand = _commandsFactory.AddQuest(_innerQuest, parent);

            addCommand.Execute();
        }
    }
}
