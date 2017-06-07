using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// Command, which updates state only of given quest.
    /// </summary>
    public class ThisStateUpdateCommand : BaseStateUpdateCommand
    {
        private readonly QuestState _state;
        private QuestState _previousState;

        /// <summary>
        /// Receives quest and repository.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="state"></param>
        /// <param name="repository"></param>
        public ThisStateUpdateCommand(Quest quest, QuestState state, IQuestRepository repository) : base(quest, repository)
        {
            _state = state;
        }

        #region Command overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {
            _previousState = QuestRef.CurrentState;
            QuestRef.CurrentState = _state;
            Repository.Update(QuestRef);
            return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            QuestRef.CurrentState = _previousState;
            Repository.RevertUpdate(QuestRef);
            return true;
        } 

        #endregion
    }
}
