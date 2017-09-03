using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Traverse up quest hierarchy.
    /// </summary>
    public abstract class UpHierarchyQuestCommand : ICommand
    {
        private readonly Quest _target;
        private readonly IQuestCommand _command;

        /// <summary>
        /// Receives quest to traverse and quest tree.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="questCommand"></param>
        protected UpHierarchyQuestCommand(Quest quest, IQuestCommand questCommand)
        {
            quest.ThrowIfNull(nameof(quest));
            questCommand.ThrowIfNull(nameof(questCommand));
            _target = quest;
            _command = questCommand;
        }

        #region ICommand implementation

        ///<inheritdoc cref="ICommand"/>
        public bool Execute()
        {
            TraverseUpHierarchy(_target, _command.Execute);
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Undo()
        {
            TraverseUpHierarchy(_target, _command.Undo);
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool IsValid()
        {
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Commit()
        {
            return _command.Commit();
        }

        #endregion

        /// <summary>
        /// Checks, whether traversing should stop on given quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        protected abstract bool ShouldStopTraversing(Quest quest);

        /// <summary>
        /// Traverse up to the hierarchy.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="questAction"></param>
        private void TraverseUpHierarchy(Quest quest, Func<Quest, bool> questAction)
        {
            while (!ShouldStopTraversing(quest))
            {
                questAction.Invoke(quest);
                quest = quest.Parent;
            }
        }
    }
}
