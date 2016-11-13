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
    /// Interface for types, that implement quest progressors logic.
    /// </summary>
    public interface IQuestManager
    {
        /// <summary>
        /// Starts quest.
        /// </summary>
        /// <param name="quest"></param>
        Response Start(Quest quest);

        /// <summary>
        /// Finishes quest.
        /// </summary>
        /// <param name="quest"></param>
        Response Finish(Quest quest);

        /// <summary>
        /// Mark quest as failed.
        /// </summary>
        /// <param name="quest"></param>
        Response Fail(Quest quest);
    }
}
