using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Commands.Wrappers
{
    public class RecountQuestProgressCommandWrapper : Command
    {
        private readonly Command _innerCommand;
        private readonly Quest _target;
        private readonly IQuestProgressRecounter _recountStrategy;

        /// <summary>
        /// Receives inner command, quest target and recount strategy
        /// </summary>
        /// <param name="innerCommand"></param>
        /// <param name="target"></param>
        /// <param name="recountStrategy"></param>
        public RecountQuestProgressCommandWrapper(Command innerCommand, Quest target, IQuestProgressRecounter recountStrategy)
        {
            innerCommand.ThrowIfNull(nameof(innerCommand));
            target.ThrowIfNull(nameof(target));
            recountStrategy.ThrowIfNull(nameof(recountStrategy));

            _innerCommand = innerCommand;
            _target = target;
            _recountStrategy = recountStrategy;
        }

        #region Command implementation

        ///<inheritdoc cref="Command"/>
        public override bool Execute()
        {
            if (_innerCommand.Execute())
            {
                _recountStrategy.RecountProgress(_target);
                return true;
            }
            return false;
        }

        ///<inheritdoc cref="Command"/>
        public override bool Undo()
        {
            if (_innerCommand.Undo())
            {
                _recountStrategy.RecountProgress(_target);
                return true;
            }
            return false;
        }

        #endregion
    }
}
