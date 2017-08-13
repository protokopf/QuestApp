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
                bool result = InnerExecute();
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
                bool result = InnerUndo();
                _hasExecuted = false;
                return result;
            }
            return false;
        }

        ///<inheritdoc cref="ICommand"/>
        public virtual bool IsValid()
        {
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Commit()
        {
            bool result = false;
            if (!_hasCommited)
            {
                result = InnerCommit();
                _hasCommited = true;
            }

            return result;
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

        /// <summary>
        /// Will be called only if command has not been commited.
        /// </summary>
        /// <returns></returns>
        protected abstract bool InnerCommit();
    }
}
