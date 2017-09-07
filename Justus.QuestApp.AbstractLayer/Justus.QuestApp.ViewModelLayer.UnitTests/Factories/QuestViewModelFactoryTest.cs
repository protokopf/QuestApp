using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.ViewModelLayer.Factories;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.Factories
{
    [TestFixture]
    class QuestViewModelFactoryTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new QuestViewModelFactory(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questFactory", ex.ParamName);
        }

        [Test]
        public void CreateQuestViewModelTest()
        {
            //Arrange
            Quest model = new FakeQuest();

            IQuestFactory questFactory = MockRepository.GenerateStrictMock<IQuestFactory>();
            questFactory.Expect(qf => qf.CreateQuest()).
                Repeat.Once().
                Return(model);

            QuestViewModelFactory questVmFactory = new QuestViewModelFactory(questFactory);

            //Act
            IQuestViewModel vm = questVmFactory.CreateQuestViewModel();
            
            //Assert
            Assert.IsNotNull(vm);
            Assert.AreEqual(model, vm.Model);
            Assert.IsInstanceOf<QuestViewModel>(vm);

        }
    }
}
