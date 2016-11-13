using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Model.QuestManagement.Validators
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
            Response result = new Response();
            switch (quest.CurrentState)
            {
                case QuestState.Progress:
                    result.Errors.Add("You cannot start quest in progress!");
                    return result;
                case QuestState.Done:
                    result.Errors.Add("You cannot start the quest already done!");
                    return result;
                case QuestState.Failed:
                    result.Errors.Add("You cannot start the failed quest!");
                    return result;
            }
            if (quest.Children.Count != 0)
            {
                result.Errors.Add("You can start only the most nested quest!");
                return result;
            }
            return result;
        }

        #endregion
    }
}
