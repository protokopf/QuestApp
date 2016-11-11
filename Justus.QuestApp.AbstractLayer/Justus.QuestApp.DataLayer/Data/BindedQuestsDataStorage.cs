using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

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
                RecursiveInsert(entity.Children);
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
            _innerCache.AddRange(entities);
            RecursiveInsert(entities);
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
            RefreshCache();
            Quest toDelete = _innerCache.Find(x => x.Id == id);
            if (toDelete != null)
            {
                _innerCache.Remove(toDelete);
                if (toDelete.Children != null)
                {
                    RecursiveDelete(toDelete.Children);
                }
            }
            _innerStorage.Delete(id);
        }

        ///<inheritdoc/>
        public void DeleteAll()
        {
            ThrowIfClosed();
            _innerStorage.DeleteAll();
            _innerCache.Clear();
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

        private void RecursiveInsert(List<Quest> children)
        {
            if (children.Count > 0)
            {             
                foreach (Quest quest in children)
                {
                    SetId(quest);
                    if (quest.Parent != null)
                    {
                        quest.ParentId = quest.Parent.Id;
                    }
                    if (quest.Children != null)
                    {
                        RecursiveInsert(quest.Children);
                    }
                }
                _innerStorage.InsertAll(children);
            }
        }

        private void RecursiveDelete(List<Quest> children)
        {
            if (children.Count > 0)
            {
                foreach (Quest quest in children)
                {                   
                    if (quest.Children != null)
                    {
                        RecursiveDelete(quest.Children);
                    }
                    _innerStorage.Delete(quest.Id);
                }
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
            //If we should retrieve data
            if (_shouldRetrieveAllData)
            {
                //Retrieve data from 'low level' storage
                _innerCache = _innerStorage.GetAll();
                //Bind all entities
                if (_innerCache != null)
                {
                    CycleBinding(_innerCache);
                    //Remain only top-level entities.
                    _innerCache.RemoveAll(quest => quest.Parent != null);
                }        
                _shouldRetrieveAllData = false;
            }
        }

        private void CycleBinding(List<Quest> toBind)
        {
            int length = toBind.Count;
            for (int main = 0; main < length; ++main)
            {
                Quest mainQuest = toBind[main];
                mainQuest.Children = new List<Quest>();
                for (int sub = 0; sub < length; ++sub)
                {
                    Quest subQuest = toBind[sub];
                    if (subQuest.ParentId == mainQuest.Id)
                    {
                        mainQuest.Children.Add(subQuest);
                        subQuest.Parent = mainQuest;
                    }
                }
            }
        }

        #endregion
    }
}
