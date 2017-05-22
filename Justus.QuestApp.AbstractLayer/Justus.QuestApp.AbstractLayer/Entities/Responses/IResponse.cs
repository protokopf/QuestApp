using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Entities.Responses
{
    /// <summary>
    /// Interface for responces.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Points, whether response is successfull or not.
        /// </summary>
        bool IsSuccessful { get; }
    }
}
