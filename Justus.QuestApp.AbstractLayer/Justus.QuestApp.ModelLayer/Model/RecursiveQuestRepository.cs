﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Model
{
    /* Основные условия для данного типа:
     * - Изменения сохраняются только при вызове метода Save()/Dispose();
     * - При первой попытке получения данных тип обращается к внутренней реализации и извлекает оттуда информацию о квестах.
     * - В течении работы с объектом репозитория  во внутренние списки сохраняется информация об изменениях - ссылки на удаленные, изменненные, добавленные квесты.
     * - При "освобождении" или "сохранении" объекта репозитория все изменения переносятся на внутреннее хранилище, для чего оно открывается,
     * а затем закрывается.
     * - Присваиваем Id только тем квестам, которые должны сохраняться. Присваивание происходит при сохранении квестов в репозиторий.
     */

    /// <summary>
    /// Gives access to recursive quest repository.
    /// </summary>
    public class RecursiveQuestRepository : IQuestRepository
    {
        /// <summary>
        /// Reference to data storage interface.
        /// </summary>
        private readonly IDataAccessInterface<Quest> _dataStorage;

        private readonly string _connectionString;
        private readonly object _dataLocker = new object();

        /// <summary>
        /// Inner quest cache.
        /// </summary>
        private List<Quest> _questList;

        private readonly HashSet<int> _toDelete;
        private readonly List<Quest> _toUpdate;
        private readonly List<Quest> _toInsert;

        private bool _shouldDeleteAll;

        private int _currentId;

        /// <summary>
        /// Receives data storage interface injection.
        /// </summary>
        /// <param name="dataStorageInterface"></param>
        /// <param name="connectionString"></param>
        public RecursiveQuestRepository(IDataAccessInterface<Quest> dataStorageInterface, string connectionString)
        {
            if (dataStorageInterface == null)
            {
                throw new ArgumentNullException(nameof(dataStorageInterface));
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string should not be null or empty.");
            }
            _dataStorage = dataStorageInterface;
            _connectionString = connectionString;

            _toDelete = new HashSet<int>();
            _toUpdate = new List<Quest>();
            _toInsert = new List<Quest>();

            _questList = new List<Quest>();
            _currentId = 1;
        }

        #region IQuestRepository implementation

        ///<inheritdoc/>
        public void Insert(Quest quest)
        {
            lock (_dataLocker)
            {
                if (quest == null)
                {
                    throw new ArgumentNullException(nameof(quest));
                }

                SetId(quest);

                _toInsert.Add(quest);

                if (quest.Children != null)
                {
                    RecursiveInsert(quest.Children);
                }

                _questList.Add(quest);
            }
        }

        ///<inheritdoc/>
        public void InsertAll(List<Quest> quests)
        {
            lock (_dataLocker)
            {
                if (quests == null)
                {
                    throw new ArgumentNullException(nameof(quests));
                }
                RecursiveInsert(quests);
                _questList.AddRange(quests);
            }
        }

        ///<inheritdoc/>
        public bool RevertInsert(Quest quest)
        {
            lock (_dataLocker)
            {
                return _toInsert.Remove(quest);
            }
        }

        ///<inheritdoc/>
        public void Update(Quest quest)
        {
            lock (_dataLocker)
            {
                if (quest == null)
                {
                    throw new ArgumentNullException(nameof(quest));
                }
                _toUpdate.Add(quest);
            }
        }

        ///<inheritdoc/>
        public void UpdateAll(List<Quest> quests)
        {
            lock (_dataLocker)
            {
                if (quests == null)
                {
                    throw new ArgumentNullException(nameof(quests));
                }
                _toUpdate.AddRange(quests);
            }
        }

        ///<inheritdoc/>
        public bool RevertUpdate(Quest quest)
        {
            lock (_dataLocker)
            {
                return _toUpdate.Remove(quest);
            }
        }

        ///<inheritdoc/>
        public Quest Get(int id)
        {
            lock (_dataLocker)
            {
                return _questList.Find(quest => quest.Id == id);
            }
        }

        ///<inheritdoc/>
        public List<Quest> GetAll()
        {
            lock (_dataLocker)
            {
                return _questList;
            }
        }

        ///<inheritdoc/>
        public void Delete(Quest quest)
        {
            lock (_dataLocker)
            {
                if (quest == null)
                {
                    throw new ArgumentNullException(nameof(quest));
                }
                Quest parentOfDeleted = quest.Parent;
                if (parentOfDeleted == null)
                {
                    _questList.Remove(quest);
                }
                else
                {
                    parentOfDeleted.Children.Remove(quest);
                }
                _toDelete.Add(quest.Id);
            }
        }

        ///<inheritdoc/>
        public void DeleteAll()
        {
            lock (_dataLocker)
            {
                _shouldDeleteAll = true;
            }
        }

        ///<inheritdoc/>
        public bool RevertDelete(Quest quest)
        {
            lock (_dataLocker)
            {
                return _toDelete.Remove(quest.Id);
            }
        }

        ///<inheritdoc/>
        public void PushQuests()
        {
            lock (_dataLocker)
            {
                int totalCount = _toDelete.Count + _toUpdate.Count + _toInsert.Count;
                if (totalCount > 0 || _shouldDeleteAll)
                {
                    using (_dataStorage)
                    {
                        _dataStorage.Open(_connectionString);

                        if (_toInsert.Count > 0)
                        {
                            _dataStorage.InsertAll(_toInsert);
                            _toInsert.Clear();
                        }

                        if (_toUpdate.Count > 0)
                        {
                            _dataStorage.UpdateAll(_toUpdate);
                            _toUpdate.Clear();
                        }

                        if (_shouldDeleteAll)
                        {
                            _dataStorage.DeleteAll();
                            _shouldDeleteAll = false;
                        }
                        else
                        {
                            foreach (int questId in _toDelete)
                            {
                                _dataStorage.Delete(questId);
                            }
                        }
                        _toDelete.Clear();
                    }
                }
            }
        }

        ///<inheritdoc/>
        public void PullQuests()
        {
            lock (_dataLocker)
            {
                using (_dataStorage)
                {
                    _dataStorage.Open(_connectionString);
                    _questList = _dataStorage.GetAll();
                }

                if (_questList == null)
                {
                    _questList = new List<Quest>();
                }

                InitializeId(_questList);

                CycleBinding(_questList);
                _questList.RemoveAll(quest => quest.Parent != null);
            }
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            PushQuests();
        }

        #endregion

        #region Private methods

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
                _toInsert.AddRange(children);
            }
        }

        private void InitializeId(List<Quest> allQuests)
        {
            if (allQuests == null || allQuests.Count == 0)
            {
                _currentId = 1;
                return;
            }
            _currentId = allQuests.Max(x => x.Id) + 1;
        }

        private void SetId(Quest quest)
        {
            if (quest.Id == 0)
            {
                quest.Id = _currentId++;
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
