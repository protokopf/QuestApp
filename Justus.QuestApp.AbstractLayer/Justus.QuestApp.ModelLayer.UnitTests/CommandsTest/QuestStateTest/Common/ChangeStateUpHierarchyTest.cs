using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.Commands.State.Common;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest.Common
{
    [TestFixture]
    class ChangeStateUpHierarchyTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(ChangeStateUpHierarchy).IsSubclassOf(typeof(UpHierarchyQuestCommand)));
        }

        [Test]
        public void ExecuteStandaloneQuestTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateQuest(State.Progress);
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Root).
                Repeat.Twice().
                Return(root);

            ICommand command = new ChangeStateUpHierarchy(parent, repository, State.Done);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Done, parent.State);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoStandaloneQuestTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateQuest(State.Progress);
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Root).
                Repeat.Times(4).
                Return(root);

            ICommand command = new ChangeStateUpHierarchy(parent, repository, State.Done);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(State.Progress, parent.State);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteAllParentHierarchyTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Idle);
            parent.Parent = root;

            parent = parent.Children[0].Children[0];

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            repository.Expect(r => r.Root).
                Return(root).
                Repeat.Times(4);


            ICommand command = new ChangeStateUpHierarchy(parent, repository, State.Done);

            //Act
            command.Execute();

            //Assert
            QuestHelper.ExecuteForUpHierarchy(
                quest: parent, 
                action: q => Assert.AreEqual(State.Done, q.State), 
                stopCondition:q => q == root);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoAllParentHierarchyTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Idle);
            parent.Parent = root;

            parent = parent.Children[0].Children[0];

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            repository.Expect(r => r.Root).
                Return(root).
                Repeat.Times(8);


            ICommand command = new ChangeStateUpHierarchy(parent, repository, State.Done);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            QuestHelper.ExecuteForUpHierarchy(parent, q => Assert.AreEqual(State.Idle, q.State), q => q == root);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateQuest(State.Progress);
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Root).
                Repeat.Twice().
                Return(root);

            ICommand command = new ChangeStateUpHierarchy(parent, repository, State.Done);

            //Act
            command.Execute();
            command.Commit();

            //Assert
            Assert.AreEqual(State.Done, parent.State);

            repository.VerifyAllExpectations();
        }
    }
}
