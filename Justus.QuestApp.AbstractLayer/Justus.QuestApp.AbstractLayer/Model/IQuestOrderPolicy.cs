using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for quest ordering policies.
    /// </summary>
    public interface IQuestOrderStrategy
    {
        /// <summary>
        /// Orders quests.
        /// </summary>
        /// <param name="quests"></param>
        /// <returns></returns>
        IOrderedEnumerable<Quest> Order(IEnumerable<Quest> quests);

        /// <summary>
        /// Gets or sets value, points whether quests should be ordered by ascending. If false - order by descending.
        /// False by default.
        /// </summary>
        bool Descending { set; get; }
    }
}
