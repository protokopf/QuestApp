using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;

namespace Justus.QuestApp.AbstractLayer.Data
{
    /// <summary>
    /// Interface to data layer.
    /// </summary>
    public interface IDataLayerInterface<TEntity> : IDisposable where TEntity : IdentifiedEntity
    {
        /// <summary>
        /// Opens data storage.
        /// </summary>
        /// <param name="pathToStorage"></param>
        void Open(string pathToStorage);

        /// <summary>
        /// Closes connection to data storage.
        /// </summary>
        void Close();

        /// <summary>
        /// Insert entity.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert all entities.
        /// </summary>
        /// <param name="entities"></param>
        void InsertAll(List<TEntity> entities);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Update all entities.
        /// </summary>
        /// <param name="entities"></param>
        void UpdateAll(List<TEntity> entities);

        /// <summary>
        /// Returns entity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(int id);

        /// <summary>
        /// Returns all entities from data storage.
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();

        /// <summary>
        /// Deletes entity by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        void DeleteAll();
    }
}
