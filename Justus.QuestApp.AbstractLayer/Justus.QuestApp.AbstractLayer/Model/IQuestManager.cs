using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        void Start(Quest quest);

        /// <summary>
        /// Finishes quest.
        /// </summary>
        /// <param name="quest"></param>
        void Finish(Quest quest);

        /// <summary>
        /// Mark quest as failed.
        /// </summary>
        /// <param name="quest"></param>
        void Fail(Quest quest);
    }
}
