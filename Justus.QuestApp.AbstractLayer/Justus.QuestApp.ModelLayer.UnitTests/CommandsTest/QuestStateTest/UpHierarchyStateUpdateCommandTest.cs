using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest
{
    [TestFixture]
    class UpHierarchyStateUpdateCommandTest
    {
        [Test]
        public void ExecuteStandaloneQuestTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateQuest(State.Progress);
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).
                Repeat.Once();
            repository.Expect(r => r.Root).
                Repeat.Twice().
                Return(root);

            ICommand command = new UpHierarchyQuestCommand(parent, State.Done,  repository);

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
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).
                Repeat.Once();
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).
                Repeat.Once();
            repository.Expect(r => r.Root).
                Repeat.Twice().
                Return(root);

            ICommand command = new UpHierarchyQuestCommand(parent,State.Done,  repository);

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
            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).
                Repeat.Times(3);
            repository.Expect(r => r.Root).
                Return(root).
                Repeat.Times(4);

            Quest current = parent;
            while (current.Children.Count != 0)
            {
                current.Children[0].State = State.Progress;
                current = current.Children[0];
            }

            ICommand command = new UpHierarchyQuestCommand(current, State.Done, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Done, current.State);
            Assert.AreEqual(State.Done, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Done));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoAllParentHierarchyTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).
                Repeat.Times(3);
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).
                Repeat.Times(3);
            repository.Expect(r => r.Root).
                Return(root).
                Repeat.Times(4);

            Quest current = parent;
            while (current.Children.Count != 0)
            {
                current.Children[0].State = State.Progress;
                current = current.Children[0];
            }

            ICommand command = new UpHierarchyQuestCommand(current, State.Done, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(State.Progress, current.State);
            Assert.AreEqual(State.Progress, parent.State);
            Assert.False(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Done));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteOnlyChildTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).
                Repeat.Times(1);
            repository.Expect(r => r.Root).
                Return(root).
                Repeat.Times(2);

            Quest current = parent;
            current.Children[0].Children[1].State = State.Progress;
            while (current.Children.Count != 0)
            {
                current.Children[0].State = State.Progress;
                current = current.Children[0];
            }

            ICommand command = new UpHierarchyQuestCommand(current, State.Done, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Done, current.State);
            Assert.AreEqual(State.Progress, parent.State);
            Assert.IsFalse(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Done));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoOnlyChildTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;
            parent.Parent = root;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).
                Repeat.Times(1);
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).
                Repeat.Times(1);
            repository.Expect(r => r.Root).
                Return(root).
                Repeat.Times(2);

            Quest current = parent;
            current.Children[0].Children[1].State = State.Progress;
            while (current.Children.Count != 0)
            {
                current.Children[0].State = State.Progress;
                current = current.Children[0];
            }

            ICommand command = new UpHierarchyQuestCommand(current, State.Done, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(State.Progress, current.State);
            Assert.AreEqual(State.Progress, parent.State);
            Assert.IsFalse(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Done));

            repository.VerifyAllExpectations();
        }
    }
}
