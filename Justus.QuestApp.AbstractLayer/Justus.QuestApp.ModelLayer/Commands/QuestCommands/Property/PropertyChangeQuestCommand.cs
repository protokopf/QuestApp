using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property
{
    /// <summary>
    /// Abstract type for changing properties.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    public abstract class PropertyChangeQuestCommand<TProperty> : IQuestCommand
    {
        private readonly Dictionary<Quest, TProperty> _questToProperty;

        /// <summary>
        /// Default ctor. Initializes inner dictionary.
        /// </summary>
        protected PropertyChangeQuestCommand()
        {
            _questToProperty = new Dictionary<Quest, TProperty>();
        }

        #region IQuestCommand implementation

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Execute(Quest quest)
        {
            if (_questToProperty.ContainsKey(quest))
            {
                return false;
            }
            TProperty currentValue = GetPropertyValue(quest);
            //TODO: somehow get old value from the quest and save it to dictionary.
            _questToProperty.Add(quest, currentValue);
            //TODO: somehow assign something to the property
            ActionOnQuest(quest, currentValue);
            return true;
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Undo(Quest quest)
        {
            TProperty previousValue;
            if (!_questToProperty.TryGetValue(quest, out previousValue))
            {
                return false;
            }
            //TODO: somehow assign value to the quest
            SetPropertyValue(quest, previousValue);
            _questToProperty.Remove(quest);
            return true;
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Commit()
        {
            _questToProperty.Clear();
            return true;
        }

        #endregion

        /// <summary>
        /// Returns properties value of the given Quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        protected abstract TProperty GetPropertyValue(Quest quest);

        /// <summary>
        /// Sets the given value for the property of the given quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="propertValue"></param>
        protected abstract void SetPropertyValue(Quest quest, TProperty propertValue);

        /// <summary>
        /// Makes some changes on quest. Passes old value of property.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="oldValue"></param>
        protected abstract void ActionOnQuest(Quest quest, TProperty oldValue);
    }
}
