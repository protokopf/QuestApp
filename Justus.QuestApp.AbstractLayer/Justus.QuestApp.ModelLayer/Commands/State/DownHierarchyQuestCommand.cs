using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// Updates newState of current quest and each child quest in quest tree.
    /// </summary>
    public class DownHierarchyQuestCommand : BaseQuestCommand
    {
        private readonly AbstractLayer.Entities.Quest.State _newState;
        private readonly Dictionary<Quest, AbstractLayer.Entities.Quest.State> _statesDictionary; 

        public DownHierarchyQuestCommand(Quest quest, AbstractLayer.Entities.Quest.State newState, IQuestTree tree) : base(quest, tree)
        {
            _newState = newState;
            _statesDictionary = new Dictionary<Quest, AbstractLayer.Entities.Quest.State>();
        }

        #region ICommand overriding

        ///<inehritdoc/>
        protected override bool InnerExecute()
        {
            _statesDictionary.Add(QuestRef, QuestRef.State);
            QuestRef.State = _newState;
            QuestTree.Update(QuestRef);
            if (QuestRef.Children != null)
            {
                AssignStateDownToHierarchy(QuestRef.Children, _newState);
            }
            return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            RevertChanges();
            return true;
        }
        #endregion

        #region Private methods

        private void AssignStateDownToHierarchy(List<Quest> children, AbstractLayer.Entities.Quest.State newState)
        {
            int length = children.Count;
            for (int i = 0; i < length; ++i)
            {
                _statesDictionary.Add(children[i], children[i].State);
                children[i].State = newState;
                QuestTree.Update(children[i]);
                if (children[i].Children != null && children[i].Children.Count != 0)
                {
                    AssignStateDownToHierarchy(children[i].Children, newState);
                }
            }
        }

        private void RevertChanges()
        {
            foreach (var item in _statesDictionary)
            {
                item.Key.State = item.Value;
                QuestTree.RevertUpdate(item.Key);
            }
            _statesDictionary.Clear();
        }

        #endregion
    }
}
