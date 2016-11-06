using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;

namespace Justus.QuestApp.AbstractLayer.Data
{
    /// <summary>
    /// Interface for data storages, that implement functionality in asynchronous way.
    /// </summary>
    public interface IAsyncDataLayerInterface<TEntity> : IDisposable where TEntity : IdentifiedEntity
    {
        /// <summary>
        /// Opens data storage.
        /// </summary>
        /// <param name="pathToStorage"></param>
        /// <returns></returns>
        Task Open(string pathToStorage);

        /// <summary>
        /// Closes data storage.
        /// </summary>
        /// <returns></returns>
        Task Close();

        /// <summary>
        /// Inserts entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Insert(TEntity entity);

        /// <summary>
        /// Inserts all entities.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task InsertAll(List<TEntity> entities);

        /// <summary>
        /// Updates entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(TEntity entity);

        /// <summary>
        /// Updates all entities.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task UpdateAll(List<TEntity> entities);

        /// <summary>
        /// Returns entity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Get(int id);

        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAll();

        /// <summary>
        /// Deletes by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <returns></returns>
        Task DeleteAll();
    }
}
