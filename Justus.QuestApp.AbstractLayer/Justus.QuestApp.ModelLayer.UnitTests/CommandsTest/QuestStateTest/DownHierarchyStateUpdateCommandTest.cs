using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest
{
    [TestFixture]
    class DownHierarchyStateUpdateCommandTest
    {
        [Test]
        public void ExecuteChangesDownHierarchyTest()
        {
            //Arrange
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, State.Progress);
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.NotNull)).Repeat.Times(7);

            ICommand command = new DownHierarchyQuestCommand(parent,State.Idle, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Idle, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Idle));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoRevertChangesDownHierarchyTest()
        {
            //Arrange
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, State.Progress);
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.NotNull)).Repeat.Times(7);
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.NotNull)).Repeat.Times(7);

            ICommand command = new DownHierarchyQuestCommand(parent, State.Idle, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(State.Progress, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Progress));

            repository.VerifyAllExpectations();
        }
    }
}
