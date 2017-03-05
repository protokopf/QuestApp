using Justus.QuestApp.AbstractLayer.Entities.Quest;
using System;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for types, which count reminded time for quest.
    /// </summary>
    public interface IQuestTimeLeftCounter
    {
        /// <summary>
        /// Count time left for quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        TimeSpan CountTimeLeft(Quest quest);
    }
}
