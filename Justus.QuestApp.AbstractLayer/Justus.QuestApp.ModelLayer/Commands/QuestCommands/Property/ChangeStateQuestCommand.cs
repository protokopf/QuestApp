using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property
{
    /// <summary>
    /// Changes state of given quest.
    /// </summary>
    public class ChangeStateQuestCommand : PropertyChangeQuestCommand<State>
    {
        private readonly State _newState;

        /// <summary>
        /// Receives state, that will be applied to quest.
        /// </summary>
        /// <param name="newState"></param>
        public ChangeStateQuestCommand(State newState)
        {
            _newState = newState;
        }

        #region PropertyChangeQuestCommand overriding

        ///<inheritdoc/>
        protected override State GetPropertyValue(Quest quest)
        {
            return quest.State;
        }

        ///<inheritdoc/>
        protected override void SetPropertyValue(Quest quest, State propertValue)
        {
            quest.State = propertValue;
        }

        ///<inheritdoc/>
        protected override void ActionOnQuest(Quest quest, State oldValue)
        {
            quest.State = _newState;
        }

        #endregion
    }
}
