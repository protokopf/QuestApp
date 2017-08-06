using System;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.QuestItself
{
    /// <summary>
    /// Validates quests description.
    /// </summary>
    public class DescriptionQuestValidator<TMessage> : IQuestValidator<ClarifiedResponse<TMessage>>
    {
        private readonly TMessage _nullOrWhitespace;
        private readonly TMessage _nullOrWhitespaceClar;

        public DescriptionQuestValidator(TMessage nullOrWhitespace, TMessage nullOrWhitespaceClar)
        {
            _nullOrWhitespace = nullOrWhitespace;
            _nullOrWhitespaceClar = nullOrWhitespaceClar;
        }

        #region IQuestValidator implementation

        ///<inheritdoc/>
        public ClarifiedResponse<TMessage> Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            ClarifiedResponse<TMessage> response = new ClarifiedResponse<TMessage>();

            if (string.IsNullOrWhiteSpace(quest.Description))
            {
                response.Errors.Add(new ClarifiedError<TMessage>()
                {
                    Error = _nullOrWhitespace,
                    Clarification = _nullOrWhitespaceClar
                });
            }

            return response;
        }

        #endregion
    }
}
