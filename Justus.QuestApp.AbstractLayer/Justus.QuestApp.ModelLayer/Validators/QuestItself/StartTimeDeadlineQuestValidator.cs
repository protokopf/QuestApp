using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.QuestItself
{
    /// <summary>
    /// Validates startTime and deadline of quests.
    /// </summary>
    public class StartTimeDeadlineQuestValidator : IQuestValidator<ClarifiedResponse<QuestValidationErrorCode>>
    {
        private readonly DateTime _defaultDateTime = default(DateTime);

        #region IQuestValidator implementation

        ///<inheritdoc/>
        public ClarifiedResponse<QuestValidationErrorCode> Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            
            ClarifiedResponse<QuestValidationErrorCode> response  = new ClarifiedResponse<QuestValidationErrorCode>();

            DateTime start = quest.StartTime;
            DateTime deadline = quest.Deadline;


            if (deadline < DateTime.Now && deadline != _defaultDateTime)
            {
                response.Errors.Add(new ClarifiedError<QuestValidationErrorCode>
                {
                    Error = QuestValidationErrorCode.DeadlineLessThanNow,
                    Clarification = QuestValidationErrorCode.DeadlineLessThanNowClar
                });
            }
            
            if (start > deadline && deadline != _defaultDateTime)
            {
                response.Errors.Add(new ClarifiedError<QuestValidationErrorCode>()
                {
                    Error = QuestValidationErrorCode.StartTimeMoreThanDeadline,
                    Clarification = QuestValidationErrorCode.StartTimeMoreThanDeadlineClar
                });
            }
            
            return response;
        }

        #endregion
    }
}
