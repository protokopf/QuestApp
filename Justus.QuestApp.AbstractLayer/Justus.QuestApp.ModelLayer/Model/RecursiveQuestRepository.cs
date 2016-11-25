using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.DataLayer.Entities;

namespace Justus.QuestApp.ModelLayer.Model
{
    /*
     * Основные условия для данного типа:
     * - Вся информация обрабатывается с помощью внутреннего кеша.
     * - Изменения сохраняются только при вызове метода Save()/Dispose();
     * - При первой попытке получения данных тип обращается к внутренней реализации и извлекает оттуда информацию о квестах.
     * - В течении работы с объектом репозитория, все изменения с квестами отражаются только на кеше, но при этом во внутренние списки
     * сохраняется информация об изменениях - ссылки на удаленные, изменненные, добавленные квесты.
     * - При "освобождении" или "сохранении" объекта репозитория все изменения переносятся на внутреннее хранилище, для чего оно открывается,
     * а затем закрывается.
     */

    /// <summary>
    /// Gives access to recursive quest repository.
    /// </summary>
    public class RecursiveQuestRepository : IQuestRepository
    {
        /// <summary>
        /// Reference to data storage interface.
        /// </summary>
        private IDataAccessInterface<Quest> _dataStorage;

        /// <summary>
        /// Inner quest cache.
        /// </summary>
        private List<Quest> _innerCache;

        private bool _shouldRetrieveData;

        /// <summary>
        /// Receives data storage interface injection.
        /// </summary>
        /// <param name="dataStorageInterface"></param>
        public RecursiveQuestRepository(IDataAccessInterface<Quest> dataStorageInterface)
        {
            if (dataStorageInterface == null)
            {
                throw new ArgumentNullException(nameof(dataStorageInterface));
            }
            _dataStorage = dataStorageInterface;
            _innerCache = new List<Quest>();
        }

        #region IQuestRepository implementation

        ///<inheritdoc/>
        public void Dispose()
        {
            Save();
        }

        ///<inheritdoc/>
        public void Insert(Quest entity)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void InsertAll(List<Quest> entities)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void Update(Quest entity)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void UpdateAll(List<Quest> entities)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Quest Get(int id)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public List<Quest> GetAll()
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void Save()
        {
            throw new NotImplementedException();
        } 

        #endregion
    }
}
