using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.UnitTests.Stubs
{
    public class FakeQuestStorage : IDataAccessInterface<Quest>
    {
        public List<Quest> QuestStorage { get; set; }
        private bool _isClosed = true;

        public virtual void Dispose()
        {
            Close();
        }

        public virtual void Open(string pathToStorage)
        {
            if (QuestStorage == null)
            {
                QuestStorage = new List<Quest>();
            }
            _isClosed = false;
        }

        public virtual void Close()
        {
            _isClosed = true;
        }

        public virtual bool IsClosed()
        {
            return _isClosed;
        }

        public virtual void Insert(Quest entity)
        {
            QuestStorage?.Add(entity);
        }

        public virtual void InsertAll(List<Quest> entities)
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

        public virtual void UpdateAll(List<Quest> entities)
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

        public virtual List<Quest> GetAll()
        {
            List<Quest> clonedList = new List<Quest>(QuestStorage.Count);
            foreach (Quest quest in QuestStorage)
            {
                clonedList.Add(quest);
            }
            return clonedList;
        }

        public virtual void Delete(int id)
        {
            Quest quest = QuestStorage?.Find(x => x.Id == id);
            if (quest != null)
            {
                QuestStorage.Remove(quest);
            }
        }

        public virtual void DeleteAll()
        {
            QuestStorage?.Clear();
        }
    }
}
