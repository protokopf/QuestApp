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
        private int mCount = 0;

        public StubQuestRepositoryService(int count)
        {
            mCount = count;
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
            throw new NotImplementedException();
        }

        public void UpdateAll(List<Quest> quests)
        {
            throw new NotImplementedException();
        }

        public bool RevertUpdate(Quest quest)
        {
            throw new NotImplementedException();
        }

        public Quest Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Quest> GetAll()
        {
            return GetQuests(mCount);
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
                Quest q = new StubQuest
                {
                    Title = "Title " + i,
                    Deadline = DateTime.Now + new TimeSpan(i, 0, 0)
                };
                q.CurrentState = i%2 == 0 ? QuestState.Progress : QuestState.Idle;
                quests.Add(q);
            }
            return quests;
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