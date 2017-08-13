using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Traverse up quest hierarchy.
    /// </summary>
    public abstract class UpHierarchyQuestCommand : AbstractQuestCommand
    {
        /// <summary>
        /// Receives quest to traverse and quest tree.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="tree"></param>
        protected UpHierarchyQuestCommand(Quest quest) : base(quest)
        {
        }

        #region BaseQuestCommand overriding

        ///<inheritdoc cref="BaseQuestCommand"/>
        protected override bool InnerExecute()
        {
            TraverseUpHierarchy(QuestRef, ExecuteOnQuest);
            return true;
        }

        ///<inheritdoc cref="BaseQuestCommand"/>
        protected override bool InnerUndo()
        {
            TraverseUpHierarchy(QuestRef, UndoOnQuest);
            return true;
        }

        #endregion

        #region Protected virtual methods

        /// <summary>
        /// Points, whether command should traverse up for current quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        protected abstract bool ShouldStopTraversing(Quest quest);

        #endregion

        private void TraverseUpHierarchy(Quest quest, Action<Quest> questAction)
        {
            while (!ShouldStopTraversing(quest))
            {
                questAction?.Invoke(quest);
                quest = quest.Parent;
            }
        }
    }
}
