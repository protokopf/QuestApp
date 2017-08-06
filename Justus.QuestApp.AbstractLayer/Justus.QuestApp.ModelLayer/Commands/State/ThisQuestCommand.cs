using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// ICommand, which updates state only of given quest.
    /// </summary>
    public class ThisQuestCommand : BaseQuestCommand
    {
        private readonly AbstractLayer.Entities.Quest.State _state;
        private AbstractLayer.Entities.Quest.State _previousState;

        /// <summary>
        /// Receives quest and tree.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="state"></param>
        /// <param name="tree"></param>
        public ThisQuestCommand(Quest quest, AbstractLayer.Entities.Quest.State state, IQuestTree tree) : base(quest, tree)
        {
            _state = state;
        }

        #region ICommand overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {
            _previousState = QuestRef.State;
            QuestRef.State = _state;
            QuestTree.Update(QuestRef);
            return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            QuestRef.State = _previousState;
            QuestTree.RevertUpdate(QuestRef);
            return true;
        } 

        #endregion
    }
}
