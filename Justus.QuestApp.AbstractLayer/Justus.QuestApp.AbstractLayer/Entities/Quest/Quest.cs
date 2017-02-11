using System;
using System.Collections.Generic;

namespace Justus.QuestApp.AbstractLayer.Entities.Quest
{
    /// <summary>
    /// Abstract type for all possible quests types.
    /// </summary>
    public abstract class Quest : IdentifiedEntity
    {
        /// <summary>
        /// Parents quest id.
        /// </summary>
        public abstract int ParentId { get; set; }

        /// <summary>
        /// Quest title.
        /// </summary>
        public abstract string Title { get; set; }

        /// <summary>
        /// Description of the quest.
        /// </summary>
        public abstract string Description { get; set; }

        /// <summary>
        /// Reference to parent quest.
        /// </summary>
        public abstract Quest Parent { get; set; }

        /// <summary>
        /// Quest children.
        /// </summary>
        public abstract List<Quest> Children { get; set; }

        /// <summary>
        /// Time, until which quest should be done.
        /// </summary>
        public abstract DateTime Deadline { get; set; }

        /// <summary>
        /// Time, when quest should start.
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Current quest state.
        /// </summary>
        public virtual QuestState CurrentState { get; set; }

        /// <summary>
        /// Points, whether quest important or not.
        /// </summary>
        public virtual bool IsImportant { get; set; }
    }
}
