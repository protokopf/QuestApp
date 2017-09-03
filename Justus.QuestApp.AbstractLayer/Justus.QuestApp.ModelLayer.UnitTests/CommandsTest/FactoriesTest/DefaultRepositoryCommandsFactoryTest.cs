using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
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
                Assert.Throws<ArgumentNullException>(() => new DefaultTreeCommandsFactory(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("repository", ex.ParamName);
        }

        [Test]
        public void AddCommandTest()
        {
            //Arrange
            Quest quest = new FakeQuest();
            Quest parent = new FakeQuest();

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            ITreeCommandsFactory factory = new DefaultTreeCommandsFactory(repository);

            //Act
            ICommand addCommand = factory.AddQuest(parent, quest);

            //Assert
            Assert.IsNotNull(addCommand);
            Assert.IsTrue(addCommand is CompositeCommand);
        }

        [Test]
        public void UpdateCommandTest()
        {
            //Arrange
            ITreeCommandsFactory factory = new DefaultTreeCommandsFactory(
                MockRepository.GenerateStrictMock<IQuestTree>());

            Quest quest = new FakeQuest();

            //Act
            ICommand addCommand = factory.UpdateQuest(quest);

            //Assert
            Assert.IsNotNull(addCommand);
            Assert.IsTrue(addCommand is CurrentQuestCommand);
        }

        [Test]
        public void DeleteCommandTest()
        {
            //Arrange
            Quest quest = new FakeQuest();
            Quest parent = new FakeQuest();

            ITreeCommandsFactory factory = new DefaultTreeCommandsFactory(
                MockRepository.GenerateStrictMock<IQuestTree>());


            //Act
            ICommand addCommand = factory.DeleteQuest(parent, quest);

            //Assert
            Assert.IsNotNull(addCommand);
            Assert.IsTrue(addCommand is CompositeCommand);
        }
    }
}
