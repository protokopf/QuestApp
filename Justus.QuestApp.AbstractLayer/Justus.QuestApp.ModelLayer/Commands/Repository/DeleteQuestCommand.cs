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
    public class DeleteQuestCommand : RepositoryCommand
    {
        private bool _isValid = false;
        private bool _hasChecked = false;

        private List<Quest> _fromDelete = null;
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

        #region RepositoryCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if (!_hasExecuted)
            {
                if(!_hasChecked)
                {
                    IsValid();
                }
                if (_isValid)
                {
                    _repository.Delete(_toDelete);
                    _fromDelete.Remove(_toDelete);
                    _hasExecuted = true;
                }
            }        
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (_hasExecuted)
            {
                _repository.RevertDelete(_toDelete);
                //TODO: вставить команду обратно в список детей того родителя, откуда команда удалилась!
                _fromDelete.Add(_toDelete);
                _hasExecuted = false;
                _hasChecked = false;
            }
        }

        ///<inheritdoc/>
        public override bool IsValid()
        {
            _isValid = FindRecursive(_repository.GetAll(), _toDelete);
            _hasChecked = true;
            return _isValid;
        }

        #endregion

        private bool FindRecursive(List<Quest> quests, Quest toDelete)
        {
            if (quests == null || quests.Count == 0)
            {
                return false;
            }
            if (quests.Find(quest => quest == toDelete)!= null)
            {
                _fromDelete = quests;
                return true;
            }
            foreach (Quest quest in quests)
            {
                if (FindRecursive(quest.Children, toDelete))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
