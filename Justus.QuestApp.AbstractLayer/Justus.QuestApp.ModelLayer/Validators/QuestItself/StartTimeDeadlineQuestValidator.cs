using System;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.QuestItself
{
    /// <summary>
    /// Validates startTime and deadline of quests.
    /// </summary>
    public class StartTimeDeadlineQuestValidator<TMessage> : IQuestValidator<ClarifiedResponse<TMessage>>
    {
        private readonly TMessage _startTimeMoreThanDeadline;
        private readonly TMessage _startTimeMoreThanDeadlineClar;

        private readonly TMessage _deadlineLessThanNow;
        private readonly TMessage _deadlineLessThanNowClar;

        /// <summary>
        /// Reveices messages for possible validation fails.
        /// </summary>
        /// <param name="startTimeMoreDeadline"></param>
        /// <param name="startTimeMoreDeadlineClar"></param>
        /// <param name="deadlineLessThanNow"></param>
        /// <param name="deadlineLessThanNowClar"></param>
        public StartTimeDeadlineQuestValidator(TMessage startTimeMoreDeadline, TMessage startTimeMoreDeadlineClar,
                                               TMessage deadlineLessThanNow, TMessage deadlineLessThanNowClar)
        {
            _startTimeMoreThanDeadline = startTimeMoreDeadline;
            _startTimeMoreThanDeadlineClar = startTimeMoreDeadlineClar;
            _deadlineLessThanNow = deadlineLessThanNow;
            _deadlineLessThanNowClar = deadlineLessThanNowClar;
        }

        #region IQuestValidator implementation

        ///<inheritdoc/>
        public ClarifiedResponse<TMessage> Validate(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));

            ClarifiedResponse<TMessage> response  = new ClarifiedResponse<TMessage>();

            DateTime? start = quest.StartTime;
            DateTime? deadline = quest.Deadline;


            if (deadline != null && deadline < DateTime.Now)
            {
                response.Errors.Add(new ClarifiedError<TMessage>
                {
                    Error = _deadlineLessThanNow,
                    Clarification = _deadlineLessThanNowClar
                });
            }
            
            if (deadline != null && start > deadline)
            {
                response.Errors.Add(new ClarifiedError<TMessage>()
                {
                    Error = _startTimeMoreThanDeadline,
                    Clarification = _startTimeMoreThanDeadlineClar
                });
            }
            
            return response;
        }

        #endregion
    }
}
