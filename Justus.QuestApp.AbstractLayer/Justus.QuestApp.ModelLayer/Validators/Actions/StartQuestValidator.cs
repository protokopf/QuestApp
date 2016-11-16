using System;
using System.Runtime.InteropServices;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.Actions
{
    /// <summary>
    /// Validates, whether quest is ready for start or not.
    /// </summary>
    public class StartQuestValidator : IQuestValidator
    {
        #region IQuestValidator implementation

        ///<inheritdoc/>
        public Response Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

            Response result = new Response();
            if (quest.CurrentState != QuestState.Idle)
            {
                result.Errors.Add("ERR_QUEST_ACT_WRONG_STATE");
                return result;
            }
            if (quest.Children.Count != 0)
            {
                result.Errors.Add("ERR_QUEST_ACT_HAS_CHILDREN");
                return result;
            }
            return result;
        }

        #endregion
    }
}
