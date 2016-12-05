using Justus.QuestApp.AbstractLayer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ModelLayer.Commands.Management
{
    /// <summary>
    /// Manages commands.
    /// </summary>
    public class CommandCyclingManager : ICommandManager
    {
        private readonly int _queueSize;

        private Command[] _commands;
        private int _index;
        private int _execIndex;

        /// <summary>
        /// Receives queue size and initialize inner command storage.
        /// </summary>
        /// <param name="queueSize"></param>
        public CommandCyclingManager(int queueSize)
        {
            if(queueSize < 1)
            {
                throw new IndexOutOfRangeException("ERR_COMMAND_MGR_INDX");
            }
            _queueSize = queueSize;
            _commands = new Command[_queueSize];
            _index = 0;
            _execIndex = -1;
        }

        #region ICommandManager implementation

        ///<inheritdoc/>
        public void Add(Command commandToAdd)
        {
            if(commandToAdd == null)
            {
                throw new ArgumentNullException(nameof(commandToAdd));
            }

            if (_index == _queueSize)
            {
                _index = 0;
            }

            _commands[_index++] = commandToAdd;
        }

        ///<inheritdoc/>
        public void Do()
        {
            if (_execIndex >= -1 && _execIndex < _index && _execIndex < _commands.Length - 1)
            {
                if (_commands[_execIndex + 1].IsValid())
                {
                    _commands[++_execIndex].Execute();
                }
            }
        }

        ///<inheritdoc/>
        public void Undo()
        {
            if (_execIndex > -1)
            {
                _commands[_execIndex--].Undo();
            }
        }

        #endregion
    }
}
