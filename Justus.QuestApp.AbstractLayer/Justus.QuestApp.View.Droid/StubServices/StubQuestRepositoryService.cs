using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.View.Droid.StubServices
{
    class StubQuestRepositoryService : IDataAccessInterface<Quest>
    {
        private int _depth = 0;
        private int _child = 0;
        private int _currentId = 0;
        private int _topCount = 0;
        private List<Quest> _quests = null;
        private Random _random = new Random();

        public StubQuestRepositoryService(int topCount, int depth, int child)
        {
            _topCount = topCount;
            _depth = depth;
            _child = child;
        }

        public void Dispose()
        {
            
        }

        public void Insert(Quest quest)
        {
            
        }

        public void InsertAll(List<Quest> quests)
        {
            
        }

        public void Update(Quest quest)
        {
            
        }

        public void UpdateAll(List<Quest> quests)
        {
            
        }

        public Quest Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Quest> GetAll()
        {
            return GetQuests(_topCount);
        }


        public void DeleteAll()
        {
            
        }


        private List<Quest> GetQuests(int count)
        {
            List<Quest> quests = new List<Quest>();
            for (int i = 0; i < count; ++i)
            {
                CreateCompositeQuestsWithIdRelations(0,_depth, _child, ref quests);
            }
            return quests;
        }

        private Quest CreateQuest(int id = 0)
        {
            QuestState state = QuestState.Idle;

            return new StubQuest()
            {
                Id = id,
                Title = "Quest ¹ " + id,
                Description = /*GenerateDescription()*/ $"Desription {id}",
                CurrentState = state,
                Children = new List<Quest>(),
                Deadline = GetDeadLine(id),
                StartTime = GetStartDate(id)
            };
        }

        private void CreateCompositeQuestsWithIdRelations(int parentId, int compositionLevel, int childNumber,
            ref List<Quest> totalQuests)
        {
            Quest quest = CreateQuest(++_currentId);
            quest.ParentId = parentId;

            totalQuests.Add(quest);

            if (compositionLevel == 0 || childNumber == 0)
            {
                return;
            }
            for (int i = 0; i < childNumber; ++i)
            {
                CreateCompositeQuestsWithIdRelations(quest.Id, compositionLevel - 1, childNumber, ref totalQuests);
            }
            return;
        }

        private DateTime GetDeadLine(int id)
        {
            DateTime result = DateTime.MaxValue;
            result = DateTime.Now + new TimeSpan(0, id, 60, 0);
            return result;
        }

        private DateTime GetStartDate(int id)
        {
            DateTime result = DateTime.MaxValue;
            result = DateTime.Now - new TimeSpan(0, id, 5, 0);
            return result;
        }

        private string GenerateDescription()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 15; ++i)
            {
                builder.AppendLine(
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
            }
            return builder.ToString();
        }

        public void Open(string pathToStorage)
        {
            
        }

        public void Close()
        {
            
        }

        public bool IsClosed()
        {
            return false;
        }

        public void Delete(int id)
        {
            _quests?.RemoveAll(q => q.Id == id);
        }
    }

    class StubQuest : Quest
    {
        public override int Id { get; set; }
        public override int ParentId { get; set; }
        public override string Title { get; set; }
        public override string Description { get; set; }
        public override Quest Parent { get; set; }
        public override List<Quest> Children { get; set; }
        public override DateTime Deadline { get; set; }
    }
}