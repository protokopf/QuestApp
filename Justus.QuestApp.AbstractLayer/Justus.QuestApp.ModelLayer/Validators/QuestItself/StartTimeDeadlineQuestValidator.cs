﻿using System;
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

        private readonly DateTime _defaultDateTime = default(DateTime);

        #region IQuestValidator implementation

        ///<inheritdoc/>
        public ClarifiedResponse<TMessage> Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            
            ClarifiedResponse<TMessage> response  = new ClarifiedResponse<TMessage>();

            DateTime start = quest.StartTime;
            DateTime deadline = quest.Deadline;


            if (deadline < DateTime.Now && deadline != _defaultDateTime)
            {
                response.Errors.Add(new ClarifiedError<TMessage>
                {
                    Error = _deadlineLessThanNow,
                    Clarification = _deadlineLessThanNowClar
                });
            }
            
            if (start > deadline && deadline != _defaultDateTime)
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
