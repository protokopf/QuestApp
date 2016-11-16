namespace Justus.QuestApp.AbstractLayer.Entities.Quest
{
    /// <summary>
    /// All possible quest states.
    /// </summary>
    public enum QuestState
    {
        /// <summary>
        /// Quest is ready for picking up.
        /// </summary>
        Idle = 0,

        /// <summary>
        /// Quest is in progress now.
        /// </summary>
        Progress = 1,

        /// <summary>
        /// Quest successfully finished.
        /// </summary>
        Done = 2,

        /// <summary>
        /// Quest is failed.
        /// </summary>
        Failed = 3
    }
}
