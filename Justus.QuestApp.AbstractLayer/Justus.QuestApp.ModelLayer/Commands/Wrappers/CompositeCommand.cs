using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.Wrappers
{
    /// <summary>
    /// ICommand, which executes another commands.
    /// </summary>
    public class CompositeCommand : ICommand
    {
        private readonly ICommand[] _commands;

        /// <summary>
        /// Receives array of inner commands.
        /// </summary>
        /// <param name="commands"></param>
        public CompositeCommand(ICommand[] commands)
        {
            commands.ThrowIfNull(nameof(commands));         
            _commands = commands;
        }

        #region ICommand overriding

        ///<inheritdoc/>
        public bool Execute()
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
        public bool Undo()
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
        public bool IsValid()
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

        ///<inheritdoc/>
        public bool Commit()
        {
            int length = _commands.Length;
            for (int i = 0; i < length; ++i)
            {
                if (!_commands[i].Commit())
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
