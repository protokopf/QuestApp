using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class AvailableQuestListVIewModelTest
    {
        [Test]
        public void CtorNotThrowExceptionsTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            //Act && Assert
            Assert.DoesNotThrow(() => new AvailableQuestListViewModel(model, stateCommands, repoCommands));

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }
    }
}
