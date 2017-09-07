using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property
{
    /// <summary>
    /// Recount progress for given quest.
    /// </summary>
    public class RecountProgressQuestCommand : PropertyChangeQuestCommand<double>
    {
        #region RecountProgressQuestCommand overriding

        ///<inheritdoc />
        protected override double GetPropertyValue(Quest quest)
        {
            return quest.Progress;
        }

        ///<inheritdoc />
        protected override void SetPropertyValue(Quest quest, double propertValue)
        {
            quest.Progress = propertValue;
        }

        ///<inheritdoc />
        protected override void ActionOnQuest(Quest quest, double oldValue)
        {
            if (quest.Children != null && quest.Children.Count != 0)
            {
                quest.Progress = quest.Children.Average(q => q.Progress);
            }
            else
            {
                quest.Progress = quest.State == State.Done ? 1 : 0;
            }
        }

        #endregion
    }
}
