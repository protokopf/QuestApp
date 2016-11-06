using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.DataLayer.Entities;
using SQLite;

namespace Justus.QuestApp.DataLayer.Data
{
    /// <summary>
    /// Sqllite storage for quest items.
    /// </summary>
    public class SqliteDataStorage : IDataLayerInterface<Quest>
    {
        private SQLiteConnection _connection;

        #region IDataLayerInterface implementation

        ///<inheritdoc/>
        public void Open(string pathToStorage)
        {
            _connection = new SQLiteConnection(pathToStorage);
            _connection.CreateTable<SqliteQuest>();
        }

        ///<inheritdoc/>
        public void Close()
        {
            _connection.Close();
            _connection = null;
        }

        ///<inheritdoc/>
        public void Insert(Quest entity)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void InsertAll(List<Quest> entities)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void Update(Quest entity)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void UpdateAll(List<Quest> entities)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Quest Get(int id)
        {
            return _connection?.Get<SqliteQuest>(id);
        }

        ///<inheritdoc/>
        public List<Quest> GetAll()
        {
            return _connection?.Table<SqliteQuest>().Cast<Quest>().ToList();
        }

        ///<inheritdoc/>
        public void Delete(int id)
        {
            _connection?.Delete<SqliteQuest>(id);
        }

        ///<inheritdoc/>
        public void DeleteAll()
        {
            _connection?.DeleteAll<SqliteQuest>();
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
