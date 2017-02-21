using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.View.Droid.Fragments;

namespace Justus.QuestApp.View.Droid.StubServices
{
    class StubQuestRepositoryService : IQuestRepository
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
            throw new NotImplementedException();
        }

        public void Insert(Quest quest)
        {
            throw new NotImplementedException();
        }

        public void InsertAll(List<Quest> quests)
        {
            throw new NotImplementedException();
        }

        public bool RevertInsert(Quest quest)
        {
            throw new NotImplementedException();
        }

        public void Update(Quest quest)
        {
            
        }

        public void UpdateAll(List<Quest> quests)
        {
            throw new NotImplementedException();
        }

        public bool RevertUpdate(Quest quest)
        {
            return true;
        }

        public Quest Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Quest> GetAll()
        {
            return _quests ?? (_quests = GetQuests(_topCount));
        }

        public void Delete(Quest quest)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public bool RevertDelete(Quest quest)
        {
            throw new NotImplementedException();
        }

        public void PushQuests()
        {
            Thread.Sleep(2000);
        }

        public void PullQuests()
        {
            Thread.Sleep(2000);
        }

        private List<Quest> GetQuests(int count)
        {
            List<Quest> quests = new List<Quest>();
            for (int i = 0; i < count; ++i)
            {
                quests.Add(CreateCompositeQuestFromAbove(_depth, _child));
            }
            return quests;
        }

        private Quest CreateQuest(int id = 0)
        {
            QuestState state = QuestState.Idle;

            return new StubQuest()
            {
                Id = id,
                Title = "Quest � " + id,
                Description = "Description " + id,
                CurrentState = state,
                Children = new List<Quest>(),
                Deadline = GetDeadLine(id),
                StartTime = GetStartDate(id)
            };
        }

        private Quest CreateCompositeQuestFromAbove(int compositionLevel, int childNumber)
        {
            Quest quest = CreateQuest(++_currentId);
            if (compositionLevel == 0 || childNumber == 0)
            {
                return quest;
            }
            for (int i = 0; i < childNumber; ++i)
            {
                Quest child = CreateCompositeQuestFromAbove(compositionLevel - 1, childNumber);
                child.Parent = quest;
                quest.Children.Add(child);
            }
            return quest;
        }

        private DateTime GetDeadLine(int id)
        {
            DateTime result = DateTime.MaxValue;
            //if (id%2 == 0)
            //{
                result = DateTime.Now + new TimeSpan(0, 1, 60, 0);
            //}
            //else if (id%3 == 0)
            //{
            //    result = DateTime.Now -new TimeSpan(0, 12, 0, 0);
            //}
            return result;
        }

        private DateTime GetStartDate(int id)
        {
            DateTime result = DateTime.MaxValue;
            //if (id % 2 == 0)
            //{
                result = DateTime.Now - new TimeSpan(0, 0, 5, 0);
            //}
            //else if (id % 3 == 0)
            //{
            //    result = DateTime.Now - new TimeSpan(0, 12, 0, 0);
            //}
            return result;
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