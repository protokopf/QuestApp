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
    public class GeneralQuestStorage : IDataAccessInterface<Quest>
    {
        private readonly IDataAccessInterface<Quest> _innerStorage;
        private List<Quest> _innerCache;
        private bool _shouldRetrieveAllData;

        /// <summary>
        /// Receives reference to data storage.
        /// </summary>
        /// <param name="dataStorage"></param>
        public GeneralQuestStorage(IDataAccessInterface<Quest> dataStorage)
        {
            if (dataStorage == null)
            {
                throw new ArgumentNullException(nameof(dataStorage));
            }
            _innerStorage = dataStorage;
            _shouldRetrieveAllData = true;
            _innerCache = null;
        }

        #region IDataAccessInterface implementation

        ///<inheritdoc/>
        public void Open(string pathToStorage)
        {
            if (string.IsNullOrWhiteSpace(pathToStorage))
            {
                throw new ArgumentException("Path should not be empty or null string.");
            }
            _innerStorage.Open(pathToStorage);
        }

        ///<inheritdoc/>
        public void Close()
        {
            _innerStorage.Close();
        }

        ///<inheritdoc/>
        public bool IsClosed()
        {
            return _innerStorage.IsClosed();
        }

        ///<inheritdoc/>
        public void Insert(Quest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _innerStorage.Insert(entity);
            _shouldRetrieveAllData = true;
        }

        ///<inheritdoc/>
        public void InsertAll(List<Quest> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            _innerStorage.InsertAll(entities);
            _shouldRetrieveAllData = true;
        }

        ///<inheritdoc/>
        public void Update(Quest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _innerStorage.Update(entity);
            _shouldRetrieveAllData = true;
        }

        ///<inheritdoc/>
        public void UpdateAll(List<Quest> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            _innerStorage.UpdateAll(entities);
            _shouldRetrieveAllData = true;
        }

        ///<inheritdoc/>
        public Quest Get(int id)
        {
            RefreshCache();
            return _innerCache.FirstOrDefault(quest => quest.Id == id);
        }

        ///<inheritdoc/>
        public List<Quest> GetAll()
        {
            RefreshCache();
            return _innerCache;
        }

        ///<inheritdoc/>
        public void Delete(int id)
        {
            _innerStorage.Delete(id);
            _shouldRetrieveAllData = true;
        }

        ///<inheritdoc/>
        public void DeleteAll()
        {
            _innerStorage.DeleteAll();
            _shouldRetrieveAllData = true;
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            Close();
        }

        #endregion

        #region Private methods

        private void RefreshCache()
        {
            if (_shouldRetrieveAllData)
            {
                _innerCache = _innerStorage.GetAll();
                if (_innerCache != null)
                {
                    foreach (Quest quest in _innerCache)
                    {
                        BindWithChildren(quest,_innerCache);
                    }
                }
                _shouldRetrieveAllData = false;
            }
        }

        private void BindWithChildren(Quest quest, List<Quest> others)
        {
            if (quest == null || others == null)
            {
                return;
            }
            quest.Children = new List<Quest>();
            int id = quest.Id;
            int othersLength = _innerCache.Count;
            for (int i = 0; i < othersLength; ++i)
            {
                Quest current = _innerCache[i];
                if (id == current.ParentId)
                {
                    current.Parent = quest;
                    quest.Children.Add(current);
                }
            }
        
        }

        #endregion
    }
}
