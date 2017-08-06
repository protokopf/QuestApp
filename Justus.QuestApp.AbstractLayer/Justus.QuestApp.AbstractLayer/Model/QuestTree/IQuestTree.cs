using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Behaviours;

namespace Justus.QuestApp.AbstractLayer.Model.QuestTree
{
    /// <summary>
    /// Represents quest tree interface.
    /// </summary>
    public interface IQuestTree : IInitializable
    {
        /// <summary>
        /// Returns main root of quests tree.
        /// </summary>
        Quest Root { get; }

        /// <summary>
        /// Returns quest from tree by predicate
        /// </summary>
        /// <param name="questPredicate"></param>
        /// <returns></returns>
        Quest Get(Predicate<Quest> questPredicate);

        /// <summary>
        /// Loads children for particular quest.
        /// </summary>
        /// <returns></returns>
        void LoadChildren(Quest quest);

        /// <summary>
        /// Unloads children from quest tree.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        void UnloadChildren(Quest quest);

        /// <summary>
        /// Insert entity.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        void AddChild(Quest parent, Quest child);

        /// <summary>
        /// Deletes entity by id.
        /// </summary>
        void RemoveChild(Quest parent, Quest child);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity"></param>
        void Update(Quest entity);

        /// <summary>
        /// Reverts entity inserting.
        /// </summary>
        /// <param name="entity"></param>
        void RevertUpdate(Quest entity);

        /// <summary>
        /// Saves any made changes.
        /// </summary>
        void Save();
    }
}
