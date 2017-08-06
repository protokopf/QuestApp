using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.RepositoryTest
{
    [TestFixture]
    class UpdateQuestTest
    {

        [Test]
        public void InitializeFailQuestNullTest()
        {
            //Arrange
            Quest toUpdate = null;
            IQuestTree repository = MockRepository.GenerateMock<IQuestTree>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new UpdateQuestCommand(repository, toUpdate));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questToUpdate", ex.ParamName);
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            Quest toUpdate = new FakeQuest();

            repository.Expect(rep => rep.Update(Arg<Quest>.Is.Equal(toUpdate))).
                Repeat.Once();

            ICommand command = new UpdateQuestCommand(repository, toUpdate);

            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            Quest toUpdate = new FakeQuest();

            repository.Expect(rep => rep.Update(Arg<Quest>.Is.Equal(toUpdate))).
                Repeat.Once();
            repository.Expect(rep => rep.RevertUpdate(Arg<Quest>.Is.Equal(toUpdate))).
                Repeat.Once();

            ICommand command = new UpdateQuestCommand(repository, toUpdate);

            //Act
            bool executeResult = command.Execute();
            bool undoResult = command.Undo();

            //Assert
            Assert.IsTrue(executeResult);
            Assert.IsTrue(undoResult);

            repository.VerifyAllExpectations();
        }
    }
}
