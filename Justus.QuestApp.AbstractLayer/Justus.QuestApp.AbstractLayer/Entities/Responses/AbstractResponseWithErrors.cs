using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Model.Composite;

namespace Justus.QuestApp.AbstractLayer.Entities.Responses
{
    /// <summary>
    /// Abstract type for responses with errors.
    /// </summary>
    /// <typeparam name="TError"></typeparam>
    public abstract class AbstractResponseWithErrors<TError> : IResponseWithErrors<TError>, 
        IComposable<IResponseWithErrors<TError>>
    {
        /// <summary>
        /// Default ctor. Initializes Erros list.
        /// </summary>
        protected AbstractResponseWithErrors()
        {
            Errors = new List<TError>();
        }

        #region IResponseWithErrors implementation

        ///<inheritdoc/>
        public IList<TError> Errors { get; }

        ///<inheritdoc/>
        public bool IsSuccessful => Errors.Count == 0;

        #endregion

        #region IComposable implementaion

        ///<inheritdoc/>
        public void Compose(IResponseWithErrors<TError> other)
        {
            foreach (TError error in other.Errors)
            {
                Errors.Add(error);
            }
        }

        #endregion
    }
}
