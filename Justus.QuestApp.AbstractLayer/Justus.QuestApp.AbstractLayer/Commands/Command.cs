namespace Justus.QuestApp.AbstractLayer.Commands
{
    /// <summary>
    /// Abstract type for all commands.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Reverts command influence.
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// Check, whether command is valid or not.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            return true;
        }
    }
}
