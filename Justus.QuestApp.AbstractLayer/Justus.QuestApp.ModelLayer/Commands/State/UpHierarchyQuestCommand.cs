using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// ICommand which marks quest as done.
    /// </summary>
    public class UpHierarchyQuestCommand : BaseQuestCommand
    {
        private readonly Dictionary<Quest, AbstractLayer.Entities.Quest.State> _changedQuests;
        private readonly AbstractLayer.Entities.Quest.State _newState;

        /// <summary>
        /// Receives quest and tree.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="newState"></param>
        /// <param name="tree"></param>
        public UpHierarchyQuestCommand(Quest quest, AbstractLayer.Entities.Quest.State newState, IQuestTree tree) : base(quest, tree)
        {
            _newState = newState;
            _changedQuests = new Dictionary<Quest, AbstractLayer.Entities.Quest.State>();
        }

        #region BaseQuestCommand overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {           
            AssignStateAllParentHierarchyIfSiblingsHasSameState(QuestRef, _newState);
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

        private void AssignStateAllParentHierarchyIfSiblingsHasSameState(Quest parent, AbstractLayer.Entities.Quest.State newState)
        {
            while (true)
            {
                if (parent == null)
                {
                    break;
                }
                if (AllQuestsHasState(parent.Children, newState))
                {
                    _changedQuests.Add(parent, parent.State);
                    parent.State = newState;
                    QuestTree.Update(parent);
                    parent = parent.Parent;
                }
                else
                {
                    break;
                }               
            }
        }

        private bool AllQuestsHasState(List<Quest> quests, AbstractLayer.Entities.Quest.State state)
        {
            int length = quests.Count;
            for (int i = 0; i < length; ++i)
            {
                if (quests[i].State != state)
                {
                    return false;
                }
            }
            return true;
        }

        private void RevertChanges()
        {
            foreach(KeyValuePair<Quest, AbstractLayer.Entities.Quest.State> item in _changedQuests)
            {
                item.Key.State = item.Value;
                QuestTree.RevertUpdate(item.Key);
            }
            _changedQuests.Clear();
        }

        #endregion


    }
}
