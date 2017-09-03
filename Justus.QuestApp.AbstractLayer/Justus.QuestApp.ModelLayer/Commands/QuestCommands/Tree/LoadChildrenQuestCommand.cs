using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Tree
{
    public class LoadChildrenQuestCommand : IQuestCommand
    {
        private readonly IQuestTree _questTree;

        /// <summary>
        /// Receives IQuestTree implementation.
        /// </summary>
        /// <param name="questTree"></param>
        public LoadChildrenQuestCommand(IQuestTree questTree)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Execute(Quest quest)
        {
            _questTree.LoadChildren(quest);
            return true;
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Undo(Quest quest)
        {
            _questTree.UnloadChildren(quest);
            return true;
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Commit()
        {
            return true;
        }
    }
}
