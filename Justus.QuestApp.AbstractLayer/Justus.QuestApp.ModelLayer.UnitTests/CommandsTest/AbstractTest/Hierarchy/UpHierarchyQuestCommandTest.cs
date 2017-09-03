using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class UpHierarchyQuestCommandTest
    {
        private class UpHierarchyQuestCommandMock : UpHierarchyQuestCommand
        {
            public UpHierarchyQuestCommandMock(Quest quest, IQuestCommand questCommand) : base(quest, questCommand)
            {
            }

            protected override bool ShouldStopTraversing(Quest quest)
            {
                return quest == null;
            }
        }

        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(UpHierarchyQuestCommand).IsSubclassOf(typeof(object)));
            Assert.IsTrue(typeof(ICommand).IsAssignableFrom(typeof(UpHierarchyQuestCommand)));
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            Quest targer = QuestHelper.CreateQuest();

            //Act
            ArgumentNullException targetEx = Assert.Throws<ArgumentNullException>(() => new UpHierarchyQuestCommandMock(null, questCommand));
            ArgumentNullException questCommandEx = Assert.Throws<ArgumentNullException>(() => new UpHierarchyQuestCommandMock(targer, null));

            //Assert
            Assert.IsNotNull(targetEx);
            Assert.AreEqual("quest", targetEx.ParamName);

            Assert.IsNotNull(questCommandEx);
            Assert.AreEqual("questCommand", questCommandEx.ParamName);
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

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent2))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent1))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent0))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(root))).Repeat.Once().Return(true);

            UpHierarchyQuestCommandMock command = new UpHierarchyQuestCommandMock(parent2, innerCommand);


            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteWithParentsOneReturnsFalseTest()
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

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent2))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent1))).Repeat.Once().Return(false);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent0))).Repeat.Never();
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(root))).Repeat.Never();

            UpHierarchyQuestCommandMock command = new UpHierarchyQuestCommandMock(parent2, innerCommand);


            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
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

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent2))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent1))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent0))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(root))).Repeat.Once().Return(true);

            UpHierarchyQuestCommandMock command = new UpHierarchyQuestCommandMock(parent2, innerCommand);


            //Act
            bool result = command.Undo();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void UndoWithParentsOneReturnsFalseTest()
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

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent2))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent1))).Repeat.Once().Return(false);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent0))).Repeat.Never();
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(root))).Repeat.Never();

            UpHierarchyQuestCommandMock command = new UpHierarchyQuestCommandMock(parent2, innerCommand);


            //Act
            bool result = command.Undo();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest quest = new FakeQuest();

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            innerCommand.Expect(ic => ic.Commit()).Repeat.Once().Return(true);

            UpHierarchyQuestCommandMock command = new UpHierarchyQuestCommandMock(quest, innerCommand);

            //Act
            bool result = command.Commit();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            Quest quest = new FakeQuest();

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();

            UpHierarchyQuestCommandMock command = new UpHierarchyQuestCommandMock(quest, innerCommand);

            //Act
            bool result = command.IsValid();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
        }
    }
}
