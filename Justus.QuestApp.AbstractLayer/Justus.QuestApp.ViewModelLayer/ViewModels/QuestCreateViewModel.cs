using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for creating quest.
    /// </summary>
    public class QuestCreateViewModel : BaseViewModel
    {
        private readonly IQuestCreator _questCreator;
        private readonly IRepositoryCommandsFactory _commandsFactory;
        private readonly IQuestRepository _questRepository;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestCreateViewModel(IQuestCreator questCreator, IRepositoryCommandsFactory repCommandsFactory, IQuestRepository repository)
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
            _questCreator = questCreator;
            _commandsFactory = repCommandsFactory;
            _questRepository = repository;
        }

        /// <summary>
        /// Id of parent quest.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Title of current quest.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of current quest.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Points, whether current quest important or not.
        /// </summary>
        public bool IsImportant { get; set; }

        /// <summary>
        /// Points, whether start time should be used.
        /// </summary>
        public bool UseStartTime { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Points, whether deadline should be used.
        /// </summary>
        public bool UseDeadline { get; set; }
        
        /// <summary>
        /// Deadline.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Resets view model state.
        /// </summary>
        public void Save()
        {
            Quest quest = _questCreator.Create();
            quest.Title = Title;
            quest.Description = Description;
            quest.IsImportant = IsImportant;

            if (UseStartTime)
            {
                quest.StartTime = StartTime;
            }
            if (UseDeadline)
            {
                quest.Deadline = Deadline;
            }

            Quest parent = _questRepository.Get(qt => qt.Id == ParentId);

            Command addCommand = _commandsFactory.AddQuest(quest, parent);

            addCommand.Execute();
        }
    }
}
