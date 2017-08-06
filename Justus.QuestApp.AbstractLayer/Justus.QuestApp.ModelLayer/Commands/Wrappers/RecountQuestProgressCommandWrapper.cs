using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Commands.Wrappers
{
    public class RecountQuestProgressCommandWrapper : ICommand
    {
        private readonly ICommand _innerCommand;
        private readonly Quest _target;
        private readonly IQuestProgressRecounter _recountStrategy;

        /// <summary>
        /// Receives inner command, quest target and recount strategy
        /// </summary>
        /// <param name="innerCommand"></param>
        /// <param name="target"></param>
        /// <param name="recountStrategy"></param>
        public RecountQuestProgressCommandWrapper(ICommand innerCommand, Quest target, IQuestProgressRecounter recountStrategy)
        {
            innerCommand.ThrowIfNull(nameof(innerCommand));
            target.ThrowIfNull(nameof(target));
            recountStrategy.ThrowIfNull(nameof(recountStrategy));

            _innerCommand = innerCommand;
            _target = target;
            _recountStrategy = recountStrategy;
        }

        #region ICommand implementation

        ///<inheritdoc cref="ICommand"/>
        public bool Execute()
        {
            if (_innerCommand.Execute())
            {
                _recountStrategy.RecountProgress(_target);
                return true;
            }
            return false;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Undo()
        {
            if (_innerCommand.Undo())
            {
                _recountStrategy.RecountProgress(_target);
                return true;
            }
            return false;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool IsValid()
        {
            return _innerCommand.IsValid();
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Commit()
        {
            return _innerCommand.Commit();
        }

        #endregion
    }
}
