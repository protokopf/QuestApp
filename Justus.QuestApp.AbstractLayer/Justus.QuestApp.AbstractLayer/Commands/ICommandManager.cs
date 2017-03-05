namespace Justus.QuestApp.AbstractLayer.Commands
{
    /// <summary>
    /// Manages commands.
    /// </summary>
    public interface ICommandManager
    {
        /// <summary>
        /// Adds command to commands queue.
        /// </summary>
        /// <param name="command"></param>
        void Add(Command command);

        /// <summary>
        /// Revert last done command.
        /// </summary>
        void Undo();

        /// <summary>
        /// Execute top command.
        /// </summary>
        void Do();
    }
}
