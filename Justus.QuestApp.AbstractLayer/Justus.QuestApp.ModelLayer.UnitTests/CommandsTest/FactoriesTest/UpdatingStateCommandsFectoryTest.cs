using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Commands.State;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.FactoriesTest
{
    [TestFixture]
    class UpdatingStateCommandsFectoryTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange & Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new UpdatingStateCommandsFactory(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("repository", ex.ParamName);
        }

        [Test]
        public void DoneFailTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

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
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

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
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

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
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

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
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

            //Act
            Command command = factory.DoneQuest(quest);

            //Assert
            Assert.IsTrue(command is UpHierarchyStateUpdateCommand);
        }

        [Test]
        public void FailQuestCommandTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

            //Act
            Command command = factory.FailQuest(quest);

            //Assert
            Assert.IsTrue(command is UpHierarchyStateUpdateCommand);
        }

        [Test]
        public void CancelQuestCommandTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

            //Act
            Command command = factory.CancelQuest(quest);

            //Assert
            Assert.IsTrue(command is DownHierarchyStateUpdateCommand);
        }

        [Test]
        public void StartQuestCommandTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            Quest quest = MockRepository.GeneratePartialMock<Quest>();

            IStateCommandsFactory factory = new UpdatingStateCommandsFactory(repository);

            //Act
            Command command = factory.StartQuest(quest);

            //Assert
            Assert.IsTrue(command is ThisStateUpdateCommand);
        }
    }
}
