using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;

namespace Justus.QuestApp.ModelLayer.UnitTests.Helpers
{
    /// <summary>
    /// Contains help methods for unit tests.
    /// </summary>
    public static class QuestHelper
    {
        public static Quest CreateQuest(QuestState state)
        {
            return new FakeQuest()
            {
                Children = new List<Quest>(),
                CurrentState = state,
                Title = "Title",
                Description = "Description",
                Id = 0,
                ParentId = 0,
                Parent = null
            };
        }

        public static Quest CreateCompositeQuest(int compositionLevel, int childNumber, QuestState state)
        {
            Quest quest = CreateQuest(state);
            if (compositionLevel == 0 || childNumber == 0)
            {
                return quest;
            }
            for (int i = 0; i < childNumber; ++i)
            {
                Quest child = CreateCompositeQuest(compositionLevel - 1, childNumber, state);
                child.Parent = quest;
                quest.Children.Add(child);
            }
            return quest;
        }

        public static bool CheckThatAllQuestsHierarchyMatchPredicate(List<Quest> quests, Func<Quest,bool> predicate)
        {
            foreach (Quest quest in quests)
            {
                if (!predicate( quest) || !CheckThatAllQuestsHierarchyMatchPredicate(quest.Children, predicate))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckThatAnyQuestFromHierarchyMatchPredicate(List<Quest> quests, Func<Quest, bool> predicate)
        {
            foreach (Quest quest in quests)
            {
                if(predicate(quest))
                {
                    return true;
                }
                if (CheckThatAnyQuestFromHierarchyMatchPredicate(quest.Children, predicate))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CountSubQuests(List<Quest> childs)
        {
            if (childs == null || childs.Count == 0)
            {
                return 0;
            }
            int totalCount = childs.Count;
            foreach (Quest child in childs)
            {
                totalCount += CountSubQuests(child.Children);
            }
            return totalCount;
        }

        public static Quest CreateQuest(int id = 0)
        {
            return new FakeQuest()
            {
                Id = id,
                Title = "title",
                Description = "description",
                CurrentState = QuestState.Idle,
                Children = new List<Quest>()
            };
        }

        public static Quest CreateCompositeQuestFromAbove(int compositionLevel, int childNumber)
        {
            Quest quest = CreateQuest();
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

        public static List<Quest> CreateCompositeQuestFromBelow(int compositionLevel, int childNumber, int startId = 1)
        {
            List<Quest> result = new List<Quest>();

            while (compositionLevel != 0)
            {
                Quest quest = CreateQuest(startId);
                result.Add(quest);

                for (int i = 0; i < childNumber; ++i)
                {
                    Quest child = CreateQuest(++startId);
                    result.Add(child);
                    child.ParentId = quest.Id;
                }
                compositionLevel--;
                startId++;
            }
            return result;
        }

        public static int CountExpectedSubQuests(int depth, int child)
        {
            int total = 0;
            if (depth == 0 || child == 0)
            {
                return total;
            }
            do
            {
                total += child;
                child = (int)Math.Pow(child, 2);
                --depth;
            } while (depth != 0);

            return total;
        }

        public static List<Quest> CreateQuests(int count)
        {
            List<Quest> quests = new List<Quest>();
            for (int i = 0; i < count; ++i)
            {
                quests.Add(CreateQuest(i + 1));
            }
            return quests;
        }
    }
}
