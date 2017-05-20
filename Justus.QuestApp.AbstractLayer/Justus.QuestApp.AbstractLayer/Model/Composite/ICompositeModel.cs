using System.Collections.Generic;

namespace Justus.QuestApp.AbstractLayer.Model.Composite
{
    /// <summary>
    /// Interface for composite models.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICompositeModel<TEntity>
    {
        /// <summary>
        /// Current entity, which may have leaves.
        /// </summary>
        TEntity Root { get; set; }

        /// <summary>
        /// Leaves of current bean.
        /// </summary>
        List<TEntity> Leaves { get; }
    }
}
