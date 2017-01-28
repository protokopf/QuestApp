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
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest
{
    [TestFixture]
    class DoneUpdateCommandTest
    {
        [Test]
        public void ExecuteStandaloneQuestTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Once();

            Quest parent = QuestHelper.CreateQuest(QuestState.Progress);

            Command command = new DoneUpdateCommand(parent, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(QuestState.Done, parent.CurrentState);
            
            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoStandaloneQuestTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Once();
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).Repeat.Once().Return(true);

            Quest parent = QuestHelper.CreateQuest(QuestState.Progress);

            Command command = new DoneUpdateCommand(parent, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(QuestState.Progress, parent.CurrentState);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteAllParentHierarchyTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(3);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, QuestState.Done);
            parent.CurrentState = QuestState.Progress;

            Quest current = parent;
            while (current.Children.Count != 0)
            {
                current.Children[0].CurrentState = QuestState.Progress;
                current = current.Children[0];
            }

            Command command = new DoneUpdateCommand(current, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(QuestState.Done, current.CurrentState);
            Assert.AreEqual(QuestState.Done, parent.CurrentState);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.CurrentState == QuestState.Done));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoAllParentHierarchyTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(3);
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).Return(true).Repeat.Times(3);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, QuestState.Done);
            parent.CurrentState = QuestState.Progress;

            Quest current = parent;
            while (current.Children.Count != 0)
            {
                current.Children[0].CurrentState = QuestState.Progress;
                current = current.Children[0];
            }

            Command command = new DoneUpdateCommand(current, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(QuestState.Progress, current.CurrentState);
            Assert.AreEqual(QuestState.Progress, parent.CurrentState);
            Assert.False(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.CurrentState == QuestState.Done));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteOnlyChildTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(1);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, QuestState.Done);
            parent.CurrentState = QuestState.Progress;

            Quest current = parent;
            current.Children[0].Children[1].CurrentState = QuestState.Progress;
            while (current.Children.Count != 0)
            {
                current.Children[0].CurrentState = QuestState.Progress;
                current = current.Children[0];
            }

            Command command = new DoneUpdateCommand(current, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(QuestState.Done, current.CurrentState);
            Assert.AreEqual(QuestState.Progress, parent.CurrentState);
            Assert.IsFalse(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.CurrentState == QuestState.Done));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoOnlyChildTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Anything)).Repeat.Times(1);
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Anything)).Return(true).Repeat.Times(1);

            Quest parent = QuestHelper.CreateCompositeQuest(2, 3, QuestState.Done);
            parent.CurrentState = QuestState.Progress;

            Quest current = parent;
            current.Children[0].Children[1].CurrentState = QuestState.Progress;
            while (current.Children.Count != 0)
            {
                current.Children[0].CurrentState = QuestState.Progress;
                current = current.Children[0];
            }

            Command command = new DoneUpdateCommand(current, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(QuestState.Progress, current.CurrentState);
            Assert.AreEqual(QuestState.Progress, parent.CurrentState);
            Assert.IsFalse(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.CurrentState == QuestState.Done));

            repository.VerifyAllExpectations();
        }
    }
}
