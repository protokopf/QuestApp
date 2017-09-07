using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers.Hierarchy
{
    public class UpToRootQuestCommand : IQuestCommand
    {
        private readonly IQuestTree _questTree;
        private readonly IQuestCommand _innerCommand;

        public UpToRootQuestCommand(IQuestTree questTree, IQuestCommand innerCommand)
        {
            questTree.ThrowIfNull(nameof(questTree));
            innerCommand.ThrowIfNull(nameof(innerCommand));
            _questTree = questTree;
            _innerCommand = innerCommand;
        }

        public bool Execute(Quest quest)
        {
            TraverseUpHierarchy(quest, _innerCommand.Execute);
            return true;
        }

        public bool Undo(Quest quest)
        {
            TraverseUpHierarchy(quest, _innerCommand.Undo);
            return true;
        }

        public bool Commit()
        {
            return _innerCommand.Commit();
        }

        private void TraverseUpHierarchy(Quest quest, Func<Quest, bool> questAction)
        {
            while (quest != _questTree.Root)
            {
                questAction.Invoke(quest);
                quest = quest.Parent;
            }
        }
    }
}
