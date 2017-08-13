using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State.Common;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;
using DownHierarchyQuestCommand = Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy.DownHierarchyQuestCommand;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest.Common
{
    [TestFixture]
    class ChangeStateDownHierarchyTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(ChangeStateDownHierarchy).IsSubclassOf(typeof(DownHierarchyQuestCommand)));
        }

        [Test]
        public void ExecuteChangesDownHierarchyTest()
        {
            //Arrange
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, State.Progress);
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.NotNull)).Repeat.Times(7);

            ICommand command = new ChangeStateDownHierarchy(parent, repository, State.Idle);

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

            ICommand command = new ChangeStateDownHierarchy(parent, repository, State.Idle);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(State.Progress, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Progress));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, State.Progress);
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.NotNull)).Repeat.Times(7);
            repository.Expect(r => r.Save()).Repeat.Once();

            ICommand command = new ChangeStateDownHierarchy(parent, repository, State.Idle);

            //Act
            command.Execute();
            command.Commit();

            //Assert
            Assert.AreEqual(State.Idle, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Idle));

            repository.VerifyAllExpectations();
        }
    }
}
