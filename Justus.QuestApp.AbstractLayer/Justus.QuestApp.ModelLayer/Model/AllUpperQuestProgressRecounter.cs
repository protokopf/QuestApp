﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Model
{
    /// <summary>
    /// Recounts progress for quest and its parent hierarchy.
    /// </summary>
    public class AllUpperQuestProgressRecounter : IQuestProgressRecounter
    {
        #region IQuestProgressRecounter implementation

        ///<inheritdoc cref="IQuestProgressRecounter"/>
        public void RecountProgress(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));

            Quest current = quest;
            while (current != null)
            {
                if (current.Children?.Count != 0)
                {
                    current.Progress = current.Children.Average(q => q.Progress);
                }
                else
                {
                    current.Progress = current.CurrentState == QuestState.Done ? 1 : 0;
                }
                current = current.Parent;
            }
        } 

        #endregion
    }
}
