using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for types, that count completeness of quests.
    /// </summary>
    interface IQuestProgressCounter
    {
        /// <summary>
        /// Counts expected finish units. E.g. if quest is done on 50%, i.e., for example, on 5/10, expected unit is 10.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        int CountExpectedFinishUnits(Quest quest);

        /// <summary>
        /// Counts actual finish units. E.g. if quest is done on 50%, i.e., for example, on 5/10, actual unit is 5.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        int CountActualFinishUnits(Quest quest);
    }
}
