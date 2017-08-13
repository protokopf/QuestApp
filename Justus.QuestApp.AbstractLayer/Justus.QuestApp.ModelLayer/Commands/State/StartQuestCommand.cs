using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.Commands.State.Common;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// Starts current quest.
    /// </summary>
    public class StartQuestCommand : ChangeStateUpHierarchy
    {
        public StartQuestCommand(Quest quest, IQuestTree questTree) : base(quest, questTree, AbstractLayer.Entities.Quest.State.Progress)
        {
        }
    }
}
