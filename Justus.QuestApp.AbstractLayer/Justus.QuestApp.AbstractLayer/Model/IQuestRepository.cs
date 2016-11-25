using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for access to Quest repository.
    /// </summary>
    public interface IQuestRepository : IDisposable
    {
        /// <summary>
        /// Insert quest.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(Quest entity);

        /// <summary>
        /// Insert all quests.
        /// </summary>
        /// <param name="entities"></param>
        void InsertAll(List<Quest> entities);

        /// <summary>
        /// Update quest.
        /// </summary>
        /// <param name="entity"></param>
        void Update(Quest entity);

        /// <summary>
        /// Update all quests.
        /// </summary>
        /// <param name="entities"></param>
        void UpdateAll(List<Quest> entities);

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
        /// Deletes quest by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Deletes all quests.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Saves all made changes.
        /// </summary>
        void Save();
    }
}
