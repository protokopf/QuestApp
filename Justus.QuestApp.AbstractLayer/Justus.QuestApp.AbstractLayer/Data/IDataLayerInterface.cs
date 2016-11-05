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
        /// Saves entity.
        /// </summary>
        /// <param name="entity"></param>
        void Save(TEntity entity);

        /// <summary>
        /// Saves all entities.
        /// </summary>
        /// <param name="entities"></param>
        void SaveAll(List<TEntity> entities);

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
    }
}
