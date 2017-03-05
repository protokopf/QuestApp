using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// Updates state of current quest and each child quest in quest tree.
    /// </summary>
    public class DownHierarchyStateUpdateCommand : BaseStateUpdateCommand
    {
        private readonly QuestState _state;
        private readonly Dictionary<Quest, QuestState> _statesDictionary; 

        public DownHierarchyStateUpdateCommand(Quest quest, QuestState state, IQuestRepository repository) : base(quest, repository)
        {
            _state = state;
            _statesDictionary = new Dictionary<Quest, QuestState>();
        }

        #region Command overriding

        ///<inehritdoc/>
        public override void Execute()
        {
            if (!HasExecuted)
            {
                _statesDictionary.Add(QuestRef, QuestRef.CurrentState);
                QuestRef.CurrentState = _state;
                Repository.Update(QuestRef);
                if (QuestRef.Children != null)
                {
                    AssignStateDownToHierarchy(QuestRef.Children, _state);
                }
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if(HasExecuted)
            {
                RevertChanges();
                HasExecuted = false;
            }
        }
        #endregion

        #region Private methods

        private void AssignStateDownToHierarchy(List<Quest> children, QuestState state)
        {
            int length = children.Count;
            for (int i = 0; i < length; ++i)
            {
                _statesDictionary.Add(children[i], children[i].CurrentState);
                children[i].CurrentState = _state;
                Repository.Update(children[i]);
                if (children[i].Children != null && children[i].Children.Count != 0)
                {
                    AssignStateDownToHierarchy(children[i].Children, state);
                }
            }
        }

        private void RevertChanges()
        {
            foreach (var item in _statesDictionary)
            {
                item.Key.CurrentState = item.Value;
                Repository.RevertUpdate(item.Key);
            }
            _statesDictionary.Clear();
        }

        #endregion
    }
}
