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
    class RecountProgressRepositoryCommandsFactoryTest
    {
        [Test]
        public void AddQuestMethodTest()
        {
            //Arrange
            Quest quest = new Quest();
            Command innerAddCommand = MockRepository.GeneratePartialMock<Command>();

            IRepositoryCommandsFactory innerFactory = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            innerFactory.Expect(inf => inf.AddQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerAddCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();


            RecountProgressRepositoryCommandsFactory factory = new RecountProgressRepositoryCommandsFactory(innerFactory, recounter);

            //Act
            Command addCommand = factory.AddQuest(quest);

            //Assert
            Assert.IsNotNull(addCommand);
            RecountQuestProgressCommandWrapper recountCommand = addCommand as RecountQuestProgressCommandWrapper;
            Assert.IsNotNull(recountCommand);

            innerAddCommand.VerifyAllExpectations();
            innerFactory.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void DeleteQuestMethodTest()
        {
            //Arrange
            Quest quest = new Quest();
            Command innerDeleteCommand = MockRepository.GeneratePartialMock<Command>();

            IRepositoryCommandsFactory innerFactory = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            innerFactory.Expect(inf => inf.DeleteQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerDeleteCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();


            RecountProgressRepositoryCommandsFactory factory = new RecountProgressRepositoryCommandsFactory(innerFactory, recounter);

            //Act
            Command deleteCommand = factory.DeleteQuest(quest);

            //Assert
            Assert.IsNotNull(deleteCommand);
            RecountQuestProgressCommandWrapper recountCommand = deleteCommand as RecountQuestProgressCommandWrapper;
            Assert.IsNotNull(recountCommand);

            innerDeleteCommand.VerifyAllExpectations();
            innerFactory.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void UpdateQuestMethodTest()
        {
            //Arrange
            Quest quest = new Quest();
            Command innerUpdateCommand = MockRepository.GeneratePartialMock<Command>();

            IRepositoryCommandsFactory innerFactory = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            innerFactory.Expect(inf => inf.UpdateQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerUpdateCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();


            RecountProgressRepositoryCommandsFactory factory = new RecountProgressRepositoryCommandsFactory(innerFactory, recounter);

            //Act
            Command updateCommand = factory.UpdateQuest(quest);

            //Assert
            Assert.IsNotNull(updateCommand);
            RecountQuestProgressCommandWrapper recountCommand = updateCommand as RecountQuestProgressCommandWrapper;
            Assert.IsNotNull(recountCommand);

            innerUpdateCommand.VerifyAllExpectations();
            innerFactory.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }
    }
}
