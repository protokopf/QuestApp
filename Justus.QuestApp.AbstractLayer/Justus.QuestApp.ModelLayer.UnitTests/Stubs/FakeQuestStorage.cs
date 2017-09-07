using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.UnitTests.Stubs
{
    public class FakeQuestStorage : IQuestDataLayer
    {
        public List<Quest> QuestStorage { get; set; }
        private bool _isClosed = true;

        public void Dispose()
        {
            Close();
        }

        public void Open()
        {
            if (QuestStorage == null)
            {
                QuestStorage = new List<Quest>();
            }
            _isClosed = false;
        }

        public void Close()
        {
            _isClosed = true;
        }

        public bool IsClosed()
        {
            return _isClosed;
        }

        public void Insert(Quest entity)
        {
            QuestStorage?.Add(entity);
        }

        public void InsertAll(IEnumerable<Quest> entities)
        {
            QuestStorage?.AddRange(entities);
        }

        public virtual void Update(Quest entity)
        {
            if (QuestStorage != null)
            {
                QuestStorage.RemoveAll(x => x.Id == entity.Id);
                QuestStorage.Add(entity);
            }
        }

        public void UpdateAll(IEnumerable<Quest> entities)
        {
            if (QuestStorage != null)
            {
                foreach (Quest quest in entities)
                {
                    QuestStorage.RemoveAll(x => x.Id == quest.Id);
                }
                QuestStorage.AddRange(entities);
            }
        }

        public virtual Quest Get(int id)
        {
            return QuestStorage?.Find(x => x.Id == id);
        }

        public virtual IEnumerable<Quest> GetAll()
        {
            List<Quest> clonedList = new List<Quest>(QuestStorage.Count);
            foreach (Quest quest in QuestStorage)
            {
                clonedList.Add(quest);
            }
            return clonedList;
        }

        public virtual IEnumerable<Quest> GetAll(int parentId)
        {
            List<Quest> clonedList = new List<Quest>(QuestStorage.Count);
            foreach (Quest quest in QuestStorage)
            {
                clonedList.Add(quest);
            }
            return clonedList;
        }

        public void Delete(int id)
        {
            Quest quest = QuestStorage?.Find(x => x.Id == id);
            if (quest != null)
            {
                QuestStorage.Remove(quest);
            }
        }

        public void DeleteAll()
        {
            QuestStorage?.Clear();
        }

        public void DeleteAll(int parentID)
        {
            QuestStorage?.RemoveAll(q => q.ParentId == parentID);
        }

    }
}
