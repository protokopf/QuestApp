using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.StateTest
{
    [TestFixture]
    class UpHierarchyStateUpdateCommandTest
    {
        [Test]
        public void ExecuteStandaloneQuestTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Once();

            Quest parent = QuestHelper.CreateQuest(State.Progress);

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
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Once();
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).Repeat.Once();

            Quest parent = QuestHelper.CreateQuest(State.Progress);

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
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(3);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;

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
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(3);
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).Repeat.Times(3);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;

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
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(1);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;

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
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(1);
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).Repeat.Times(1);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, State.Done);
            parent.State = State.Progress;

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
