using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Classic.Common.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class UpHierarchyQuestCommandTest
    {
        private class MockUpHierarchyQuestCommand : UpHierarchyQuestCommand
        {
            public MockUpHierarchyQuestCommand(Quest quest) : base(quest)
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

            protected override bool ShouldStopTraversing(Quest quest)
            {
                return quest.Parent == null;
            }

            public event Action<Quest> OnExecuteOnQuest;

            public event Action<Quest> OnUndoOnQuest;
        }

        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(UpHierarchyQuestCommand).IsSubclassOf(typeof(AbstractQuestCommand)));
        }

        [Test]
        public void ExecuteWithParentsTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent0 = new FakeQuest();
            Quest parent1 = new FakeQuest();
            Quest parent2 = new FakeQuest();
            root.Parent = null;
            parent0.Parent = root;
            parent1.Parent = parent0;
            parent2.Parent = parent1;

            bool hasExecutedOnRoot = false;
            bool hasExecutedOnParent0 = false;
            bool hasExecutedOnParent1 = false;
            bool hasExecutedOnParent2 = false;


            MockUpHierarchyQuestCommand command = new MockUpHierarchyQuestCommand(parent2);
            command.OnExecuteOnQuest += (q) =>
            {
                if (q == root)
                {
                    hasExecutedOnRoot = true;
                }
                else if (q == parent0)
                {
                    hasExecutedOnParent0 = true;
                }
                else if (q == parent1)
                {
                    hasExecutedOnParent1 = true;
                }
                else if (q == parent2)
                {
                    hasExecutedOnParent2 = true;
                }
            };

            //Act
            command.Execute();

            //Assert
            Assert.IsFalse(hasExecutedOnRoot);
            Assert.IsTrue(hasExecutedOnParent0);
            Assert.IsTrue(hasExecutedOnParent1);
            Assert.IsTrue(hasExecutedOnParent2);
        }

        [Test]
        public void UndoWithParentsTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent0 = new FakeQuest();
            Quest parent1 = new FakeQuest();
            Quest parent2 = new FakeQuest();

            root.Parent = null;
            parent0.Parent = root;
            parent1.Parent = parent0;
            parent2.Parent = parent1;

            bool hasUndoOnRoot = false;
            bool hasUndoOnParent0 = false;
            bool hasUndoOnParent1 = false;
            bool hasUndoOnParent2 = false;

            MockUpHierarchyQuestCommand command = new MockUpHierarchyQuestCommand(parent2);
            command.OnUndoOnQuest += (q) =>
            {
                if (q == root)
                {
                    hasUndoOnRoot = true;
                }
                else if (q == parent0)
                {
                    hasUndoOnParent0 = true;
                }
                else if (q == parent1)
                {
                    hasUndoOnParent1 = true;
                }
                else if (q == parent2)
                {
                    hasUndoOnParent2 = true;
                }
            };

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.IsFalse(hasUndoOnRoot);
            Assert.IsTrue(hasUndoOnParent0);
            Assert.IsTrue(hasUndoOnParent1);
            Assert.IsTrue(hasUndoOnParent2);
        }
    }
}
