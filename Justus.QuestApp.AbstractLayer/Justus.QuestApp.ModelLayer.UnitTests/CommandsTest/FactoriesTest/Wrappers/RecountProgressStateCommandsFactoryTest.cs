using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Factories.Wrappers;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.FactoriesTest.Wrappers
{
    [TestFixture]
    class RecountProgressStateCommandsFactoryTest
    {
        [Test]
        public void DoneMethodTest()
        {
            //Arrange
            Quest quest = new Quest();
            Command innerDoneCommand = MockRepository.GeneratePartialMock<Command>();

            IStateCommandsFactory innerFactory = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            innerFactory.Expect(inf => inf.DoneQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerDoneCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();

            RecountProgressStateCommandsFactory factory = new RecountProgressStateCommandsFactory(innerFactory, recounter);

            //Act
            Command doneCommand = factory.DoneQuest(quest);

            //Assert
            Assert.IsNotNull(doneCommand);
            RecountQuestProgressCommandWrapper recountCommand = doneCommand as RecountQuestProgressCommandWrapper;
            Assert.IsNotNull(recountCommand);

            innerDoneCommand.VerifyAllExpectations();
            innerFactory.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void CancelMethodTest()
        {
            //Arrange
            Quest quest = new Quest();
            Command innerCancelCommand = MockRepository.GeneratePartialMock<Command>();

            IStateCommandsFactory innerFactory = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            innerFactory.Expect(inf => inf.CancelQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerCancelCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();

            RecountProgressStateCommandsFactory factory = new RecountProgressStateCommandsFactory(innerFactory, recounter);

            //Act
            Command cancelCommand = factory.CancelQuest(quest);

            //Assert
            Assert.IsNotNull(cancelCommand);
            RecountQuestProgressCommandWrapper recountCommand = cancelCommand as RecountQuestProgressCommandWrapper;
            Assert.IsNotNull(recountCommand);

            innerCancelCommand.VerifyAllExpectations();
            innerFactory.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void FailMethodTest()
        {
            //Arrange
            Quest quest = new Quest();
            Command innerFailCommand = MockRepository.GeneratePartialMock<Command>();

            IStateCommandsFactory innerFactory = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            innerFactory.Expect(inf => inf.FailQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerFailCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();

            RecountProgressStateCommandsFactory factory = new RecountProgressStateCommandsFactory(innerFactory, recounter);

            //Act
            Command failCommand = factory.FailQuest(quest);

            //Assert
            Assert.IsNotNull(failCommand);
            RecountQuestProgressCommandWrapper recountCommand = failCommand as RecountQuestProgressCommandWrapper;
            Assert.IsNotNull(recountCommand);

            innerFailCommand.VerifyAllExpectations();
            innerFactory.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void StartMethodTest()
        {
            //Arrange
            Quest quest = new Quest();
            Command innerStartCommand = MockRepository.GeneratePartialMock<Command>();

            IStateCommandsFactory innerFactory = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            innerFactory.Expect(inf => inf.StartQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerStartCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();

            RecountProgressStateCommandsFactory factory = new RecountProgressStateCommandsFactory(innerFactory, recounter);

            //Act
            Command startCommand = factory.StartQuest(quest);

            //Assert
            Assert.IsNotNull(startCommand);
            RecountQuestProgressCommandWrapper recountCommand = startCommand as RecountQuestProgressCommandWrapper;
            Assert.IsNotNull(recountCommand);

            innerStartCommand.VerifyAllExpectations();
            innerFactory.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

    }
}
