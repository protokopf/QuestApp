using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using NUnit.Framework;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.FactoriesTest
{
    [TestFixture]
    class DefaultStateCommandsFectoryTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange & Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new DefaultStateCommandsFactory(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questTree", ex.ParamName);
        }

        [Test]
        public void DoneFailTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => factory.DoneQuest(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void FailFailTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => factory.FailQuest(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void StartFailTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => factory.StartQuest(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void CancelFailTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => factory.CancelQuest(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void DoneQuestCommandTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ICommand command = factory.DoneQuest(quest);

            //Assert
            Assert.AreEqual(typeof(CompositeCommand), command.GetType());
        }

        [Test]
        public void FailQuestCommandTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ICommand command = factory.FailQuest(quest);

            //Assert
            Assert.AreEqual(typeof(CompositeCommand), command.GetType());
        }

        [Test]
        public void CancelQuestCommandTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ICommand command = factory.CancelQuest(quest);

            //Assert
            Assert.AreEqual(typeof(CompositeCommand),command.GetType());
        }

        [Test]
        public void StartQuestCommandTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new DefaultStateCommandsFactory(repository);

            //Act
            ICommand command = factory.StartQuest(quest);

            //Assert
            Assert.AreEqual(typeof(CompositeCommand), command.GetType());
        }
    }
}
