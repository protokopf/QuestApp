using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// Base class for command, which changes state and updates command.
    /// </summary>
    public abstract class BaseQuestCommand : AbstractTreeCommand
    {
        /// <summary>
        /// Guess what.
        /// </summary>
        protected Quest QuestRef;

        /// <summary>
        /// Receives quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="tree"></param>
        protected BaseQuestCommand(Quest quest, IQuestTree tree) : base(tree)
        {
            quest.ThrowIfNull(nameof(quest));
            QuestRef = quest;
        }
    }
}
