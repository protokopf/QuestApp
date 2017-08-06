using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.RepositoryTest
{
    [TestFixture]
    class AddQuestCommandTest
    {
        [Test]
        public void InitializeFailQuestNullTest()
        {
            //Arrange
            Quest toAdd = new Quest();
            Quest parent = new Quest();
            IQuestTree repository = MockRepository.GenerateMock<IQuestTree>();

            //Act
            ArgumentNullException parentEx = Assert.Throws<ArgumentNullException>(() => new AddQuestCommand(repository, null, toAdd));
            ArgumentNullException childEx = Assert.Throws<ArgumentNullException>(() => new AddQuestCommand(repository, parent, null));

            //Assert
            Assert.IsNotNull(parentEx);
            Assert.AreEqual("parent", parentEx.ParamName);

            Assert.IsNotNull(childEx);
            Assert.AreEqual("childToAdd", childEx.ParamName);
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            Quest parent = new Quest();
            Quest toAdd = new Quest();

            repository.Expect(rep => rep.AddChild(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(toAdd))).
                Repeat.Once();

            ICommand command = new AddQuestCommand(repository, parent, toAdd);

            //Act
            command.Execute();

            //Assert
            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            Quest parent = new Quest();
            Quest toAdd = new Quest();

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(rep => rep.AddChild(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(toAdd))).
                Repeat.Once();
            repository.Expect(rep => rep.RemoveChild(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(toAdd))).
                Repeat.Once();


            ICommand command = new AddQuestCommand(repository, parent, toAdd);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            repository.VerifyAllExpectations();
        }

    }
}
