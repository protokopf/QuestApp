using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// Command, that can be or execute, or reverted. It won't be executed, if it is executed already. 
    /// It won't be reverted, if it is reverted already.
    /// </summary>
    public abstract class SwitchCommand : Command
    {
        /// <summary>
        /// Points if command has been executed.
        /// </summary>
        private bool _hasExecuted;

        #region Command implementation

        ///<inheritdoc cref="Command"/>
        public sealed override bool Execute()
        {
            if (!_hasExecuted)
            {
                _hasExecuted = true;
                return InnerExecute();
            }
            return false;
        }

        ///<inheritdoc cref="Command"/>
        public sealed override bool Undo()
        {
            if (_hasExecuted)
            {
                _hasExecuted = false;
                return InnerUndo();
            }
            return false;
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
