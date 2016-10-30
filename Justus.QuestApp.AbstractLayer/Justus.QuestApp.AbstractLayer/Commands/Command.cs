using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Commands
{
    /// <summary>
    /// Interface to all commands.
    /// </summary>
    public interface Command
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        void Execute();

        /// <summary>
        /// Reverts command influence.
        /// </summary>
        void Undo();

        /// <summary>
        /// Check, whether command is valid or not.
        /// </summary>
        /// <returns></returns>
        bool IsValid();
    }
}
