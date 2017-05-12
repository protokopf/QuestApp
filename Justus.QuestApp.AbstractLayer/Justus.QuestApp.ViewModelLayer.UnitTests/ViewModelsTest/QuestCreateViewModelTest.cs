using System;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class QuestCreateViewModelTest
    {

        [Test]
        public void UseStartTimeFalseDropsDeadlineDateTimeTest()
        {
            //Arrange

            QuestCreateViewModel viewModel = new QuestCreateViewModel();

            DateTime dateTime = DateTime.Now;

            //Act
            viewModel.StartTime = dateTime;
            DateTime beforeSetting = viewModel.StartTime;
            viewModel.UseStartTime = false;
            DateTime afterSetting = viewModel.StartTime;

            //Assert
            Assert.AreEqual(dateTime, beforeSetting);
            Assert.AreEqual(DateTime.MinValue, afterSetting);
        }

        [Test]
        public void UseDeadlineFalseDropsDeadlineDateTimeTest()
        {
            //Arrange

            QuestCreateViewModel viewModel = new QuestCreateViewModel();

            DateTime dateTime = DateTime.Now;

            //Act
            viewModel.Deadline = dateTime;
            DateTime beforeSetting = viewModel.Deadline;
            viewModel.UseDeadline = false;
            DateTime afterSetting = viewModel.Deadline;

            //Assert
            Assert.AreEqual(dateTime, beforeSetting);
            Assert.AreEqual(DateTime.MinValue, afterSetting);
        }

        [Test]
        public void StartTimeWontBeAssignedIfUseStartTimeFalseTest()
        {
            //Arrange

            QuestCreateViewModel viewModel = new QuestCreateViewModel();

            DateTime dateTime = DateTime.Now;

            //Act
            viewModel.UseStartTime = false;
            viewModel.StartTime = dateTime;
            DateTime falseUse = viewModel.StartTime;

            viewModel.UseStartTime = true;
            viewModel.StartTime = dateTime;
            DateTime trueUse = viewModel.StartTime;

            //Assert
            Assert.AreEqual(DateTime.MinValue, falseUse);
            Assert.AreEqual(dateTime, trueUse);
        }

        [Test]
        public void DeadlineWontBeAssignedIfUseDeadlineFalseTest()
        {
            //Arrange
            QuestCreateViewModel viewModel = new QuestCreateViewModel();

            DateTime dateTime = DateTime.Now;

            //Act
            viewModel.UseDeadline = false;
            viewModel.Deadline = dateTime;
            DateTime falseUse = viewModel.Deadline;

            viewModel.UseDeadline = true;
            viewModel.Deadline = dateTime;
            DateTime trueUse = viewModel.Deadline;

            //Assert
            Assert.AreEqual(DateTime.MinValue, falseUse);
            Assert.AreEqual(dateTime, trueUse);
        }
    }
}
