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
        /// Current quest state.
        /// </summary>
        public QuestState CurrentState { get; set; }
    }
}
