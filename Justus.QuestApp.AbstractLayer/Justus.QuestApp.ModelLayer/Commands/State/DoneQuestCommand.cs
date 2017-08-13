using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State.Common;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// Marks quest as done.
    /// </summary>
    public class DoneQuestCommand : ChangeStateUpHierarchyIfChildrenHaveTheSameState
    {
        public DoneQuestCommand(Quest quest, IQuestTree questTree) : base(quest, questTree, AbstractLayer.Entities.Quest.State.Done)
        {
        }
    }
}
