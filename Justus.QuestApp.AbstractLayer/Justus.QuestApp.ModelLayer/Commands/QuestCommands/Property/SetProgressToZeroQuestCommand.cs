using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property
{
    /// <summary>
    /// Sets progress of quest to zero.
    /// </summary>
    public class SetProgressToZeroQuestCommand : PropertyChangeQuestCommand<double>
    {
        #region PropertyChangeQuestCommand overriding

        ///<inheritdoc/>
        protected override double GetPropertyValue(Quest quest)
        {
            return quest.Progress;
        }

        ///<inheritdoc/>
        protected override void SetPropertyValue(Quest quest, double propertValue)
        {
            quest.Progress = propertValue;
        }

        ///<inheritdoc/>
        protected override void ActionOnQuest(Quest quest, double oldValue)
        {
            quest.Progress = 0;
        }

        #endregion
    }
}
