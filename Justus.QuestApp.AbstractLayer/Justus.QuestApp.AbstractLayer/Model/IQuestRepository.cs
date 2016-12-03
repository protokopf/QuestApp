using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for access to Quest repository.
    /// </summary>
    public interface IQuestRepository : IDisposable
    {
        /// <summary>
        /// Insert quest in quest.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        void Insert(Quest quest);

        /// <summary>
        /// Insert all quests.
        /// </summary>
        /// <param name="quests"></param>
        /// <param name="parent"></param>
        void InsertAll(List<Quest> quests);

        /// <summary>
        /// Reverts insert of given quest.
        /// </summary>
        /// <param name="quest"></param>
        bool RevertInsert(Quest quest);

        /// <summary>
        /// Update quest.
        /// </summary>
        /// <param name="quest"></param>
        void Update(Quest quest);

        /// <summary>
        /// Update all quests.
        /// </summary>
        /// <param name="quests"></param>
        void UpdateAll(List<Quest> quests);

        /// <summary>
        /// Reverts update of quest.
        /// </summary>
        /// <param name="quest"></param>
        bool RevertUpdate(Quest quest);

        /// <summary>
        /// Returns quest by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Quest Get(int id);

        /// <summary>
        /// Returns all quests.
        /// </summary>
        /// <returns></returns>
        List<Quest> GetAll();

        /// <summary>
        /// Deletes quest.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Quest quest);

        /// <summary>
        /// Deletes all quests.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Reverts delete.
        /// </summary>
        /// <param name="id"></param>
        bool RevertDelete(Quest quest);

        /// <summary>
        /// Saves all made changes.
        /// </summary>
        void Save();

        /// <summary>
        /// Refreshes repository state.
        /// </summary>
        void Refresh();
    }
}
