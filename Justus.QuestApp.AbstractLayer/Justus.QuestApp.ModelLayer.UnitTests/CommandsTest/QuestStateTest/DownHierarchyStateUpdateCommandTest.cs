using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
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
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, QuestState.Progress);
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(null)).IgnoreArguments().Repeat.Times(7);

            Command command = new DownHierarchyStateUpdateCommand(parent,QuestState.Idle, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(QuestState.Idle, parent.CurrentState);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.CurrentState == QuestState.Idle));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoRevertChangesDownHierarchyTest()
        {
            //Arrange
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, QuestState.Progress);
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(null)).IgnoreArguments().Repeat.Times(7);
            repository.Expect(r => r.RevertUpdate(null)).IgnoreArguments().Return(true).Repeat.Times(7);

            Command command = new DownHierarchyStateUpdateCommand(parent, QuestState.Idle, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(QuestState.Progress, parent.CurrentState);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.CurrentState == QuestState.Progress));

            repository.VerifyAllExpectations();
        }
    }
}
