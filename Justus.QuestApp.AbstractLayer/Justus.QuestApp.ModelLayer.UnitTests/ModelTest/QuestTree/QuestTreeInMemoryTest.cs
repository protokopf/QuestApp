using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.ModelLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest.QuestTree
{
    [TestFixture]
    class QuestTreeInMemoryTest
    {
        private const int TopRootId = 1;

        [Test]
        public void InitializeNoSavedRootTest()
        {
            //Arrange
            Quest createdRoot = new FakeQuest();

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();
            factory.Expect(cr => cr.CreateQuest()).
                Repeat.Once().
                Return(createdRoot);

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Insert(Arg<Quest>.Is.Anything)).
                Repeat.Once();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            bool isInitialized = tree.IsInitialized();
            Quest root = tree.Root;

            //Assert
            Assert.IsTrue(isInitialized);
            Assert.IsNotNull(root);

            Assert.AreEqual(createdRoot, root);
            Assert.AreEqual(null, root.ParentId);

            Quest savedRoot =
                dataLayer.GetArgumentsForCallsMadeOn(dl => dl.Insert(Arg<Quest>.Is.Anything))[0][0] as Quest;

            Assert.IsNotNull(savedRoot);

            Assert.AreEqual(root,savedRoot);
            Assert.AreEqual(TopRootId,savedRoot.Id);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void InitializeCalledTwiceButShouldBeInitializedOnceTest()
        {
            //Arrange
            Quest createdRoot = new FakeQuest();

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();
            factory.Expect(cr => cr.CreateQuest()).
                Repeat.Once().
                Return(createdRoot);

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Insert(Arg<Quest>.Is.Anything)).
                Repeat.Once();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.Initialize();
            Quest root = tree.Root;

            //Assert
            Assert.IsNotNull(root);

            Assert.AreEqual(createdRoot, root);

            Quest savedRoot =
                dataLayer.GetArgumentsForCallsMadeOn(dl => dl.Insert(Arg<Quest>.Is.Anything))[0][0] as Quest;

            Assert.IsNotNull(savedRoot);

            Assert.AreEqual(root, savedRoot);
            Assert.AreEqual(TopRootId, savedRoot.Id);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void InitializeWithSavedRootNoChildrenTest()
        {
            //Arrange
            Quest savedRoot = new FakeQuest()
            {
                Id = TopRootId
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(null);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            Quest root = tree.Root;

            //Assert
            Assert.IsNotNull(root);
            Assert.AreEqual(savedRoot, root);
            Assert.IsEmpty(root.Children);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void InitializeWithSavedRootWithChildrenTest()
        {
            //Arrange
            Quest savedRoot = new FakeQuest()
            {
                Id = TopRootId
            };

            List<Quest> children = new List<Quest>()
            {
                new FakeQuest()
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(children);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            Quest root = tree.Root;

            //Assert
            Assert.IsNotNull(root);
            Assert.AreEqual(savedRoot, root);
            Assert.IsNotEmpty(root.Children);
            Assert.AreEqual(children, root.Children);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void InitializeBindRootAndChildren()
        {
            //Arrange
            Quest savedRoot = new FakeQuest()
            {
                Id = TopRootId
            };

            List<Quest> children = new List<Quest>()
            {
                new FakeQuest()
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(children);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            Quest root = tree.Root;

            //Assert
            Assert.AreEqual(1,root.Children.Count);
            Quest child = root.Children[0];

            Assert.IsTrue(root.Children.Any(q => q == child));
            Assert.AreEqual(root, child.Parent);
            Assert.AreEqual(root.Id, child.ParentId);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void GetTestNulPredicate()
        {
            //Arrange
            string title = "title";

            Quest savedRoot = new FakeQuest()
            {
                Title = title,
                Id = TopRootId
                
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(null);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            ArgumentNullException ane = Assert.Throws<ArgumentNullException>(() => tree.Get(null));

            //Assert
            Assert.IsNotNull(ane);
            Assert.AreEqual("questPredicate", ane.ParamName);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void GetTest()
        {
            //Arrange
            string title = "title";

            Quest savedRoot = new FakeQuest()
            {
                Id = TopRootId,
                Title = title
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(null);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            Quest foundQuest = tree.Get(q => q.Title == title);

            //Assert
            Assert.IsNotNull(foundQuest);
            Assert.AreEqual(savedRoot, foundQuest);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void LoadChildrenNullQuestTest()
        {
            //Arrange
            Quest savedRoot = new FakeQuest() {Id = TopRootId };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(null);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            ArgumentNullException ane = Assert.Throws<ArgumentNullException>(() => tree.LoadChildren(null));

            //Assert
            Assert.IsNotNull(ane);
            Assert.AreEqual("quest", ane.ParamName);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void LoadChildrenTest()
        {
            //Arrange
            Quest savedRoot = new FakeQuest() {Id = TopRootId};

            List<Quest> firstLevel = new List<Quest>()
            {
                new FakeQuest() {Id = 2}
            };

            List<Quest> secondLevel = new List<Quest>()
            {
                new FakeQuest() {Id = 3}
            };
            
            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(2);

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(2).
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(2);

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(firstLevel);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(2))).
                Repeat.Once().
                Return(secondLevel);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            Quest fromFirstLevel = firstLevel[0];

            //Act
            tree.Initialize();
            tree.LoadChildren(fromFirstLevel);
            Quest fromSecondLevel = tree.Get(q => q.Id == secondLevel[0].Id);

            //Assert
            Assert.IsNotNull(fromFirstLevel.Children);
            Assert.IsNotEmpty(fromFirstLevel.Children);
            Assert.AreEqual(secondLevel, fromFirstLevel.Children);

            Assert.IsNotNull(fromSecondLevel);


            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void LoadChildrenBindParentAndChildrenTest()
        {
            //Arrange
            Quest savedRoot = new FakeQuest(){Id = TopRootId};

            List<Quest> firstLevel = new List<Quest>()
            {
                new FakeQuest() {Id = 2}
            };

            List<Quest> secondLevel = new List<Quest>()
            {
                new FakeQuest() {Id = 3}
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(2);

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(2).
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(2);

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(firstLevel);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(2))).
                Repeat.Once().
                Return(secondLevel);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            Quest rootChild = tree.Get(q => q.Parent == tree.Root);
            tree.LoadChildren(rootChild);


            //Assert
            Assert.AreEqual(1, rootChild.Children.Count);
            Quest childOfrootChild = rootChild.Children[0];

            Assert.AreEqual(secondLevel[0], childOfrootChild);
            Assert.AreEqual(rootChild, childOfrootChild.Parent);
            Assert.AreEqual(rootChild.Id, childOfrootChild.ParentId);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void UnloadRootChildrenTest()
        {
            //Arrange
            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = 1}
            };

            Quest rootChild = rootChildren[0];

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            bool isChildInTree = tree.Get(q => q == rootChild) != null;
            bool isChildInChildren = tree.Root.Children.Find(q => q == rootChild) != null;

            tree.UnloadChildren(tree.Root);

            bool isChildInTreeAfterUnload = tree.Get(q => q == rootChild) != null;
            bool isChildInChildrenAfterUnload = tree.Root.Children.Find(q => q == rootChild) != null;

            //Assert
            Assert.IsTrue(isChildInTree);
            Assert.IsTrue(isChildInChildren);

            Assert.IsFalse(isChildInTreeAfterUnload);
            Assert.IsFalse(isChildInChildrenAfterUnload);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void AddChildToRootWithoutSavingTest()
        {
            //Arrange
            int addedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = 1}
            };

            Quest toAdd = new FakeQuest()
            {
                Id = addedId,
                Children = null
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);

            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.AddChild(tree.Root, toAdd);
            Quest root = tree.Root;

            //Assert
            Quest added = root.Children.Find(q => q.Id == addedId);
            Assert.IsNotNull(added);
            Assert.IsNotNull(added.Children);
            Assert.IsEmpty(added.Children);
            Assert.AreEqual(toAdd, added);
            Assert.AreEqual(root.Id, added.ParentId);
            Assert.AreEqual(root, added.Parent);

            Quest addedFromTree = tree.Get(q => q.Id == addedId);
            Assert.IsNotNull(addedFromTree);
            Assert.AreEqual(added,addedFromTree);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void AddChildToRootAndSaveTest()
        {
            //Arrange
            int addedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = 2}
            };

            Quest toAdd = new FakeQuest()
            {
                Id = addedId
            };

            List<Quest> insertedQuests = new List<Quest>();

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.InsertAll(Arg<IEnumerable<Quest>>.Is.Anything)).Do(new Action<IEnumerable<Quest>>(
                    inserted =>
                    {
                        insertedQuests.AddRange(inserted);
                    })).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.AddChild(tree.Root, toAdd);
            tree.Save();

            Quest root = tree.Root;

            //Assert
            Quest added = root.Children.Find(q => q.Id == addedId);
            Assert.IsNotNull(added);
            Assert.AreEqual(toAdd, added);
            Assert.AreEqual(root.Id, added.ParentId);
            Assert.AreEqual(root, added.Parent);

            Quest addedFromTree = tree.Get(q => q.Id == addedId);
            Assert.IsNotNull(addedFromTree);
            Assert.AreEqual(added, addedFromTree);

            Assert.IsNotEmpty(insertedQuests);
            Assert.IsTrue(insertedQuests.Contains(toAdd));

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void RemoveChildFromRootWithoutSavingTest()
        {
            //Arrange
            int removedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = removedId}
            };

            Quest toRemove = rootChildren[0];

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.Open()).
                 Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(removedId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.RemoveChild(tree.Root, toRemove);
            Quest root = tree.Root;

            //Assert
            Quest removedInRootChildren = root.Children.Find(q => q == toRemove);
            Assert.IsNull(removedInRootChildren);
            Assert.AreEqual(TopRootId, toRemove.ParentId);
            Assert.AreEqual(null, toRemove.Parent);

            Quest removedInTree = tree.Get(q => q == toRemove);
            Assert.IsNull(removedInTree);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void RemoveRecursiveChildFromRootThenSavingTest()
        {
            //Arrange
            int removedId = 42;
            int childOfRemovedId = 43;
            int childOfChildOfRemovedId = 44;

            Quest savedRoot = new FakeQuest() { Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest
                {
                    Id = removedId
                }
            };

            Quest toRemove = rootChildren[0];
            Quest childOfToRemove = new FakeQuest
            {
                Id = childOfRemovedId,
            };
            Quest childOfChildToRemove = new FakeQuest
            {
                Id = childOfChildOfRemovedId
            };
            childOfToRemove.ParentId = removedId;
            childOfChildToRemove.ParentId = childOfRemovedId;

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            #region  Mocks logic that loads all child hierarchy before deleting any quest

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(removedId))).
                Repeat.Once().
                Return(new List<Quest>() { childOfToRemove });
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(childOfRemovedId))).
                Repeat.Once().
                Return(new List<Quest>(){childOfChildToRemove});
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(childOfChildOfRemovedId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

        #endregion

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Delete(Arg<int>.Is.Equal(removedId))).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Delete(Arg<int>.Is.Equal(childOfRemovedId))).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Delete(Arg<int>.Is.Equal(childOfChildOfRemovedId))).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.RemoveChild(tree.Root, toRemove);
            tree.Save();
            Quest root = tree.Root;

            //Assert
            Quest removedInRootChildren = root.Children.Find(q => q == toRemove);
            Assert.IsNull(removedInRootChildren);
            Assert.AreEqual(TopRootId, toRemove.ParentId);
            Assert.AreEqual(null, toRemove.Parent);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void RemoveChildFromRootAndSaveTest()
        {
            //Arrange
            int removedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = removedId}
            };

            Quest toRemove = rootChildren[0];

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(removedId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Delete(Arg<int>.Is.Equal(removedId))).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.RemoveChild(tree.Root, toRemove);
            tree.Save();
            Quest root = tree.Root;

            //Assert
            Quest removedInRootChildren = root.Children.Find(q => q == toRemove);
            Assert.IsNull(removedInRootChildren);
            Assert.AreEqual(TopRootId, toRemove.ParentId);
            Assert.AreEqual(null, toRemove.Parent);

            Quest removedInTree = tree.Get(q => q == toRemove);
            Assert.IsNull(removedInTree);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void AddRemoveChildThenSaveTest()
        {
            //Arrange
            int addedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = 1}
            };

            Quest toAdd = new FakeQuest()
            {
                Id = addedId
            };

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(addedId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.InsertAll(Arg<HashSet<Quest>>.Is.Anything)).
                IgnoreArguments().
                Repeat.Never();
            dataLayer.Expect(dl => dl.Delete(Arg<int>.Is.Anything)).
                IgnoreArguments().
                Repeat.Never();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.AddChild(tree.Root, toAdd);
            tree.RemoveChild(tree.Root, toAdd);
            tree.Save();

            Quest root = tree.Root;

            //Assert
            Quest added = root.Children.Find(q => q.Id == addedId);
            Assert.IsNull(added);
            Assert.AreEqual(TopRootId, toAdd.ParentId);
            Assert.AreEqual(null, toAdd.Parent);

            Quest addedFromTree = tree.Get(q => q.Id == addedId);
            Assert.IsNull(addedFromTree);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void RemoveAddChildThenSaveTest()
        {
            //Arrange
            int removedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = removedId}
            };

            Quest toRemove = rootChildren[0];

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Times(1);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Times(1).
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(removedId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Times(1);

            dataLayer.Expect(dl => dl.InsertAll(Arg<HashSet<Quest>>.Is.Anything)).
                IgnoreArguments().
                Repeat.Never();
            dataLayer.Expect(dl => dl.Delete(Arg<int>.Is.Anything)).
                IgnoreArguments().
                Repeat.Never();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.RemoveChild(tree.Root, toRemove);
            tree.AddChild(tree.Root, toRemove);
            tree.Save();
            Quest root = tree.Root;

            //Assert
            Quest removedInRootChildren = root.Children.Find(q => q == toRemove);
            Assert.IsNotNull(removedInRootChildren);
            Assert.AreEqual(toRemove, removedInRootChildren);
            Assert.AreEqual(tree.Root.Id, removedInRootChildren.ParentId);
            Assert.AreEqual(tree.Root, removedInRootChildren.Parent);

            Quest removedInTree = tree.Get(q => q == toRemove);
            Assert.IsNotNull(removedInTree);
            Assert.AreEqual(toRemove, removedInTree);

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void UpdateQuestInTreeAndSaveTest()
        {
            //Arrange
            int updatedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = updatedId}
            };

            Quest toUpdate = rootChildren[0];

            List<Quest> updatedQuests = new List<Quest>();

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.UpdateAll(Arg<IEnumerable<Quest>>.Is.Anything)).Do(new Action<IEnumerable<Quest>>(
                    updated =>
                    {
                        updatedQuests.AddRange(updated);
                    })).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.Update(toUpdate);
            tree.Save();

            //Assert
            Assert.IsNotEmpty(updatedQuests);
            Assert.IsTrue(updatedQuests.Contains(toUpdate));

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAddedButNotSavedQuestInTreeAndSaveTest()
        {
            //Arrange
            int updatedId = 42;

            Quest savedRoot = new FakeQuest() { Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = updatedId}
            };

            Quest toUpdate = rootChildren[0];

            List<Quest> addedQuests = new List<Quest>();

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.UpdateAll(Arg<IEnumerable<Quest>>.Is.Anything)).
                Repeat.Never();
            dataLayer.Expect(dl => dl.InsertAll(Arg<IEnumerable<Quest>>.Is.Anything)).Do(new Action<IEnumerable<Quest>>(
                    updated =>
                    {
                        addedQuests.AddRange(updated);
                    })).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.AddChild(savedRoot, toUpdate);
            tree.Update(toUpdate);
            tree.Save();

            //Assert
            Assert.IsNotEmpty(addedQuests);
            Assert.IsTrue(addedQuests.Contains(toUpdate));

            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void UpdateRevertUpdateQuestAndSaveTest()
        {
            //Arrange
            int updatedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = updatedId}
            };

            Quest toUpdate = rootChildren[0];

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.UpdateAll(Arg<IEnumerable<Quest>>.Is.Anything)).
                IgnoreArguments().
                Repeat.Never();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.Update(toUpdate);
            tree.RevertUpdate(toUpdate);
            tree.Save();

            //Assert
            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void UpdateQuestNotFromTreeAndSaveTest()
        {
            //Arrange
            int updatedId = 42;

            Quest savedRoot = new FakeQuest(){ Id = TopRootId };

            List<Quest> rootChildren = new List<Quest>()
            {
                new FakeQuest() {Id = updatedId}
            };

            Quest toUpdate = new FakeQuest();

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();

            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(savedRoot);
            dataLayer.Expect(dl => dl.IsClosed()).
                Repeat.Once().
                Return(false);
            dataLayer.Expect(dl => dl.GetAll(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(rootChildren);
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.UpdateAll(Arg<IEnumerable<Quest>>.Is.Anything)).
                Repeat.Never();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.Update(toUpdate);
            tree.Save();

            //Assert
            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        public void SaveWithoutAnyChangesTest()
        {
            //Arrange
            Quest createdRoot = new FakeQuest();

            IQuestFactory factory = MockRepository.GenerateStrictMock<IQuestFactory>();
            factory.Expect(cr => cr.CreateQuest()).
                Repeat.Once().
                Return(createdRoot);

            IQuestDataLayer dataLayer = MockRepository.GenerateStrictMock<IQuestDataLayer>();
            dataLayer.Expect(dl => dl.Open()).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Get(Arg<int>.Is.Equal(TopRootId))).
                Repeat.Once().
                Return(null);
            dataLayer.Expect(dl => dl.Insert(Arg<Quest>.Is.Anything)).
                Repeat.Once();
            dataLayer.Expect(dl => dl.Dispose()).
                Repeat.Once();

            dataLayer.Expect(dl => dl.InsertAll(Arg<IEnumerable<Quest>>.Is.Anything)).
                Repeat.Never();
            dataLayer.Expect(dl => dl.UpdateAll(Arg<IEnumerable<Quest>>.Is.Anything)).
                Repeat.Never();
            dataLayer.Expect(dl => dl.Delete(Arg<int>.Is.Anything)).
                Repeat.Never();

            QuestTreeInMemory tree = new QuestTreeInMemory(dataLayer, factory);

            //Act
            tree.Initialize();
            tree.Save();

            //Assert
            dataLayer.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }
    }
}
