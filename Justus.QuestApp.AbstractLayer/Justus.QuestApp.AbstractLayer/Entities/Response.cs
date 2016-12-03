using System.Collections.Generic;

namespace Justus.QuestApp.AbstractLayer.Entities
{
    /// <summary>
    /// Represents result of some action.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Default constructor. Initialize Errors list.
        /// </summary>
        public Response()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Determines, whether action is successful or not.
        /// </summary>
        public bool IsSuccessful
        {
            get { return Errors.Count == 0; }
        } 

        /// <summary>
        /// List of errors.
        /// </summary>
        public List<string> Errors { get; }
    }
}
