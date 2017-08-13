using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State.Common;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest.Common
{
    [TestFixture]
    public class ChangeStatCurrentTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(ChangeStateCurrent).IsSubclassOf(typeof(Commands.Abstracts.Hierarchy.ThisQuestCommand)));
        }

        [Test]
        public void ExecuteSuccessfulTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest(State.Progress);
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once();


            ICommand command = new ChangeStateCurrent(quest, repository, State.Failed);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Failed, quest.State);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteChangeOnlyThisStateTest()
        {
            Quest quest = QuestHelper.CreateQuest(State.Progress);
            quest.Parent = QuestHelper.CreateQuest(State.Progress);
            quest.Children = new List<Quest>
            {
                QuestHelper.CreateQuest(State.Progress)
            };
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once();


            ICommand command = new ChangeStateCurrent(quest, repository, State.Failed);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Failed, quest.State);
            Assert.AreEqual(State.Progress, quest.Parent.State);
            Assert.AreEqual(State.Progress, quest.Children[0].State);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoSuccessfulTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest(State.Progress);
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once();
            repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once();

            ICommand command = new ChangeStateCurrent(quest, repository, State.Failed);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(State.Progress, quest.State);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest(State.Progress);
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once();
            repository.Expect(r => r.Save()).
                Repeat.Once();


            ICommand command = new ChangeStateCurrent(quest, repository, State.Failed);

            //Act
            command.Execute();
            command.Commit();

            //Assert
            Assert.AreEqual(State.Failed, quest.State);

            repository.VerifyAllExpectations();
        }
    }
}
