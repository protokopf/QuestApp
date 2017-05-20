using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Entities.Errors
{
    /// <summary>
    /// Represents class for errors, that have sub error, that specify it.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public struct ClarifiedError<TMessage>
    {
        /// <summary>
        /// Main error.
        /// </summary>
        public TMessage Error { get; set; }

        /// <summary>
        /// Sub error, that clarify main error.
        /// </summary>
        public TMessage Clarification { get; set; }
    }
}
