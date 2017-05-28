using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.FactoriesTest
{
    [TestFixture]
    class DefaultRepositoryCommandsFactoryTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange && Act
            ArgumentNullException ex =
                Assert.Throws<ArgumentNullException>(() => new DefaultRepositoryCommandsFactory(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("repository", ex.ParamName);
        }

        [Test]
        public void AddCommandTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.Get(null)).IgnoreArguments().Repeat.Once().Return(null);

            IRepositoryCommandsFactory factory = new DefaultRepositoryCommandsFactory(repository);

            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            //Act
            Command addCommand = factory.AddQuest(quest);

            //Assert
            Assert.IsNotNull(addCommand);
            Assert.AreEqual(typeof(AddQuestCommand),addCommand.GetType());
        }

        [Test]
        public void UpdateCommandTest()
        {
            //Arrange
            IRepositoryCommandsFactory factory = new DefaultRepositoryCommandsFactory(
                MockRepository.GenerateStrictMock<IQuestRepository>());

            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            //Act
            Command addCommand = factory.UpdateQuest(quest);

            //Assert
            Assert.IsNotNull(addCommand);
            Assert.AreEqual(typeof(UpdateQuestCommand), addCommand.GetType());
        }

        [Test]
        public void DeleteCommandTest()
        {
            //Arrange
            IRepositoryCommandsFactory factory = new DefaultRepositoryCommandsFactory(
                MockRepository.GenerateStrictMock<IQuestRepository>());

            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            //Act
            Command addCommand = factory.DeleteQuest(quest);

            //Assert
            Assert.IsNotNull(addCommand);
            Assert.AreEqual(typeof(DeleteQuestCommand), addCommand.GetType());
        }
    }
}
