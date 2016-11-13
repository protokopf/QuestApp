using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;

namespace Justus.QuestApp.ModelLayer.UnitTests.Helpers
{
    /// <summary>
    /// Contains help methods for unit tests.
    /// </summary>
    internal static class QuestHelper
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
    }
}
