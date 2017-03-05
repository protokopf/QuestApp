﻿using Justus.QuestApp.AbstractLayer.Helpers;
using System;
using System.Threading.Tasks;

namespace Justus.QuestApp.ModelLayer.Helpers
{
    /// <summary>
    /// Wraps action in task.
    /// </summary>
    public class AutoStartTaskWrapper : ITaskWrapper
    {
        #region ITaskWrapper implementation

        ///<inheritdoc/>
        public Task Wrap(Action actionToWrap)
        {
            if (actionToWrap == null)
            {
                throw new ArgumentNullException(nameof(actionToWrap));
            }
            return Task.Run(actionToWrap);
        }

        ///<inheritdoc/>
        public Task<TResult> Wrap<TResult>(Func<TResult> functionWithResult)
        {
            if (functionWithResult == null)
            {
                throw new ArgumentNullException(nameof(functionWithResult));
            }
            return Task.Run<TResult>(functionWithResult);
        } 

        #endregion
    }
}
