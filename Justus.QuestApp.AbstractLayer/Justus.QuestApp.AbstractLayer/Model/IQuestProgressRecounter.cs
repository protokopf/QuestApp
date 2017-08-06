using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for types responsible for recounting quest progress.
    /// </summary>
    public interface IQuestProgressRecounter
    {
        /// <summary>
        /// Recounts progress value for quest.
        /// </summary>
        /// <param name="quest"></param>
        void RecountProgress(Quest quest);
    }
}
