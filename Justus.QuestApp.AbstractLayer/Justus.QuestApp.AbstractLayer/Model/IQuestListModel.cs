using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Behaviours;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Interface for types, which represents traversing model.
    /// </summary>
    public interface IQuestListModel : IRefreshable, IInitializable
    {
        /// <summary>
        /// Returns parent quest.
        /// </summary>
        Quest Parent { get; }

        /// <summary>
        /// Points, whether current model in root of quest tree.
        /// </summary>
        bool IsInTheRoot { get; }

        /// <summary>
        /// Returns List of current quests.
        /// </summary>
        List<Quest> Leaves { get; }

        /// <summary>
        /// Traverses to top root of quest tree.
        /// </summary>
        /// <returns></returns>
        bool TraverseToRoot();

        /// <summary>
        /// Traverses to parent of current leaves.
        /// </summary>
        /// <returns></returns>
        bool TraverseToParent();

        /// <summary>
        /// Traverses to specific leaf of current parent. This leaf becomes parent.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool TraverseToLeaf(int position);

        /// <summary>
        /// Represents order strategy, that will be used for ordering leaves on the same level.
        /// </summary>
        IQuestOrderStrategy OrderStrategy { get; set; }
    }
}
