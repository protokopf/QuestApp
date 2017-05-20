namespace Justus.QuestApp.AbstractLayer.Model.Composite
{
    /// <summary>
    /// Interface for types, which implement composite traverse logic.
    /// </summary>
    public interface ICompositeTraversing
    {
        /// <summary>
        /// Traverse to most top entity in composite model.
        /// </summary>
        bool TraverseToRoot();

        /// <summary>
        /// Traverse to leaf.
        /// </summary>
        /// <param name="leafNumber"></param>
        bool TraverseToLeaf(int leafNumber);

        /// <summary>
        /// Traverse to parent of current root.
        /// </summary>
        bool TraverseToParent();
    }
}
