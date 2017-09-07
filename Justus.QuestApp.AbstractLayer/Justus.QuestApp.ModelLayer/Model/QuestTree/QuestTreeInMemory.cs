using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Model.QuestTree
{
    public class QuestTreeInMemory : IQuestTree
    {
        private const int TopRootId = 1;
        private readonly object _locker = new object();

        /// <summary>
        /// Contains all quests in flat list. Is used for faster search.
        /// </summary>
        private readonly List<Quest> _flatQuestTree;

        private readonly HashSet<Quest> _toRemove;
        private readonly HashSet<Quest> _toAdd;
        private readonly HashSet<Quest> _toUpdate;

        private readonly IQuestDataLayer _dataLayer;
        private readonly IQuestFactory _questFactory;

        private Quest _innerRoot;
        private bool _isInitialized = false;

        /// <summary>
        /// Receives data layer implementation as dependency.
        /// </summary>
        /// <param name="dataLayer"></param>
        /// <param name="questFactory"></param>
        public QuestTreeInMemory(IQuestDataLayer dataLayer, IQuestFactory questFactory)
        {
            dataLayer.ThrowIfNull(nameof(dataLayer));
            questFactory.ThrowIfNull(nameof(questFactory));

            _dataLayer = dataLayer;
            _questFactory = questFactory;

            _flatQuestTree = new List<Quest>();
            _toRemove = new HashSet<Quest>();
            _toAdd = new HashSet<Quest>();
            _toUpdate = new HashSet<Quest>();
        }

        #region IQuestTree implementation

        ///<inheritdoc cref="IQuestTree"/>
        public void Initialize()
        {
            lock (_locker)
            {
                //TODO: Retrieve top root and its children. Add them to flat list.
                //TODO: If there is no top root - create and save it.
                if (_isInitialized == false)
                {
                    using (_dataLayer)
                    {
                        _dataLayer.Open();
                        Quest root = _dataLayer.Get(TopRootId);

                        //If quest there is not root in data layer, there will be no children for it.
                        if (root == null)
                        {
                            root = _questFactory.CreateQuest();
                            root.Id = TopRootId;
                            _dataLayer.Insert(root);
                        }
                        else
                        {
                            //Try load children for root from data layer.
                            InnerLoadChildren(root, _dataLayer);
                        }
                        _innerRoot = root;
                    }
                    _flatQuestTree.Add(_innerRoot);
                    _isInitialized = true;
                }
            }
        }

        ///<inheritdoc cref="IQuestTree"/>
        public bool IsInitialized()
        {
            return _isInitialized;
        }

        ///<inheritdoc cref="IQuestTree"/>
        public Quest Root => _innerRoot;

        ///<inheritdoc cref="IQuestTree"/>
        public Quest Get(Predicate<Quest> questPredicate)
        {
            questPredicate.ThrowIfNull(nameof(questPredicate));
            return _flatQuestTree.Find(questPredicate);
        }

        ///<inheritdoc cref="IQuestTree"/>
        public void LoadChildren(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            lock (_locker)
            {
                //TODO: Load quest children from repository. Add them to frat list and quest children.
                using (_dataLayer)
                {
                    _dataLayer.Open();
                    InnerLoadChildren(quest, _dataLayer);
                }
            }
        }

        ///<inheritdoc cref="IQuestTree"/>
        public void UnloadChildren(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            lock (_locker)
            {
                //TODO: Unload should also be recursive?
                quest.Children.Clear();
                _flatQuestTree.RemoveAll(q => q.ParentId == quest.Id && q.Parent == quest);
            }
        }

        ///<inheritdoc cref="IQuestTree"/>
        public void AddChild(Quest parent, Quest child)
        {
            parent.ThrowIfNull(nameof(parent));
            child.ThrowIfNull(nameof(child));
            lock (_locker)
            {
                BindChildAndParent(child, parent);

                _flatQuestTree.Add(child);

                if (_toRemove.Contains(child))
                {
                    _toRemove.Remove(child);
                }
                else
                {
                    _toAdd.Add(child);
                }
            }
        }

        ///<inheritdoc cref="IQuestTree"/>
        public void RemoveChild(Quest parent, Quest child)
        {
            parent.ThrowIfNull(nameof(parent));
            child.ThrowIfNull(nameof(child));
            //TODO: find way how to optimize deleting quests
            lock (_locker)
            {
                List<Quest> toRemoveQuests = new List<Quest>() {child};
                GatherAllHierarchy(child, ref toRemoveQuests);
                foreach (Quest quest in toRemoveQuests)
                {
                    InnerRemoveChild(quest.Parent, quest);
                }
            }
        }

        ///<inheritdoc cref="IQuestTree"/>
        public void Update(Quest entity)
        {
            entity.ThrowIfNull(nameof(entity));
            lock (_locker)
            {                
                if (_flatQuestTree.Contains(entity) && !_toAdd.Contains(entity))
                {
                    _toUpdate.Add(entity);
                }
            }
        }

        ///<inheritdoc cref="IQuestTree"/>
        public void RevertUpdate(Quest entity)
        {
            entity.ThrowIfNull(nameof(entity));
            lock (_locker)
            {
                _toUpdate.Remove(entity);
            }
        }

        ///<inheritdoc cref="IQuestTree"/>
        public void Save()
        {
            lock (_locker)
            {
                //TODO: For each _to<Action> make this actions.
                int removedLength = _toRemove.Count;
                int addedLength = _toAdd.Count;
                int updatedLength = _toUpdate.Count;

                int changed = removedLength + addedLength + updatedLength;
                if (changed > 0)
                {
                    using (_dataLayer)
                    {
                        _dataLayer.Open();
                        if (removedLength > 0)
                        {
                            foreach (Quest toRemove in _toRemove)
                            {
                                _dataLayer.Delete(toRemove.Id);
                            }
                            _toRemove.Clear();
                        }
                        if (addedLength > 0)
                        {
                            _dataLayer.InsertAll(_toAdd);
                            _toAdd.Clear();
                        }
                        if (updatedLength > 0)
                        {
                            _dataLayer.UpdateAll(_toUpdate);
                            _toUpdate.Clear();
                        }
                    }
                }
            }
        }

        #endregion

        private void InnerLoadChildren(Quest quest, IQuestDataLayer dataLayer)
        {
            if (!dataLayer.IsClosed())
            {
                List<Quest> children = dataLayer.GetAll(quest.Id)?.ToList();

                if (children == null)
                {
                    quest.Children = new List<Quest>();
                }
                else
                {
                    quest.Children = children;
                    BindChildrenToParent(quest);
                    _flatQuestTree.AddRange(quest.Children);
                }

            }
        }

        private void InnerRemoveChild(Quest parent, Quest child)
        {
            UnbindChildAndParent(child, parent);
            _flatQuestTree.Remove(child);

            if (_toAdd.Contains(child))
            {
                _toAdd.Remove(child);
            }
            else
            {
                _toRemove.Add(child);
            }
        }

        private void GatherAllHierarchy(Quest parent, ref List<Quest> hierarchy)
        {
            //TODO: find way how to reduce calls count to database.
            LoadChildren(parent);
            List<Quest> children = parent.Children;
            if (children != null && children.Count != 0)
            {
                hierarchy.AddRange(children);
                int length = children.Count;
                for (int i = 0; i < length; ++i)
                {
                    GatherAllHierarchy(children[i], ref hierarchy);
                }
            }
        }

        private static void HandleChild(Quest child)
        {
            if (child.Children == null)
            {
                child.Children = new List<Quest>();
            }
        }

        private static void BindChildToParent(Quest child, Quest parent)
        {
            HandleChild(child);
            child.Parent = parent;
            child.ParentId = parent.Id;        
        }

        private static void BindChildAndParent(Quest child, Quest parent)
        {
            parent.Children.Add(child);
            BindChildToParent(child, parent);
        }

        private static void UnbindChildAndParent(Quest child, Quest parent)
        {
            parent?.Children.Remove(child);
            child.Parent = null;
        }

        private static void BindChildrenToParent(Quest parent)
        {
            foreach (Quest child in parent.Children)
            {
                BindChildToParent(child, parent);
            }
        }

    }
}
