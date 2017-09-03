using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers
{
    public class CompositeQuestCommand : IQuestCommand
    {
        private readonly IQuestCommand[] _innerCommands;

        public CompositeQuestCommand(params IQuestCommand[] innerCommands)
        {
            innerCommands.ThrowIfNull(nameof(innerCommands));
            _innerCommands = innerCommands;
        }

        ///<inheritdoc/>
        public bool Execute(Quest quest)
        {
            int length = _innerCommands.Length;

            bool result = true;

            for (int i = 0; i < length; ++i)
            {
                if (!_innerCommands[i].Execute(quest))
                {
                    result = false;
                }
            }

            return result;
        }

        ///<inheritdoc/>
        public bool Undo(Quest quest)
        {
            int length = _innerCommands.Length;

            bool result = true;

            for (int i = length - 1; i >= 0; --i)
            {
                if (!_innerCommands[i].Undo(quest))
                {
                    result = false;
                }
            }

            return result;
        }

        ///<inheritdoc/>
        public bool Commit()
        {
            int length = _innerCommands.Length;
            for (int i = 0; i < length; ++i)
            {
                if (!_innerCommands[i].Commit())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
