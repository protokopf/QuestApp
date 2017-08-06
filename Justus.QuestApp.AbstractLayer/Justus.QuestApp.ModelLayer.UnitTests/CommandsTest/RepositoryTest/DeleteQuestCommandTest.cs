using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.RepositoryTest
{
    [TestFixture]
    class DeleteQuestCommandTest
    {
        [Test]
        public void InitializeFailQuestNullTest()
        {
            //Arrange
            Quest toDelete = new Quest();
            Quest parent = new Quest();
            IQuestTree repository = MockRepository.GenerateMock<IQuestTree>();

            //Act
            ArgumentNullException parentEx = Assert.Throws<ArgumentNullException>(() => new DeleteQuestCommand(repository, null, toDelete));
            ArgumentNullException childEx = Assert.Throws<ArgumentNullException>(() => new DeleteQuestCommand(repository, parent, null));

            //Assert
            Assert.IsNotNull(parentEx);
            Assert.AreEqual("parent", parentEx.ParamName);

            Assert.IsNotNull(childEx);
            Assert.AreEqual("questToDelete", childEx.ParamName);
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            Quest parent = QuestHelper.CreateQuest();
            Quest toDelete = QuestHelper.CreateQuest(42);

            repository.Expect(rep => rep.RemoveChild(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(toDelete))).
                Repeat.Once();

            ICommand command = new DeleteQuestCommand(repository,parent, toDelete);

            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoAfterExecutionTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            Quest parent = new Quest();
            Quest toDelete = new Quest();
            
            repository.Expect(rep => rep.RemoveChild(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(toDelete))).
                Repeat.Once();
            repository.Expect(rep => rep.AddChild(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(toDelete))).Repeat.Once();

            ICommand command = new DeleteQuestCommand(repository,parent, toDelete);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            repository.VerifyAllExpectations();
        }
    }
}
