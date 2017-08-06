using System;
using System.Collections.Generic;

namespace Justus.QuestApp.AbstractLayer.Entities.Quest
{
    /// <summary>
    /// Abstract type for all possible quests types.
    /// </summary>
    public class Quest : IdentifiedEntity
    {
        /// <summary>
        /// Parents quest id.
        /// </summary>
        public virtual int? ParentId { get; set; }

        /// <summary>
        /// Quest title.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Description of the quest.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Reference to parent quest.
        /// </summary>
        public virtual Quest Parent { get; set; }

        /// <summary>
        /// Quest children.
        /// </summary>
        public virtual List<Quest> Children { get; set; }

        /// <summary>
        /// Time, until which quest should be done.
        /// </summary>
        public virtual DateTime? Deadline { get; set; }

        /// <summary>
        /// Time, when quest should start.
        /// </summary>
        public virtual DateTime? StartTime { get; set; }

        /// <summary>
        /// Current quest state.
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Points, whether quest important or not.
        /// </summary>
        public virtual bool IsImportant { get; set; }

        /// <summary>
        /// Progress of quest.
        /// </summary>
        public double Progress { get; set; }
    }
}
