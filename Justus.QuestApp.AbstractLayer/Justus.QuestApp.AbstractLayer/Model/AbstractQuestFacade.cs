using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Façade to quests.
    /// </summary>
    public abstract class AbstractQuestFacade : IDisposable
    {
        /// <summary>
        /// Returns quest by IdentifiedEntity object.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public abstract Quest GetQuest(Func<Quest> predicate);

        /// <summary>
        /// Saves quest.
        /// </summary>
        /// <param name="quest"></param>
        public abstract void SaveQuest(Quest quest);

        /// <summary>
        /// Returns list of quest with provided state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract List<Quest> GetQuestsByState(QuestState state);

        /// <summary>
        /// Retrieves all quests.
        /// </summary>
        /// <returns></returns>
        public abstract List<Quest> RetrieveAll();

        /// <summary>
        /// Saves all quests.
        /// </summary>
        /// <param name="quests"></param>
        public abstract void SaveAll(List<Quest> quests);

        /// <summary>
        /// Event, that takes place, when quantity of quests changed.
        /// </summary>
        public event Action QuantityChanged;

        /// <summary>
        /// Event, that occurs, when state of some quests changed.
        /// </summary>
        public event Action StateChanged;

        /// <summary>
        /// Disposes quest façade.
        /// </summary>
        public abstract void Dispose();
    }
}
