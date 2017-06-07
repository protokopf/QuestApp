using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;

namespace Justus.QuestApp.ModelLayer.Commands.Factories.Wrappers
{
    /// <summary>
    /// Wrapper for state commands factory, that provide recount progress functionality for each command.
    /// </summary>
    public class RecountProgressStateCommandsFactory : IStateCommandsFactory
    {
        private readonly IStateCommandsFactory _innerFactory;
        private readonly IQuestProgressRecounter _progressRecounter;

        /// <summary>
        /// Receives references to wrapped factory and progress recounter;
        /// </summary>
        /// <param name="innerFactory"></param>
        /// <param name="progressRecounter"></param>
        public RecountProgressStateCommandsFactory(IStateCommandsFactory innerFactory,
            IQuestProgressRecounter progressRecounter)
        {
            innerFactory.ThrowIfNull(nameof(innerFactory));
            progressRecounter.ThrowIfNull(nameof(progressRecounter));
            _innerFactory = innerFactory;
            _progressRecounter = progressRecounter;
        }

        #region IStateCommandsFactory implementation

        ///<inheritdoc cref="IStateCommandsFactory"/>
        public Command CancelQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.CancelQuest(quest), quest, _progressRecounter);
        }

        ///<inheritdoc cref="IStateCommandsFactory"/>
        public Command DoneQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.DoneQuest(quest), quest, _progressRecounter);
        }

        ///<inheritdoc cref="IStateCommandsFactory"/>
        public Command FailQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.FailQuest(quest), quest, _progressRecounter);
        }

        ///<inheritdoc cref="IStateCommandsFactory"/>
        public Command StartQuest(Quest quest)
        {
            return new RecountQuestProgressCommandWrapper(_innerFactory.StartQuest(quest), quest, _progressRecounter);
        }

        #endregion
    }
}
