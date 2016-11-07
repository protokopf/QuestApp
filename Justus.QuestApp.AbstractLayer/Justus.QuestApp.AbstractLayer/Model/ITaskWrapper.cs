using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for task wrappers.
    /// </summary>
    public interface ITaskWrapper
    {
        /// <summary>
        /// Wraps action, that returns nothing
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Wrap(Action action);

        /// <summary>
        /// Wraps function, that returns TResult.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="functionWithResult"></param>
        /// <returns></returns>
        Task<TResult> Wrap<TResult>(Func<TResult> functionWithResult);
    }
}
