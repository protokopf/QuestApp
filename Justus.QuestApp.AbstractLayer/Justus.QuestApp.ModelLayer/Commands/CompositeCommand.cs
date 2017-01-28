using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;

namespace Justus.QuestApp.ModelLayer.Commands
{
    /// <summary>
    /// Command, which executes another commands.
    /// </summary>
    public class CompositeCommand : Command
    {
        private readonly Command[] _commands;

        /// <summary>
        /// Receives array of inner commands.
        /// </summary>
        /// <param name="commands"></param>
        public CompositeCommand(Command[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            _commands = commands;
        }

        #region Command overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            int length = _commands.Length;
            for (int i = 0; i < length; ++i)
            {
                _commands[i].Execute();
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            int length = _commands.Length;
            for (int i = length - 1; i >= 0; --i)
            {
                _commands[i].Undo();
            }
        }

        ///<inheritdoc/>
        public override bool IsValid()
        {
            int length = _commands.Length;
            for (int i = 0; i < length; ++i)
            {
                if (!_commands[i].IsValid())
                {
                    return false;
                }
            }
            return true;
        } 

        #endregion
    }
}
