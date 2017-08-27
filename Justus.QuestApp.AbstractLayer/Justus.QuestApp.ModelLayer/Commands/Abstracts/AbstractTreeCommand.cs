using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// Abstract command for repositories commands.
    /// </summary>
    public abstract class AbstractTreeCommand : ICommand
    {
        /// <summary>
        /// Reference to questTree.
        /// </summary>
        protected readonly IQuestTree QuestTree;

        /// <summary>
        /// Initialize questTree reference.
        /// </summary>
        /// <param name="questTree"></param>
        protected AbstractTreeCommand(IQuestTree questTree)
        {
            questTree.ThrowIfNull(nameof(questTree));
            QuestTree = questTree;
        }

        #region ICommand implementation

        ///<inheritdoc cref="ICommand"/>
        public abstract bool Execute();

        ///<inheritdoc cref="ICommand"/>
        public abstract bool Undo();

        ///<inheritdoc cref="ICommand"/>
        public virtual bool IsValid()
        {
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public virtual bool Commit()
        {
            QuestTree.Save();
            return true;
        }

        #endregion
    }
}
