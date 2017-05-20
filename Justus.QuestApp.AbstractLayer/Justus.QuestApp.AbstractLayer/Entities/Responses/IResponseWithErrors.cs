using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Entities.Responses
{
    /// <summary>
    /// Interface for responses with errors.
    /// </summary>
    /// <typeparam name="TError"></typeparam>
    public interface IResponseWithErrors<TError> : IResponse
    {
        /// <summary>
        /// Errors collection.
        /// </summary>
        IList<TError> Errors { get; }
    }
}
