using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class BaseViewModelTest
    {
        [Test]
        public void IsBusyChangedTest()
        {
            //Arrange
            BaseViewModel vm = new BaseViewModel();
            bool isNotified = false;

            vm.IsBusyChanged += (sender, args) => { isNotified = true; };

            //Act
            vm.IsBusy = true;

            //Assert
            Assert.IsTrue(vm.IsBusy);
            Assert.IsTrue(isNotified);
        }
    }
}
