using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// ICommand, which deletes quest from questTree.
    /// </summary>
    public class DeleteQuestCommand : AbstractTreeCommand
    {
        private readonly Quest _toDelete = null;
        private readonly Quest _parent = null;

        /// <summary>
        /// Receives references to questTree and quest to delete.
        /// </summary>
        /// <param name="questTree"></param>
        /// <param name="parent"></param>
        /// <param name="questToDelete"></param>
        public DeleteQuestCommand(IQuestTree questTree, Quest parent,  Quest questToDelete) : base(questTree)
        {
            parent.ThrowIfNull(nameof(parent));
            questToDelete.ThrowIfNull(nameof(questToDelete));

            _parent = parent;
            _toDelete = questToDelete;     
        }

        #region AbstractTreeCommand overriding

        ///<inheritdoc/>
        public override bool Execute()
        {
            QuestTree.RemoveChild(_parent, _toDelete);
            return true;
        }

        ///<inheritdoc/>
        public override bool Undo()
        {
            QuestTree.AddChild(_parent, _toDelete);
            return true;
        }

        #endregion
    }
}
