using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract
{
    /// <summary>
    /// Interface for Quest view model implementations.
    /// </summary>
    public interface IQuestViewModel
    {
        /// <summary>
        /// Title of current quest.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Description of current quest.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Points, whether current quest important or not.
        /// </summary>
        bool IsImportant { get; set; }

        /// <summary>
        /// Points, whether start time should be used.
        /// </summary>
        bool UseStartTime { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        DateTime? StartTime { get; set; }

        /// <summary>
        /// Points, whether deadline should be used.
        /// </summary>
        bool UseDeadline { get; set; }

        /// <summary>
        /// Deadline.
        /// </summary>
        DateTime? Deadline { get; set; }

        /// <summary>
        /// Reference to model.
        /// </summary>
        Quest Model { get; set; }
    }
}
