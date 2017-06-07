using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;

namespace Justus.QuestApp.ModelLayer.Commands.Factories.Wrappers
{
    /// <summary>
    /// Wrapper for IRepositoryCommandsFactory, that provides wrapped commands.
    /// </summary>
    public class RecountProgressRepositoryCommandsFactory : IRepositoryCommandsFactory
    {
        private readonly IRepositoryCommandsFactory _innerFactory;
        private readonly IQuestProgressRecounter _progressRecounter;


        /// <summary>
        /// Receives references to wrapped factory and progress recounter;
        /// </summary>
        /// <param name="innerFactory"></param>
        /// <param name="progressRecounter"></param>
        public RecountProgressRepositoryCommandsFactory(IRepositoryCommandsFactory innerFactory,
            IQuestProgressRecounter progressRecounter)
        {
            innerFactory.ThrowIfNull(nameof(innerFactory));
            progressRecounter.ThrowIfNull(nameof(progressRecounter));
            _innerFactory = innerFactory;
            _progressRecounter = progressRecounter;
        }

        #region IRepositoryCommandsFactory implementation

        ///<inheritdoc cref="IRepositoryCommandsFactory"/>
        public Command AddQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.AddQuest(quest), quest, _progressRecounter);
        }

        ///<inheritdoc cref="IRepositoryCommandsFactory"/>
        public Command DeleteQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.DeleteQuest(quest), quest, _progressRecounter);
        }

        ///<inheritdoc cref="IRepositoryCommandsFactory"/>
        public Command UpdateQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.UpdateQuest(quest), quest, _progressRecounter);
        }

        #endregion


    }
}
