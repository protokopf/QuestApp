using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Data
{
    /// <summary>
    /// Interface for types, which implement access to quest data layer.
    /// </summary>
    public interface IQuestDataLayer : IDisposable
    {
        /// <summary>
        /// Opens data storage.
        /// </summary>
        /// <param name="pathToStorage"></param>
        void Open();

        /// <summary>
        /// Closes connection to data storage.
        /// </summary>
        void Close();

        /// <summary>
        /// Determines, if access is closed.
        /// </summary>
        /// <returns></returns>
        bool IsClosed();

        /// <summary>
        /// Insert entity.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(Quest entity);

        /// <summary>
        /// Insert all entities.
        /// </summary>
        /// <param name="entities"></param>
        void InsertAll(IEnumerable<Quest> entities);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity"></param>
        void Update(Quest entity);

        /// <summary>
        /// Update all entities.
        /// </summary>
        /// <param name="entities"></param>
        void UpdateAll(IEnumerable<Quest> entities);

        /// <summary>
        /// Returns entity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Quest Get(int id);

        /// <summary>
        /// Returns all entities from data storage.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Quest> GetAll();

        /// <summary>
        /// Returns all child quests of some parent.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IEnumerable<Quest> GetAll(int parentId);

        /// <summary>
        /// Deletes entity by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Delete all child quests of some parent.
        /// </summary>
        /// <param name="parentId"></param>
        void DeleteAll(int parentId);
    }
}
