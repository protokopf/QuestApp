using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;

namespace Justus.QuestApp.ModelLayer.Commands.Other
{
    /// <summary>
    /// Recounts progress for target quest.
    /// </summary>
    public class RecountQuestProgressCommand : UpHierarchyQuestCommand
    {
        private readonly IQuestTree _questTree;
        private readonly Dictionary<Quest, double> _progressesDictionary;

        /// <summary>
        /// Receives target quest.
        /// </summary>
        public RecountQuestProgressCommand(Quest quest, IQuestTree questTree) : base(quest)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;

            _progressesDictionary = new Dictionary<Quest, double>();
        }

        #region UpHierarchyQuestCommand implementation

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        public override bool Commit()
        {
            return true;
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            _progressesDictionary.Add(quest, quest.Progress);
            if (quest.Children != null && quest.Children.Count != 0)
            {
                quest.Progress = quest.Children.Average(q => q.Progress);
            }
            else
            {
                quest.Progress = quest.State == AbstractLayer.Entities.Quest.State.Done ? 1 : 0;
            }
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            quest.Progress = _progressesDictionary[quest];
            _progressesDictionary.Remove(quest);
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override bool ShouldStopTraversing(Quest quest)
        {
            return quest == _questTree.Root;
        }

        #endregion
    }
}
