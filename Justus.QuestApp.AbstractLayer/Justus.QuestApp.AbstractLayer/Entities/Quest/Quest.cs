using System.Collections.Generic;

namespace Justus.QuestApp.AbstractLayer.Entities.Quest
{
    /// <summary>
    /// Abstract type for all possible quests types.
    /// </summary>
    public abstract class Quest : IdentifiedEntity
    {
        /// <summary>
        /// Default constructor.Initializes Children list.
        /// </summary>
        protected Quest()
        {
            Children = new List<Quest>();
        }

        /// <summary>
        /// Quest title.
        /// </summary>
        public abstract string Title { get; set; }

        /// <summary>
        /// Description of the quest.
        /// </summary>
        public abstract string Description { get; set; }

        /// <summary>
        /// Try to progress quest, which change its state.
        /// </summary>
        public abstract void Progress();

        /// <summary>
        /// Current quest state.
        /// </summary>
        public QuestState CurrentState { get; set; }

        /// <summary>
        /// Quest children.
        /// </summary>
        public List<Quest> Children { get; }
    }
}
