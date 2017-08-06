using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Model.Progress
{
    /// <summary>
    /// Recounts progress for quest and its parent hierarchy.
    /// </summary>
    public class AllUpperQuestProgressRecounter : IQuestProgressRecounter
    {
        private readonly IQuestTree _questTree;

        /// <summary>
        /// Receives questTree as dependency.
        /// </summary>
        /// <param name="questTree"></param>
        public AllUpperQuestProgressRecounter(IQuestTree questTree)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
        }

        #region IQuestProgressRecounter implementation

        ///<inheritdoc cref="IQuestProgressRecounter"/>
        public void RecountProgress(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));

            Quest current = quest;
            while (current != null)
            {
                if (current.Children != null && current.Children.Count != 0)
                {
                    current.Progress = current.Children.Average(q => q.Progress);
                }
                else
                {
                    current.Progress = current.State == State.Done ? 1 : 0;
                }
                _questTree.Update(current);
                current = current.Parent;
            }
        } 

        #endregion
    }
}
