using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Factories.Wrappers;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.FactoriesTest.Wrappers
{
    [TestFixture]
    class RecountProgressRepositoryCommandsFactoryTest
    {
        [Test]
        public void AddQuestMethodTest()
        {
            //Arrange
            Quest parent = new Quest();
            Quest quest = new Quest();
            ICommand innerAddCommand = MockRepository.GenerateStrictMock<ICommand>();

            ITreeCommandsFactory innerFactory = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            innerFactory.Expect(inf => inf.AddQuest(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerAddCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();


            RecountProgressTreeCommandsFactory factory = new RecountProgressTreeCommandsFactory(innerFactory, recounter);

            //Act
            ICommand addCommand = factory.AddQuest(parent, quest);

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
            Quest parent = new Quest();
            ICommand innerDeleteCommand = MockRepository.GenerateStrictMock<ICommand>();

            ITreeCommandsFactory innerFactory = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            innerFactory.Expect(inf => inf.DeleteQuest(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerDeleteCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();


            RecountProgressTreeCommandsFactory factory = new RecountProgressTreeCommandsFactory(innerFactory, recounter);

            //Act
            ICommand deleteCommand = factory.DeleteQuest(parent, quest);

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
            ICommand innerUpdateCommand = MockRepository.GenerateStrictMock<ICommand>();

            ITreeCommandsFactory innerFactory = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            innerFactory.Expect(inf => inf.UpdateQuest(Arg<Quest>.Is.Equal(quest))).Repeat.Once().Return(innerUpdateCommand);

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();


            RecountProgressTreeCommandsFactory factory = new RecountProgressTreeCommandsFactory(innerFactory, recounter);

            //Act
            ICommand updateCommand = factory.UpdateQuest(quest);

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
