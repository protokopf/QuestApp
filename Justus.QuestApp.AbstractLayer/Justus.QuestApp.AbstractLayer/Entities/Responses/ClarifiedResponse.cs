using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Errors;

namespace Justus.QuestApp.AbstractLayer.Entities.Responses
{
    /// <summary>
    /// Represents response with collection of clarified errors.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class ClarifiedResponse<TMessage> : AbstractResponseWithErrors<ClarifiedError<TMessage>>
    {
    }
}
