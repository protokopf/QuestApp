using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.DataLayer.Entities;
using SQLite;

namespace Justus.QuestApp.DataLayer.Data
{
    /// <summary>
    /// Sqllite storage for quest items.
    /// </summary>
    public class SqliteQuestStorage : IDataAccessInterface<Quest>
    {
        private SQLiteConnection _connection;
        private bool _isClosed = true;

        #region IDataAccessInterface implementation

        ///<inheritdoc/>
        public void Open(string pathToStorage)
        {
            _connection = new SQLiteConnection(pathToStorage);
            _connection.CreateTable<SqliteQuest>();
            _isClosed = false;
        }

        ///<inheritdoc/>
        public void Close()
        {
            if (_connection == null)
            {
                return;
            }
            _connection.Close();
            _connection = null;
            _isClosed = true;
        }

        ///<inheritdoc/>
        public bool IsClosed()
        {
            return _isClosed;
        }

        ///<inheritdoc/>
        public void Insert(Quest entity)
        {
            _connection?.Insert(entity);
        }

        ///<inheritdoc/>
        public void InsertAll(List<Quest> entities)
        {
            _connection?.InsertAll(entities);
        }

        ///<inheritdoc/>
        public void Update(Quest entity)
        {
            _connection?.Update(entity);
        }

        ///<inheritdoc/>
        public void UpdateAll(List<Quest> entities)
        {
            _connection?.UpdateAll(entities);
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
