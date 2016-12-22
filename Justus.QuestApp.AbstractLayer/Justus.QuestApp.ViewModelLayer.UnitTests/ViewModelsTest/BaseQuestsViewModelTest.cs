using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class BaseQuestsViewModelTest
    {
        [TearDown]
        public void TearDown()
        {
            ServiceLocator.ReleaseAll();
        }

        [Test]
        public void SaveChangesTest()
        {
            //Arrange
            ITaskWrapper wrapper = new AutoStartTaskWrapper();

            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.Save()).Repeat.Once();

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register<ITaskWrapper>(() => { return wrapper; });
            ServiceLocator.Register<IQuestRepository>(() => { return repository; });
            ServiceLocator.Register<ICommandManager>(() => { return comManager; });

            BaseQuestsViewModel vm = new BaseQuestsViewModel();

            //Act
            vm.SaveChanges();
            Task.WaitAll();

            //Assert
            repository.VerifyAllExpectations();
            comManager.VerifyAllExpectations();
        }
    }
}
