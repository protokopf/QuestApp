using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.DataLayer.Entities;

namespace Justus.QuestApp.ModelLayer.Factories
{
    /// <summary>
    /// Sqlite quest factory
    /// </summary>
    public class SqliteQuestFactory : IQuestFactory
    {
        #region IQuestFactory implementation

        ///<inheritdoc/>
        public Quest CreateQuest()
        {
            return new SqliteQuest()
            {
                Title = String.Empty,
                Description = String.Empty,
                Children = null,
                State = State.Idle,
                Deadline = null,
                StartTime = null,
                IsImportant = false,
                ParentId = null,
                Parent = null
            };
        }

        #endregion
    }
}
