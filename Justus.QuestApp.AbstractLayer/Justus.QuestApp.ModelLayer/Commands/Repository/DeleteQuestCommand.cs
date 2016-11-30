using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
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
    public class DeleteQuestCommand : Command
    {
        private bool _isValid = false;
        private bool _hasChecked = false;
        private bool _hasExecuted = false;

        private List<Quest> _fromDelete = null;
        private IQuestRepository _repository = null;
        private Quest _toDelete = null;

        /// <summary>
        /// Receives references to repository and quest to delete.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="toDelete"></param>
        public DeleteQuestCommand(IQuestRepository repository, Quest questToDelete)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (questToDelete == null)
            {
                throw new ArgumentNullException(nameof(questToDelete));
            }
            _repository = repository;
            _toDelete = questToDelete;
        }

        #region Command overriding

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

        private static bool DeleteRecursive(List<Quest> quests,Quest toDelete)
        {
            if(quests == null || quests.Count == 0)
            {
                return false;
            }
            if(quests.Remove(toDelete))
            {
                return true;
            }
            foreach(Quest quest in quests)
            {
                if(DeleteRecursive(quest.Children,toDelete))
                {
                    return true;
                }
            }
            return false;
        }

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
