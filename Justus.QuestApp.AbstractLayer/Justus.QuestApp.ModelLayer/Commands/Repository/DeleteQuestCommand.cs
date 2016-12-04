using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

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
            _repository = repository;
            _toDelete = questToDelete;
        }

        #region AbstractRepositoryCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if (!_hasExecuted)
            {
                _parentOfDeleted = _toDelete.Parent;
                if(_parentOfDeleted != null)
                {
                    _parentOfDeleted.Children.Remove(_toDelete);
                }
                else
                {
                    //If quest does not have parent -> it is top level quest.
                    _repository.GetAll().Remove(_toDelete);
                }
                _repository.Delete(_toDelete);
                _hasExecuted = true;
            }        
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (_hasExecuted)
            {
                _repository.RevertDelete(_toDelete);
                if(_parentOfDeleted != null)
                {
                    _parentOfDeleted.Children.Add(_toDelete);
                }
                else
                {
                    _repository.GetAll().Add(_toDelete);
                }
                _hasExecuted = false;
            }
        }

        #endregion
    }
}
