using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Executes inner command on quest hierarchy until root quest is reached.
    /// </summary>
    public class UpToRootQuestCommand : UpHierarchyQuestCommand
    {
        private readonly IQuestTree _questTree;

        /// <summary>
        /// Receives quest target, quest tree and quest command.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="questTree"></param>
        /// <param name="questCommand"></param>
        public UpToRootQuestCommand(Quest quest, IQuestTree questTree, IQuestCommand questCommand) : base(quest, questCommand)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override bool ShouldStopTraversing(Quest quest)
        {
            return quest == _questTree.Root;
        }
    }
}
