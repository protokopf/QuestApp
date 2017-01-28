using System;
using System.Collections.Generic;
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
        private Quest _parentOfDeleted = null;
        private Quest _toDelete = null;

        /// <summary>
        /// Receives references to repository and quest to delete.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="toDelete"></param>
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
        public override void Execute()
        {
            if (!HasExecuted)
            {
                _parentOfDeleted = _toDelete.Parent;
                if(_parentOfDeleted != null)
                {
                    _parentOfDeleted.Children.Remove(_toDelete);
                }
                else
                {
                    //If quest does not have parent -> it is top level quest.
                    Repository.GetAll().Remove(_toDelete);
                }
                Repository.Delete(_toDelete);
                HasExecuted = true;
            }        
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (HasExecuted)
            {
                Repository.RevertDelete(_toDelete);
                if(_parentOfDeleted != null)
                {
                    _parentOfDeleted.Children.Add(_toDelete);
                }
                else
                {
                    Repository.GetAll().Add(_toDelete);
                }
                HasExecuted = false;
            }
        }

        #endregion
    }
}
