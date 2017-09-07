using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.DataLayer.Entities;
using Justus.QuestApp.DataLayer.Platform;
using SQLite;
using SQLite.Net;
using SQLite.Net.Interop;

namespace Justus.QuestApp.DataLayer.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class SqliteOrmQuestDataLayer : IQuestDataLayer
    {
        private SQLiteConnection _innerConnection;
        private readonly string _connectionString;
        private readonly ISqLitePlatformFactory _sqlitePlatformFactory;
        private bool _isClosed = true;

        /// <summary>
        /// Receives connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlitePlatformFactory"></param>
        public SqliteOrmQuestDataLayer(string connectionString, ISqLitePlatformFactory sqlitePlatformFactory)
        {
            connectionString.ThrowIfNullOrWhiteSpace(nameof(connectionString));
            sqlitePlatformFactory.ThrowIfNull(nameof(sqlitePlatformFactory));
            _connectionString = connectionString;
            _sqlitePlatformFactory = sqlitePlatformFactory;
        }

        #region IQuestDataLayer implementation

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void Open()
        {
            if (_isClosed)
            {
                try
                {
                    _innerConnection = new SQLiteConnection(_sqlitePlatformFactory.Create(), _connectionString);
                    _innerConnection.CreateTable<SqliteQuest>();
                    _isClosed = false;
                }
                catch (Exception e)
                {
                    string message = e.Message;
                }

            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void Close()
        {
            if (!_isClosed)
            {
                _innerConnection.Close();
                _innerConnection = null;
                _isClosed = true;
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void Delete(int id)
        {
            if (!_isClosed)
            {
                _innerConnection.Delete<SqliteQuest>(id);
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void DeleteAll()
        {
            if (!_isClosed)
            {
                _innerConnection.DeleteAll<SqliteQuest>();
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void DeleteAll(int parentId)
        {
            if (!_isClosed)
            {
               _innerConnection.Table<SqliteQuest>().Delete(q => q.ParentId == parentId);
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public Quest Get(int id)
        {
            if (!_isClosed)
            {
                SqliteQuest quest = null;
                try
                {
                    quest = _innerConnection.Find<SqliteQuest>(q => q.Id == id);
                }
                catch (Exception e)
                {
                    string message = e.Message;
                }
                return quest;
            }
            return null;
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public IEnumerable<Quest> GetAll()
        {
            if (!_isClosed)
            {
                return _innerConnection.Table<SqliteQuest>();
            }
            return null;
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public IEnumerable<Quest> GetAll(int parentId)
        {
            if (!_isClosed)
            {
                return _innerConnection.Table<SqliteQuest>().Where(q => q.ParentId == parentId);
            }
            return null;
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void Insert(Quest entity)
        {
            if (!_isClosed && entity != null)
            {
                _innerConnection.Insert(entity);
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void InsertAll(IEnumerable<Quest> entities)
        {
            if (!_isClosed && entities != null)
            {
                _innerConnection.InsertAll(entities);
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public bool IsClosed()
        {
            return _isClosed;;
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void Update(Quest entity)
        {
            if (!_isClosed && entity != null)
            {
                _innerConnection.Update(entity);
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void UpdateAll(IEnumerable<Quest> entities)
        {
            if (!_isClosed && entities != null)
            {
                _innerConnection.UpdateAll(entities);
            }
        }

        ///<inheritdoc cref="IQuestDataLayer"/>
        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
