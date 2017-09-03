using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Commands
{
    /// <summary>
    /// Interface to command that operates on quest.
    /// </summary>
    public interface IQuestCommand
    {
        /// <summary>
        /// Executes command on quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        bool Execute(Quest quest);

        /// <summary>
        /// Rollback changes made on quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        bool Undo(Quest quest);

        /// <summary>
        /// Commits changes.
        /// </summary>
        /// <returns></returns>
        bool Commit();
    }
}
