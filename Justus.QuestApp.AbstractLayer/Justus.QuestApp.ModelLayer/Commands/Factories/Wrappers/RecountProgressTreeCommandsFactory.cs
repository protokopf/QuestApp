using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;

namespace Justus.QuestApp.ModelLayer.Commands.Factories.Wrappers
{
    /// <summary>
    /// Wrapper for ITreeCommandsFactory, that provides wrapped commands.
    /// </summary>
    public class RecountProgressTreeCommandsFactory : ITreeCommandsFactory
    {
        private readonly ITreeCommandsFactory _innerFactory;
        private readonly IQuestProgressRecounter _progressRecounter;


        /// <summary>
        /// Receives references to wrapped factory and progress recounter;
        /// </summary>
        /// <param name="innerFactory"></param>
        /// <param name="progressRecounter"></param>
        public RecountProgressTreeCommandsFactory(ITreeCommandsFactory innerFactory,
            IQuestProgressRecounter progressRecounter)
        {
            innerFactory.ThrowIfNull(nameof(innerFactory));
            progressRecounter.ThrowIfNull(nameof(progressRecounter));
            _innerFactory = innerFactory;
            _progressRecounter = progressRecounter;
        }

        #region ITreeCommandsFactory implementation

        ///<inheritdoc cref="ITreeCommandsFactory"/>
        public ICommand AddQuest(Quest parent, Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.AddQuest(parent, quest), quest, _progressRecounter);
        }

        ///<inheritdoc cref="ITreeCommandsFactory"/>
        public ICommand DeleteQuest(Quest parent, Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.DeleteQuest(parent, quest), parent, _progressRecounter);
        }

        ///<inheritdoc cref="ITreeCommandsFactory"/>
        public ICommand UpdateQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.UpdateQuest(quest), quest, _progressRecounter);
        }

        #endregion


    }
}
