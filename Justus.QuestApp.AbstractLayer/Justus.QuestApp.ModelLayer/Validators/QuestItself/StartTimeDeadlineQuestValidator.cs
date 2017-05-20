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
    public class StartTimeDeadlineQuestValidator : IQuestValidator<ClarifiedResponse<string>>
    {
        private readonly DateTime _defaultDateTime = default(DateTime);

        #region IQuestValidator implementation

        ///<inheritdoc/>
        public ClarifiedResponse<string> Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            
            ClarifiedResponse<string> response  = new ClarifiedResponse<string>();

            DateTime start = quest.StartTime;
            DateTime deadline = quest.Deadline;

            if (start == _defaultDateTime && deadline == _defaultDateTime)
            {
                //Ok situation.
            }
            else if (start == _defaultDateTime)
            {
                if (deadline > DateTime.Now)
                {
                    response.Errors.Add(new ClarifiedError<string>
                    {
                        Error = "0",
                        Clarification = "0_CL"
                    });
                }
            }
            else if (deadline == _defaultDateTime)
            {
                //Ok situation.
            }
            else
            {
                if (start > deadline)
                {
                    response.Errors.Add(new ClarifiedError<string>()
                    {
                        Error = "1",
                        Clarification = "1_CL"
                    });
                }
            }
            return response;
        }

        #endregion
    }
}
