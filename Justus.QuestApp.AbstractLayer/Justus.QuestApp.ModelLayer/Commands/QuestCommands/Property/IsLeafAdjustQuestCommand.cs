using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property
{
    /// <summary>
    /// Adjusts IsLeaf property of the quest.
    /// </summary>
    public class IsLeafAdjustQuestCommand : PropertyChangeQuestCommand<bool>
    {
        #region PropertyChangeQuestCommand overriding

        ///<inheritdoc/>
        protected override bool GetPropertyValue(Quest quest)
        {
            return quest.IsLeaf;
        }

        ///<inheritdoc/>
        protected override void SetPropertyValue(Quest quest, bool propertValue)
        {
            quest.IsLeaf = propertValue;
        }

        ///<inheritdoc/>
        protected override void ActionOnQuest(Quest quest, bool oldValue)
        {
            quest.IsLeaf = quest.Children == null || quest.Children.Count == 0;
        }

        #endregion
    }
}
