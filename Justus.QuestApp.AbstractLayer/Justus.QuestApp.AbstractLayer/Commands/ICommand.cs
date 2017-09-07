namespace Justus.QuestApp.AbstractLayer.Commands
{
    /// <summary>
    /// Abstract type for all commands.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        bool Execute();

        /// <summary>
        /// Reverts command influence.
        /// </summary>
        bool Undo();

        /// <summary>
        /// Commits any changes made by command.
        /// </summary>
        bool Commit();

        /// <summary>
        /// Check, whether command is valid or not.
        /// </summary>
        /// <returns></returns>
        bool IsValid();
    }
}
