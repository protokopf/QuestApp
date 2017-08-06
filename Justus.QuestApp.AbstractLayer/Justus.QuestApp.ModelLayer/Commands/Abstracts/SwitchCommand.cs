using Justus.QuestApp.AbstractLayer.Commands;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// ICommand, that can be or execute, or reverted. It won't be executed, if it is executed already. 
    /// It won't be reverted, if it is reverted already.
    /// </summary>
    public abstract class SwitchCommand : ICommand
    {
        /// <summary>
        /// Points if command has been executed.
        /// </summary>
        private bool _hasExecuted;

        private bool _hasCommited;

        #region ICommand implementation

        ///<inheritdoc cref="ICommand"/>
        public bool Execute()
        {
            if (!_hasExecuted && !_hasCommited)
            {
                _hasExecuted = true;
                return InnerExecute();
            }
            return false;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Undo()
        {
            if (_hasExecuted && !_hasCommited)
            {
                _hasExecuted = false;
                return InnerUndo();
            }
            return false;
        }

        ///<inheritdoc cref="ICommand"/>
        public virtual bool IsValid()
        {
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public virtual bool Commit()
        {
            _hasCommited = true;
            return true;
        }

        #endregion

        /// <summary>
        /// Will be called only if command has not been already executed.
        /// </summary>
        /// <returns></returns>
        protected abstract bool InnerExecute();

        /// <summary>
        /// Will be called only if command has not been already reverted.
        /// </summary>
        /// <returns></returns>
        protected abstract bool InnerUndo();
    }
}
