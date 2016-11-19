using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for types, that count completeness of quests.
    /// </summary>
    public interface IQuestProgressCounter
    {
        /// <summary>
        /// Count progress of given quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        ProgressValue CountProgress(Quest quest);
    }
}
