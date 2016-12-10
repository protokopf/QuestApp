using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModels
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
