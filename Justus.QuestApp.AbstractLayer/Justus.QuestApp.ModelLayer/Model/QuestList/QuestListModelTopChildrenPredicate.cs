using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Behaviours;
using Justus.QuestApp.AbstractLayer.Helpers.Extensions;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Model.QuestList
{
    /// <summary>
    /// Default implementation of IQuestListModel.
    /// </summary>
    public class QuestListModelTopChildrenPredicate : IQuestListModel
    {
        private readonly IQuestTree _questTree;
        private readonly Func<Quest, bool> _topChildrenPredicate;

        private Quest _parent;
        private List<Quest> _children;

        private bool _isInitialized = false;

        /// <summary>
        /// Receives IQuestTree dependency through this constructor.
        /// </summary>
        /// <param name="questTree"></param>
        /// <param name="topChildrenPredicate"></param>
        public QuestListModelTopChildrenPredicate(IQuestTree questTree, Func<Quest, bool> topChildrenPredicate)
        {
            questTree.ThrowIfNull(nameof(questTree));
            topChildrenPredicate.ThrowIfNull(nameof(topChildrenPredicate));
            _questTree = questTree;
            _topChildrenPredicate = topChildrenPredicate;
        }

        #region IInitializable implementation

        ///<inheritdoc cref="IInitializable"/>
        public void Initialize()
        {
            if (!_isInitialized)
            {
                if (!_questTree.IsInitialized())
                {
                    _questTree.Initialize();
                }
                Quest topRoot = _questTree.Root;
                if (topRoot.Children == null)
                {
                    _questTree.LoadChildren(topRoot);
                }
                _parent = topRoot;
                _children = HandleChildren(_parent.Children);

                _isInitialized = true;
            }
        }
        ///<inheritdoc cref="IInitializable"/>
        public bool IsInitialized()
        {
            return _isInitialized;
        }

        #endregion

        #region IQuestListModel implementation

        ///<inheritdoc cref="IQuestListModel"/>
        public Quest Parent => _parent;

        ///<inheritdoc cref="IQuestListModel"/>
        public bool IsInTheRoot => _parent == _questTree.Root;

        ///<inheritdoc cref="IQuestListModel"/>
        public List<Quest> Leaves => _children;

        ///<inheritdoc cref="IQuestListModel"/>
        public bool TraverseToRoot()
        {
            if (IsInTheRoot)
            {
                return false;
            }
            while (!IsInTheRoot)
            {
                TraverseToParent();
            }
            return true;
        }

        ///<inheritdoc cref="IQuestListModel"/>
        public bool TraverseToParent()
        {
            if (IsInTheRoot)
            {
                return false;
            }
            _questTree.UnloadChildren(_parent);
            _parent = _parent.Parent;
            _children = HandleChildren(_parent.Children);
            return true;
        }

        ///<inheritdoc cref="IQuestListModel"/>
        public bool TraverseToLeaf(int position)
        {
            if (_children == null || !_children.InRange(position))
            {
                return false;
            }
            _parent = _children[position];
            _questTree.LoadChildren(_parent);
            _children = HandleChildren(_parent.Children);
            return true;
        }

        ///<inheritdoc cref="IQuestListModel"/>
        public IQuestOrderStrategy OrderStrategy { get; set; }

        #endregion

        #region IRefreshable implementation

        ///<inheritdoc cref="IRefreshable"/>
        public void Refresh()
        {
            _children = HandleChildren(_parent.Children);
        }

        #endregion

        private List<Quest> HandleChildren(List<Quest> children)
        {
            IEnumerable<Quest> handledQuests = children;

            if (children != null)
            {
                //If we are in the root
                if (IsInTheRoot)
                {
                    //Only quests, that match provided state, will be chosen.
                    handledQuests = children.Where(_topChildrenPredicate);
                }
                //If order strategy is specified
                if (OrderStrategy != null)
                {
                    //Quests will be ordered.
                    handledQuests = OrderStrategy.Order(handledQuests);
                }
            }
            return handledQuests?.ToList();
        }
    }
}
