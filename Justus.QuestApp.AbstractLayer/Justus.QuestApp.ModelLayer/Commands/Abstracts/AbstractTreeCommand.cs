using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// Abstract command for repositories commands.
    /// </summary>
    public abstract class AbstractTreeCommand : SwitchCommand
    {
        /// <summary>
        /// Reference to questTree.
        /// </summary>
        protected IQuestTree QuestTree;

        /// <summary>
        /// Initialize questTree reference.
        /// </summary>
        /// <param name="questTree"></param>
        protected AbstractTreeCommand(IQuestTree questTree)
        {
            questTree.ThrowIfNull(nameof(questTree));
            QuestTree = questTree;
        }

        #region SwitchCommand overriding

        ///<inheritdoc cref="SwitchCommand"/>
        protected override bool InnerCommit()
        {
            QuestTree.Save();
            return true;
        }

        #endregion
    }
}
