using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.DataLayer.UnitTests.Data.Stubs
{
    internal class FakeQuestStorage : IDataAccessInterface<Quest>
    {
        public List<Quest> QuestStorage { get; set; }
        private bool _isClosed = true;

        public void Dispose()
        {
            Close();
        }

        public void Open(string pathToStorage)
        {
            if(QuestStorage == null)
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
            if(QuestStorage != null)
            {
                QuestStorage.Add(entity);
            }
        }

        public void InsertAll(List<Quest> entities)
        {
            if (QuestStorage != null)
            {
                QuestStorage.AddRange(entities);
            }
        }

        public void Update(Quest entity)
        {
        }

        public void UpdateAll(List<Quest> entities)
        {
            
        }

        public Quest Get(int id)
        {
            if (QuestStorage != null)
            {
                return QuestStorage.Find(x => x.Id == id);
            }
            return null;
        }

        public List<Quest> GetAll()
        {
            if (QuestStorage != null)
            {
                return QuestStorage;
            }
            return null;
        }

        public void Delete(int id)
        {
            if (QuestStorage != null)
            {
                Quest quest = QuestStorage.Find(x => x.Id == id);
                if(quest != null)
                {
                    QuestStorage.Remove(quest);
                }
            }
        }

        public void DeleteAll()
        {
            if (QuestStorage != null)
            {
                QuestStorage.Clear();
            }
        }
    }
}
