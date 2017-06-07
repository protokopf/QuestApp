using System;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /*
     * Получает ссылку на список всех квестов через репозиторий. Изменяет полученную информацию.
     * Выполняет команду только в том случае, если команда валидна.
     * Отменяет команду только в том случае, если команда была валидна на момент проверки валидности / выполнения.
     */

    /// <summary>
    /// Command, which deletes quest from repository.
    /// </summary>
    public class DeleteQuestCommand : AbstractRepositoryCommand
    {
        private readonly Quest _toDelete = null;

        /// <summary>
        /// Receives references to repository and quest to delete.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="questToDelete"></param>
        public DeleteQuestCommand(IQuestRepository repository, Quest questToDelete) : base(repository)
        {
            if (questToDelete == null)
            {
                throw new ArgumentNullException(nameof(questToDelete));
            }
            Repository = repository;
            _toDelete = questToDelete;
        }

        #region AbstractRepositoryCommand overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {
            Repository.Delete(_toDelete);
            return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            Repository.RevertDelete(_toDelete);
            return true;
        }

        #endregion
    }
}
