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
    public class BindedQuestsDataStorage : IDataAccessInterface<Quest>
    {
        private readonly IDataAccessInterface<Quest> _innerStorage;
        private List<Quest> _innerCache;
        private bool _shouldRetrieveAllData;
        private int _currentId;

        /// <summary>
        /// Receives reference to data storage.
        /// </summary>
        /// <param name="dataStorage"></param>
        public BindedQuestsDataStorage(IDataAccessInterface<Quest> dataStorage)
        {
            if (dataStorage == null)
            {
                throw new ArgumentNullException(nameof(dataStorage));
            }
            _innerStorage = dataStorage;
            _shouldRetrieveAllData = true;
            _innerCache = new List<Quest>();
        }

        #region IDataAccessInterface implementation

        ///<inheritdoc/>
        public void Open(string pathToStorage)
        {
            if (!IsClosed())
            {
                throw new InvalidOperationException("You should close existing connection first.");
            }
            if (string.IsNullOrWhiteSpace(pathToStorage))
            {
                throw new ArgumentException("Path should not be empty or null string.");
            }
            _innerStorage.Open(pathToStorage);
            InitializeId();
        }

        ///<inheritdoc/>
        public void Close()
        {
            if (!IsClosed())
            {
                _innerStorage.Close();
            }
        }

        ///<inheritdoc/>
        public bool IsClosed()
        {
            return _innerStorage.IsClosed();
        }

        ///<inheritdoc/>
        public void Insert(Quest entity)
        {
            ThrowIfClosed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            SetId(entity);
            _innerStorage.Insert(entity);
            _innerCache.Add(entity);

            if (entity.Children != null)
            {
                RecursiveInsert(entity,entity.Children);
            }                  
        }

        ///<inheritdoc/>
        public void InsertAll(List<Quest> entities)
        {
            ThrowIfClosed();
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _innerStorage.InsertAll(entities);
            _innerCache.AddRange(entities);

            foreach (Quest entity in entities)
            {
                if (entity.Children != null)
                {
                    RecursiveInsert(entity, entity.Children);
                }
            }
        }

        ///<inheritdoc/>
        public void Update(Quest entity)
        {
            ThrowIfClosed();
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
            ThrowIfClosed();
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
            ThrowIfClosed();
            RefreshCache();
            return _innerCache.FirstOrDefault(quest => quest.Id == id);
        }

        ///<inheritdoc/>
        public List<Quest> GetAll()
        {
            ThrowIfClosed();
            RefreshCache();
            return _innerCache;
        }

        ///<inheritdoc/>
        public void Delete(int id)
        {
            ThrowIfClosed();
            _innerStorage.Delete(id);
            _shouldRetrieveAllData = true;
        }

        ///<inheritdoc/>
        public void DeleteAll()
        {
            ThrowIfClosed();
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

        private void InitializeId()
        {
            List<Quest> storage = _innerStorage.GetAll();
            if (storage == null || storage.Count == 0)
            {
                _currentId = 1;
                return;
            }
            _currentId = storage.Max(x => x.Id) + 1;
        }

        private void SetId(Quest quest)
        {
            if (quest.Id == 0)
            {
                quest.Id = _currentId++;
            }
        }

        private void RecursiveInsert(Quest parent, List<Quest> children)
        {
            if (children.Count > 0)
            {             
                foreach (Quest quest in children)
                {
                    SetId(quest);
                    quest.ParentId = parent.Id;
                    if (quest.Children != null)
                    {
                        RecursiveInsert(quest, quest.Children);
                    }
                }
                _innerStorage.InsertAll(children);
            }
        }

        private void ThrowIfClosed()
        {
            if (IsClosed())
            {
                throw new InvalidOperationException("You should open connection first");
            }
        }

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
                    current.ParentId = quest.Id;
                    quest.Children.Add(current);
                }
            }
        
        }

        #endregion
    }
}
