using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Model.Composite;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.QuestItself
{
    public class CompositeQuestValidator<TResponse> : IQuestValidator<TResponse>
        where TResponse : IResponse, IComposable<TResponse>,  new()
    {
        private readonly IEnumerable<IQuestValidator<TResponse>> _innerValidators;

        /// <summary>
        /// Receives inner validators.
        /// </summary>
        /// <param name="questValidators"></param>
        public CompositeQuestValidator(IEnumerable<IQuestValidator<TResponse>> questValidators)
        {
            if (questValidators == null)
            {
                throw new ArgumentNullException(nameof(questValidators));
            }
            _innerValidators = questValidators;
        }

        #region IQuestValidator implementation

        ///<inheritdoc/>
        public TResponse Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            TResponse response = new TResponse();
            foreach (IQuestValidator<TResponse> validator in _innerValidators)
            {
                response.Compose(validator.Validate(quest));
            }
            return response;
        }

        #endregion
    }
}
