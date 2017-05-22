using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.DataLayer.Entities;

namespace Justus.QuestApp.ModelLayer.Factories
{
    /// <summary>
    /// Sqlite quest creator
    /// </summary>
    public class SqliteQuestCreator : IQuestCreator
    {
        #region IQuestCreator implementation

        ///<inheritdoc/>
        public Quest Create()
        {
            return new SqliteQuest()
            {
                Title = String.Empty,
                Description = String.Empty,
                Children = new List<Quest>(),
                CurrentState = QuestState.Idle,
                Deadline = default(DateTime),
                StartTime = default(DateTime),
                IsImportant = false
            };
        }

        #endregion
    }
}
