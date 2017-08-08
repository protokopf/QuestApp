using System;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.QuestItself
{
    /// <summary>
    /// Validates quest title.
    /// </summary>
    public class TitleQuestValidator<TMessage> : IQuestValidator<ClarifiedResponse<TMessage>>
    {
        private static readonly int MaxTitleLength = 25;

        private readonly TMessage _nullOrWhitespace;
        private readonly TMessage _nullOrWhitespaceClar;

        private readonly TMessage _tooLong;
        private readonly TMessage _tooLongClar;

        /// <summary>
        /// Receives error messages for given situations.
        /// </summary>
        /// <param name="nullOrWhitespace"></param>
        /// <param name="nullOrWhitespaceClar"></param>
        /// <param name="tooLong"></param>
        /// <param name="tooLongClar"></param>
        public TitleQuestValidator(TMessage nullOrWhitespace, TMessage nullOrWhitespaceClar, TMessage tooLong, TMessage tooLongClar)
        {
            _nullOrWhitespace = nullOrWhitespace;
            _nullOrWhitespaceClar = nullOrWhitespaceClar;

            _tooLong = tooLong;
            _tooLongClar = tooLongClar;
        }

        #region IQuestValidator implementation

        ///<inheritdoc/>
        public ClarifiedResponse<TMessage> Validate(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            ClarifiedResponse<TMessage> response = new ClarifiedResponse<TMessage>();

            if (string.IsNullOrWhiteSpace(quest.Title))
            {
                response.Errors.Add(new ClarifiedError<TMessage>()
                {
                    Error = _nullOrWhitespace,
                    Clarification = _nullOrWhitespaceClar
                });
            }
            else if (quest.Title.Length > MaxTitleLength)
            {
                response.Errors.Add(new ClarifiedError<TMessage>()
                {
                    Error = _tooLong,
                    Clarification = _tooLongClar
                });
            }
            return response;
        }

        #endregion
    }
}
