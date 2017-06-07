using System;
using Justus.QuestApp.AbstractLayer.Commands;

namespace Justus.QuestApp.ModelLayer.Commands.Wrappers
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
        public override bool Execute()
        {
            int length = _commands.Length;

            bool result = true;

            for (int i = 0; i < length; ++i)
            {
                if (!_commands[i].Execute())
                {
                    result = false;
                };
            }

            return result;
        }

        ///<inheritdoc/>
        public override bool Undo()
        {
            int length = _commands.Length;

            bool result = true;

            for (int i = length - 1; i >= 0; --i)
            {
                if (!_commands[i].Undo())
                {
                    result = false;
                }
            }

            return result;
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
