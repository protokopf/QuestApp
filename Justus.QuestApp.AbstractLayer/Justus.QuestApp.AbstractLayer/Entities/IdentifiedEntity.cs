namespace Justus.QuestApp.AbstractLayer.Entities
{
    /// <summary>
    /// Abstract type for all identified entities.
    /// </summary>
    public abstract class IdentifiedEntity
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public abstract int Id { get; set; }
    }
}
