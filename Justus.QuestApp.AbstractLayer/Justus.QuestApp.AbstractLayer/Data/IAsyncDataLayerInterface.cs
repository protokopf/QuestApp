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
        /// Saves entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Save(TEntity entity);

        /// <summary>
        /// Saves all entities.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task SaveAll(List<TEntity> entities);

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
    }
}
