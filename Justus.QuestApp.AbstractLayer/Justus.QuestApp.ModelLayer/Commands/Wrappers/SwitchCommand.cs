using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.Wrappers
{
    /// <summary>
    /// ICommand, that can be or execute, or reverted. It won't be executed, if it is executed already. 
    /// It won't be reverted, if it is reverted already.
    /// </summary>
    public class SwitchCommand : ICommand
    {
        private readonly ICommand _innerCommand;

        /// <summary>
        /// Receives inner command to execute.
        /// </summary>
        /// <param name="innerCommand"></param>
        public SwitchCommand(ICommand innerCommand)
        {
            innerCommand.ThrowIfNull(nameof(innerCommand));
            _innerCommand = innerCommand;
        }

        private bool _hasExecuted;
        private bool _hasCommited;

        #region ICommand implementation

        ///<inheritdoc cref="ICommand"/>
        public bool Execute()
        {
            if (!_hasExecuted && !_hasCommited)
            {
                bool result = _innerCommand.Execute();
                _hasExecuted = true;
                return result;
            }
            return false;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Undo()
        {
            if (_hasExecuted && !_hasCommited)
            {
                bool result = _innerCommand.Undo();
                _hasExecuted = false;
                return result;
            }
            return false;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool IsValid()
        {
            return _innerCommand.IsValid();
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Commit()
        {
            bool result = false;
            if (!_hasCommited)
            {
                result = _innerCommand.Commit();
                _hasCommited = true;
            }

            return result;
        }

        #endregion
    }
}
