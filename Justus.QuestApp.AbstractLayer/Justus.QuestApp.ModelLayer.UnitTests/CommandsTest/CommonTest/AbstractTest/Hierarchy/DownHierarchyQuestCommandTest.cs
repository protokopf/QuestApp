using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Classic.Common.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class DownHierarchyQuestCommandTest
    {
        private class MockDownHierarchyQuestCommand : DownHierarchyQuestCommand
        {
            public MockDownHierarchyQuestCommand(Quest quest) : base(quest)
            {
            }

            protected override bool InnerCommit()
            {
                throw new NotImplementedException();
            }

            protected override void ExecuteOnQuest(Quest quest)
            {
                OnExecuteOnQuest?.Invoke(quest);
            }

            protected override void UndoOnQuest(Quest quest)
            {
                OnUndoOnQuest?.Invoke(quest);
            }

            protected override void ExecuteOnQuestAfterTraverse(Quest quest)
            {
                OnExecuteOnQuestAfterTraverse?.Invoke(quest);
            }

            protected override void UndoOnQuestAfterTraverse(Quest quest)
            {
                OnUndoOnQuestAfterTraverse?.Invoke(quest);
            }

            public event Action<Quest> OnExecuteOnQuest;
            public event Action<Quest> OnUndoOnQuest;
            public event Action<Quest> OnExecuteOnQuestAfterTraverse;
            public event Action<Quest> OnUndoOnQuestAfterTraverse;
        }

        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(DownHierarchyQuestCommand).IsSubclassOf(typeof(AbstractQuestCommand)));
        }

        [Test]
        public void ExecuteOnQuestWithoutChildrenTest()
        {
            //Arrange
            Quest quest = new FakeQuest();

            MockDownHierarchyQuestCommand command = new MockDownHierarchyQuestCommand(quest);
            command.OnExecuteOnQuest += (q) =>
            {
                Assert.AreEqual(quest, q);
            };

            //Act
            command.Execute();

            //Assert
        }

        [Test]
        public void ExecuteOnQuestWithChildrenTest()
        {
            //Arrange
            Quest child = new FakeQuest();
            Quest quest = new FakeQuest()
            {
                Children = new List<Quest>() {child, null}
            };

            int executedOnParentCount = 0;
            int executedOnChildCount = 0;

            int executedAfterTraverseParentCount = 0;
            int executedAfterTraverseChildCount = 0;

            MockDownHierarchyQuestCommand command = new MockDownHierarchyQuestCommand(quest);
            command.OnExecuteOnQuest += (q) =>
            {
                if (q == quest)
                {
                    executedOnParentCount++;
                }
                if (q == child)
                {
                    executedOnChildCount++;
                }
            };
            command.OnExecuteOnQuestAfterTraverse += (q) =>
            {
                if (q == quest)
                {
                    executedAfterTraverseParentCount++;
                }
                if (q == child)
                {
                    executedAfterTraverseChildCount++;
                }
            };

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(1, executedOnParentCount);
            Assert.AreEqual(1, executedOnChildCount);

            Assert.AreEqual(1, executedAfterTraverseParentCount);
            Assert.AreEqual(0, executedAfterTraverseChildCount);
        }

        [Test]
        public void UndoOnQuestWithoutChildrenTest()
        {
            //Arrange
            Quest quest = new FakeQuest();

            MockDownHierarchyQuestCommand command = new MockDownHierarchyQuestCommand(quest);
            command.OnUndoOnQuest += (q) =>
            {
                Assert.AreEqual(quest, q);
            };

            //Act
            command.Execute();
            command.Undo();

            //Assert
        }

        [Test]
        public void UndoOnQuestWithChildrenTest()
        {
            //Arrange
            Quest child = new FakeQuest();
            Quest quest = new FakeQuest()
            {
                Children = new List<Quest>() { child, null }
            };

            int undoOnParentCount = 0;
            int undoOnChildCount = 0;

            int undoAfterTraverseParentCount = 0;
            int undoAfterTraverseChildCount = 0;

            MockDownHierarchyQuestCommand command = new MockDownHierarchyQuestCommand(quest);
            command.OnUndoOnQuest += (q) =>
            {
                if (q == quest)
                {
                    undoOnParentCount++;
                }
                if (q == child)
                {
                    undoOnChildCount++;
                }
            };
            command.OnUndoOnQuestAfterTraverse += (q) =>
            {
                if (q == quest)
                {
                    undoAfterTraverseParentCount++;
                }
                if (q == child)
                {
                    undoAfterTraverseChildCount++;
                }
            };

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(1, undoOnParentCount);
            Assert.AreEqual(1, undoOnChildCount);

            Assert.AreEqual(1, undoAfterTraverseParentCount);
            Assert.AreEqual(0, undoAfterTraverseChildCount);
        }
    }
}
