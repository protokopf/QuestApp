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
    }
}
