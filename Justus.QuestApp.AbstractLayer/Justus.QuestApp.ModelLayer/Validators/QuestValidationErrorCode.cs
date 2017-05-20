using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ModelLayer.Validators
{
    /// <summary>
    /// Validation errors for quests.
    /// </summary>
    public enum QuestValidationErrorCode
    {
        TitleIsNullOrEmpty,
        TitleIsNullOrEmptyClar,

        StartTimeMoreThanDeadline,
        StartTimeMoreThanDeadlineClar,

        DeadlineLessThanNow,
        DeadlineLessThanNowClar,
    }
}
