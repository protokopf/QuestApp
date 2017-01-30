using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override void Execute()
        {
            if (!HasExecuted)
            {
                _previousState = QuestRef.CurrentState;
                QuestRef.CurrentState = _state;
                Repository.Update(QuestRef);
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (HasExecuted)
            {
                QuestRef.CurrentState = _previousState;
                Repository.RevertUpdate(QuestRef);
                HasExecuted = false;
            }
        } 

        #endregion
    }
}
