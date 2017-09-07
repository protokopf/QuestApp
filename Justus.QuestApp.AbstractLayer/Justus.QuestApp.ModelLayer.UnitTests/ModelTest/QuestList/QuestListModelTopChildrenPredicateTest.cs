using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Model.QuestList;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest.QuestList
{
    [TestFixture]
    class QuestListModelTopChildrenPredicateTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestTree questTree = MockRepository.GenerateStrictMock<IQuestTree>();
            Func<Quest, bool> predicate = quest => true;

            //Act
            ArgumentNullException treeEx = Assert.Throws<ArgumentNullException>(() => new QuestListModelTopChildrenPredicate(null, predicate));
            ArgumentNullException predicateEx
                = Assert.Throws<ArgumentNullException>(() => new QuestListModelTopChildrenPredicate(questTree, null));

            //Assert
            Assert.IsNotNull(treeEx);
            Assert.IsNotNull(predicateEx);

            Assert.AreEqual("questTree", treeEx.ParamName);
            Assert.AreEqual("topChildrenPredicate", predicateEx.ParamName);
        }

        [Test]
        public void InitializeWithInitializedQuestTreeTest()
        {
            //Arrange
            List<Quest> children = new List<Quest>()
            {
                new FakeQuest() {State = State.Done},
                new FakeQuest() {State = State.Done},
                new FakeQuest() {State = State.Failed},
                new FakeQuest() {State = State.Idle}
            };
            Quest parent = new FakeQuest() {Children = children};

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Twice().
                Return(parent);

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => q.State == State.Idle || q.State == State.Done);

            //Act
            bool isInitializedBeforeInitialization = list.IsInitialized();
            list.Initialize();
            bool isInitializedAfterInitialization = list.IsInitialized();

            //Assert
            Assert.IsFalse(isInitializedBeforeInitialization);
            Assert.IsTrue(isInitializedAfterInitialization);

            Quest listParent = list.Parent;
            Assert.AreEqual(parent, listParent);
            List<Quest> listChildren = list.Leaves;
            Assert.IsNotNull(listChildren);
            Assert.IsNotEmpty(listChildren);
            Assert.AreEqual(3, listChildren.Count);
            Assert.IsTrue(listChildren.All(c => c.State == State.Done || c.State == State.Idle));

            tree.VerifyAllExpectations();
        }

        [Test]
        public void InitializeWithInitializedQuestTreeWithoutChildrenTest()
        {
            //Arrange
            Quest parent = new FakeQuest { Children = null };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(false);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Once();
            tree.Expect(tr => tr.Root).
                Repeat.Once().
                Return(parent);
            tree.Expect(tr => tr.LoadChildren(Arg<Quest>.Is.Equal(parent))).
                Repeat.Once();

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => q.State == State.Idle || q.State == State.Done);

            //Act
            bool isInitializedBeforeInitialization = list.IsInitialized();
            list.Initialize();
            bool isInitializedAfterInitialization = list.IsInitialized();

            //Assert
            Assert.IsFalse(isInitializedBeforeInitialization);
            Assert.IsTrue(isInitializedAfterInitialization);

            Quest listParent = list.Parent;
            Assert.AreEqual(parent, listParent);
            List<Quest> listChildren = list.Leaves;
            Assert.IsNull(listChildren);

            tree.VerifyAllExpectations();
        }

        [Test]
        public void InitializeWithNotInitializedQuestTreeTest()
        {
            //Arrange
            List<Quest> children = new List<Quest>()
            {
                new FakeQuest() {State = State.Done},
                new FakeQuest() {State = State.Done},
                new FakeQuest() {State = State.Failed},
                new FakeQuest() {State = State.Idle}
            };
            Quest parent = new FakeQuest() { Children = children };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(false);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Once();
            tree.Expect(tr => tr.Root).
                Repeat.Twice().
                Return(parent);

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => q.State == State.Idle || q.State == State.Done);

            //Act
            bool isInitializedBeforeInitialization = list.IsInitialized();
            list.Initialize();
            bool isInitializedAfterInitialization = list.IsInitialized();

            //Assert
            Assert.IsFalse(isInitializedBeforeInitialization);
            Assert.IsTrue(isInitializedAfterInitialization);

            Quest listParent = list.Parent;
            Assert.AreEqual(parent, listParent);
            List<Quest> listChildren = list.Leaves;
            Assert.IsNotNull(listChildren);
            Assert.IsNotEmpty(listChildren);
            Assert.AreEqual(3, listChildren.Count);
            Assert.IsTrue(listChildren.All(c => c.State == State.Done || c.State == State.Idle));

            tree.VerifyAllExpectations();
        }

        [Test]
        public void RefreshAfterInitializeTest()
        {
            //Arrange
            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest()
            };

            Quest parent = new FakeQuest { Children = children };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Times(3).
                Return(parent);

            IQuestOrderStrategy strategy = MockRepository.GenerateStrictMock<IQuestOrderStrategy>();
            strategy.Expect(str => str.Order(Arg<IEnumerable<Quest>>.Is.Anything)).
                Repeat.Twice().
                Return(children.OrderBy(q => q.IsImportant));

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => true)
            {
                OrderStrategy = strategy
            };

            //Act
            list.Initialize();
            list.Refresh();

            //Assert
            tree.VerifyAllExpectations();
            strategy.VerifyAllExpectations();
        }

        [TestCase(-1)]
        [TestCase(4)]
        public void TraverseToLeafOutOfBoundary(int position)
        {
            //Arrange
            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest()
            };
            Quest parent = new FakeQuest() { Children = children };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Twice().
                Return(parent);

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => true);

            //Act
            list.Initialize();
            bool traverseResult = list.TraverseToLeaf(position);

            //Assert
            Assert.IsFalse(traverseResult);
            tree.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToLeafWithoutOrderStrategy()
        {
            //Arrange
            int position = 0;

            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest()
            };

            Quest childToTraverse = children[position];
            List<Quest> childChildren = new List<Quest>();

            Quest parent = new FakeQuest { Children = children };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Times(3).
                Return(parent);
            tree.Expect(tr => tr.LoadChildren(Arg<Quest>.Is.Equal(childToTraverse))).
                Repeat.Once().
                Do(new Action<Quest>((q) =>
                {
                    q.Children = childChildren;
                }));

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => true);

            //Act
            list.Initialize();
            bool traverseResult = list.TraverseToLeaf(position);
            Quest listParent = list.Parent;
            List<Quest> listChildren = list.Leaves;

            //Assert
            Assert.AreEqual(childToTraverse, listParent);
            Assert.AreEqual(childChildren, listChildren);

            Assert.IsTrue(traverseResult);
            tree.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToLeafWithOrderStrategy()
        {
            //Arrange
            int position = 0;
            Func<Quest, bool> predicate = q => true;
            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest()
            };

            Quest childToTraverse = children[position];
            List<Quest> childChildren = new List<Quest>();

            Quest parent = new FakeQuest { Children = children };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Times(3).
                Return(parent);
            tree.Expect(tr => tr.LoadChildren(Arg<Quest>.Is.Equal(childToTraverse))).
                Repeat.Once().
                Do(new Action<Quest>((q) =>
                {
                    q.Children = childChildren;
                }));

            IQuestOrderStrategy strategy = MockRepository.GenerateStrictMock<IQuestOrderStrategy>();
            strategy.Expect(str => str.Order(Arg<IEnumerable<Quest>>.Is.Anything))
                .Repeat.Once()
                .Return(children.OrderBy(q => q.IsImportant));
            strategy.Expect(str => str.Order(Arg<IEnumerable<Quest>>.Is.Anything))
                .Repeat.Once()
                .Return(childChildren.OrderBy(q => q.IsImportant));

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, predicate)
            {
                OrderStrategy = strategy
            };

            //Act
            list.Initialize();
            bool traverseResult = list.TraverseToLeaf(position);

            Quest listParent = list.Parent;
            List<Quest> listChildren = list.Leaves;

            //Assert
            Assert.AreEqual(childToTraverse, listParent);
            Assert.AreEqual(childChildren, listChildren);

            Assert.IsTrue(traverseResult);

            tree.VerifyAllExpectations();
            strategy.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToParentInTopRootAlreadyTest()
        {
            //Arrange
            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest()
            };

            Quest parent = new FakeQuest { Children = children };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Times(3).
                Return(parent);

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => true);

            //Act
            list.Initialize();
            bool traverseResult = list.TraverseToParent();
            //Assert

            Assert.IsFalse(traverseResult);
            tree.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToParentTest()
        {
            //Arrange
            int position = 0;

            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
            };

            Quest parent = new FakeQuest { Children = children };


            Quest childToTraverse = children[position];
            childToTraverse.Parent = parent;

           
            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Times(4).
                Return(parent);
            tree.Expect(tr => tr.LoadChildren(Arg<Quest>.Is.Equal(childToTraverse))).
                Repeat.Once();
            tree.Expect(tr => tr.UnloadChildren(Arg<Quest>.Is.Equal(childToTraverse))).
                Repeat.Once();

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => true);

            //Act
            list.Initialize();
            bool traverseResult = list.TraverseToLeaf(position);
            bool traverseToParentResult = list.TraverseToParent();

            Quest listParent = list.Parent;
            List<Quest> listChildren = list.Leaves;

            //Assert
            Assert.AreEqual(parent, listParent);
            Assert.AreEqual(children, listChildren);

            Assert.IsTrue(traverseResult);
            Assert.IsTrue(traverseToParentResult);

            tree.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToRootInTheRootAlreadyTest()
        {
            //Arrange
            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest(),
                new FakeQuest()
            };

            Quest parent = new FakeQuest { Children = children };

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Times(3).
                Return(parent);

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => true);

            //Act
            list.Initialize();
            bool traverseResult = list.TraverseToRoot();
            //Assert

            Assert.IsFalse(traverseResult);
            tree.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToRoot()
        {
            //Arrange
            int position = 0;

            List<Quest> children = new List<Quest>()
            {
                new FakeQuest(),
            };

            Quest parent = new FakeQuest { Children = children };


            Quest childToTraverse = children[position];
            childToTraverse.Parent = parent;


            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.IsInitialized()).
                Repeat.Once().
                Return(true);
            tree.Expect(tr => tr.Initialize()).
                Repeat.Never();
            tree.Expect(tr => tr.Root).
                Repeat.Times(7).
                Return(parent);
            tree.Expect(tr => tr.LoadChildren(Arg<Quest>.Is.Equal(childToTraverse))).
                Repeat.Once();
            tree.Expect(tr => tr.UnloadChildren(Arg<Quest>.Is.Equal(childToTraverse))).
                Repeat.Once();

            QuestListModelTopChildrenPredicate list = new QuestListModelTopChildrenPredicate(tree, q => true);

            //Act
            list.Initialize();
            bool traverseResult = list.TraverseToLeaf(position);
            bool traverseToRootResult = list.TraverseToRoot();

            Quest listParent = list.Parent;
            List<Quest> listChildren = list.Leaves;

            //Assert
            Assert.AreEqual(parent, listParent);
            Assert.AreEqual(children, listChildren);

            Assert.IsTrue(traverseResult);
            Assert.IsTrue(traverseToRootResult);

            tree.VerifyAllExpectations();
        }
    }
}
