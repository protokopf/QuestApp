using System;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for creating quest.
    /// </summary>
    public class QuestCreateViewModel : BaseViewModel
    {
        private readonly IQuestCreator _questCreator;
        private readonly IRepositoryCommandsFactory _commandsFactory;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestCreateViewModel(IQuestCreator questCreator, IRepositoryCommandsFactory repCommandsFactory)
        {
            if (questCreator == null)
            {
                throw new ArgumentNullException(nameof(questCreator));
            }
            if (repCommandsFactory == null)
            {
                throw new ArgumentNullException(nameof(repCommandsFactory));
            }
            _questCreator = questCreator;
            _commandsFactory = repCommandsFactory;
        }

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
        public void Reset()
        {
            Title = String.Empty;
            Description = String.Empty;
            IsImportant = false;
            UseDeadline = false;
            UseStartTime = false;
            StartTime = DateTime.MinValue;
            Deadline = DateTime.MinValue;
        }
    }
}
