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
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest
{
    [TestFixture]
    class ThisStateUpdateCommandTest
    {
        [Test]
        public void ExecuteSuccessfulTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest(QuestState.Progress);
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(null)).IgnoreArguments().Repeat.Once();


            Command command = new ThisStateUpdateCommand(quest, QuestState.Failed, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(QuestState.Failed, quest.CurrentState);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteChangeOnlyThisQuestStateTest()
        {
            Quest quest = QuestHelper.CreateQuest(QuestState.Progress);
            quest.Parent = QuestHelper.CreateQuest(QuestState.Progress);
            quest.Children = new List<Quest>
            {
                QuestHelper.CreateQuest(QuestState.Progress)
            };
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(null)).IgnoreArguments().Repeat.Once();


            Command command = new ThisStateUpdateCommand(quest, QuestState.Failed, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(QuestState.Failed, quest.CurrentState);
            Assert.AreEqual(QuestState.Progress, quest.Parent.CurrentState);
            Assert.AreEqual(QuestState.Progress, quest.Children[0].CurrentState);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoSuccessfulTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest(QuestState.Progress);
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(r => r.Update(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(r => r.RevertUpdate(null)).IgnoreArguments().Return(true).Repeat.Once();


            Command command = new ThisStateUpdateCommand(quest, QuestState.Failed, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(QuestState.Progress, quest.CurrentState);

            repository.VerifyAllExpectations();
        }
    }
}
