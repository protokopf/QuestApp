﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Abstract type for commands on quests.
    /// </summary>
    public abstract class AbstractQuestCommand : SwitchCommand
    {
        protected readonly Quest QuestRef;

        /// <summary>
        /// Receives quest to operate on.
        /// </summary>
        /// <param name="quest"></param>
        protected AbstractQuestCommand(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            QuestRef = quest;
        }

        #region Protected abstract methods

        /// <summary>
        /// Called for quest during execution.
        /// </summary>
        /// <param name="quest"></param>
        protected abstract void ExecuteOnQuest(Quest quest);

        /// <summary>
        /// Called for quest during rollback.
        /// </summary>
        /// <param name="quest"></param>
        protected abstract void UndoOnQuest(Quest quest);

        #endregion
    }
}
