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
    public interface IQuestActionManager
    {
        /// <summary>
        /// Mark quest as Progress.
        /// </summary>
        /// <param name="quest"></param>
        void Start(Quest quest);

        /// <summary>
        /// Mark quest as Done.
        /// </summary>
        /// <param name="quest"></param>
        void Finish(Quest quest);

        /// <summary>
        /// Mark quest as Failed.
        /// </summary>
        /// <param name="quest"></param>
        void Fail(Quest quest);

        /// <summary>
        /// Mark quest as Idle.
        /// </summary>
        /// <param name="quest"></param>
        void Idle(Quest quest);
    }
}
