using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;
using NUnit.Framework;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest.QuestDetails
{
    [TestFixture]
    class QuestViewModelTest
    {
        [Test]
        public void ModelIsNullGettersTest()
        {
            //Arrange
            QuestViewModel viewModel = new QuestViewModel {Model = null};

            //Act && Assert
            Assert.IsNull(viewModel.Title);
            Assert.IsNull(viewModel.Description);
            Assert.AreEqual(default(bool), viewModel.IsImportant);
            Assert.AreEqual(null, viewModel.StartTime);
            Assert.AreEqual(null, viewModel.Deadline);
        }

        [Test]
        public void ModelIsNullSettersChangeNothingTest()
        {
            //Arrange
            QuestViewModel viewModel = new QuestViewModel { Model = null };

            //Act && Assert
            Assert.DoesNotThrow(() => { viewModel.Title = "anything"; });
            Assert.IsNull(viewModel.Title);

            Assert.DoesNotThrow(() => { viewModel.Description = "anything"; });
            Assert.IsNull(viewModel.Description);

            Assert.DoesNotThrow(() => { viewModel.IsImportant = true; });
            Assert.AreEqual(default(bool), viewModel.IsImportant);

            Assert.DoesNotThrow(() => { viewModel.StartTime = DateTime.Now; });
            Assert.AreEqual(null, viewModel.StartTime);

            Assert.DoesNotThrow(() => { viewModel.Deadline = DateTime.Now; });
            Assert.AreEqual(null, viewModel.Deadline);

            Assert.DoesNotThrow(() => { viewModel.UseStartTime = true; });
            Assert.AreEqual(true, viewModel.UseStartTime);

            Assert.DoesNotThrow(() => { viewModel.UseDeadline = true; });
            Assert.AreEqual(true, viewModel.UseDeadline);
        }

        [Test]
        public void ModelNotNullGettersTest()
        {
            //Arrange
            string title = "title";
            string description = "description";
            bool isImportant = true;
            DateTime startTime = DateTime.Now;
            DateTime deadline = DateTime.Now;
            
            Quest model = new FakeQuest()
            {
                Title = title,
                Description = description,
                IsImportant = isImportant,
                StartTime = startTime,
                Deadline = deadline
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model
            };

            //Act && Assert
            Assert.AreEqual(title, viewModel.Title);
            Assert.AreEqual(description, viewModel.Description);
            Assert.AreEqual(isImportant, viewModel.IsImportant);
            Assert.AreEqual(startTime, viewModel.StartTime);
            Assert.AreEqual(deadline, viewModel.Deadline);
        }

        [Test]
        public void ModelNotNullSettersTest()
        {
            //Arrange
            string title = "title";
            string description = "description";
            bool isImportant = true;
            DateTime startTime = DateTime.Now;
            DateTime deadline = DateTime.Now;

            Quest model = new FakeQuest()
            {
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model,
                Title = title,
                Description = description,
                IsImportant = isImportant,
                StartTime = startTime,
                Deadline = deadline
            };

            //Act

            Quest innerModel = viewModel.Model;

            //Assert
            Assert.AreEqual(title, innerModel.Title);
            Assert.AreEqual(description, innerModel.Description);
            Assert.AreEqual(isImportant, innerModel.IsImportant);
            Assert.AreEqual(startTime, innerModel.StartTime);
            Assert.AreEqual(deadline, innerModel.Deadline);
        }

        [Test]
        public void UseStartTimeDeadlineNotNullModelSetterTest()
        {
            //Arrange
            DateTime expectedStartTime = DateTime.Now;
            DateTime expectedDeadline = DateTime.Now + new TimeSpan(1);

            Quest model = new FakeQuest()
            {
                StartTime = expectedStartTime,
                Deadline = expectedDeadline
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model
            };

            //Act
            bool useStartTime = viewModel.UseStartTime;
            bool useDeadline = viewModel.UseDeadline;

            //Assert
            Assert.IsTrue(useStartTime);
            Assert.IsTrue(useDeadline);
        }

        [Test]
        public void UseStartTimeDeadlineNullModelSetterTest()
        {
            //Arrange
            Quest model = new FakeQuest()
            {
                StartTime = null,
                Deadline = null
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model
            };

            //Act
            bool useStartTime = viewModel.UseStartTime;
            bool useDeadline = viewModel.UseDeadline;

            //Assert
            Assert.IsTrue(useStartTime);
            Assert.IsTrue(useDeadline);
        }

        [Test]
        public void UseStartTimeChangingTest()
        {
            //Arrange
            DateTime? expectedStartTime = DateTime.Now;

            Quest model = new FakeQuest
            {
                StartTime = expectedStartTime
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model
            };

            //Act
            DateTime? startBeforeAnyChanges = viewModel.StartTime;

            viewModel.UseStartTime = false;
            DateTime? startAfterUseFalse = viewModel.StartTime;
            DateTime? modelStartAfterUseFalse = viewModel.Model.StartTime;

            viewModel.UseStartTime = true;
            DateTime? startAfterUseTrue = viewModel.StartTime;
            DateTime? modelStartAfterUseTrue = viewModel.Model.StartTime;

            //Assert
            Assert.AreEqual(expectedStartTime, startBeforeAnyChanges);

            Assert.AreEqual(expectedStartTime, startAfterUseFalse);
            Assert.AreEqual(null, modelStartAfterUseFalse);

            Assert.AreEqual(expectedStartTime, startAfterUseTrue);
            Assert.AreEqual(expectedStartTime, modelStartAfterUseTrue);
        }

        [Test]
        public void UseDeadlineChangingTest()
        {
            //Arrange
            DateTime? expectedDeadline = DateTime.Now;

            Quest model = new FakeQuest
            {
                Deadline = expectedDeadline
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model
            };

            //Act
            DateTime? deadBeforeAnyChanges = viewModel.Deadline;
            viewModel.UseDeadline = false;

            DateTime? deadAfterUseFalse = viewModel.Deadline;
            DateTime? modelDeadAfterUseFalse = viewModel.Model.Deadline;

            viewModel.UseDeadline = true;
            DateTime? deadAfterUseTrue = viewModel.Deadline;
            DateTime? modelDeadAfterUseTrue = viewModel.Model.Deadline;

            //Assert
            Assert.AreEqual(expectedDeadline, deadBeforeAnyChanges);

            Assert.AreEqual(expectedDeadline, deadAfterUseFalse);
            Assert.AreEqual(null, modelDeadAfterUseFalse);

            Assert.AreEqual(expectedDeadline, deadAfterUseTrue);
            Assert.AreEqual(expectedDeadline, modelDeadAfterUseTrue);
        }

        [Test]
        public void CachedDeadlineTest()
        {
            //Arrange
            DateTime? expectedDeadline = DateTime.Now;
            DateTime? newDeadline = DateTime.MaxValue;

            Quest model = new FakeQuest
            {
                Deadline = expectedDeadline
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model
            };

            //Act
            DateTime? deadBeforeAnyChanges = viewModel.Deadline;
            viewModel.UseDeadline = false;

            DateTime? deadAfterUseFalse = viewModel.Deadline;
            DateTime? modelDeadAfterUseFalse = viewModel.Model.Deadline;

            viewModel.Deadline = newDeadline;

            viewModel.UseDeadline = true;
            DateTime? deadAfterUseTrue = viewModel.Deadline;
            DateTime? modelDeadAfterUseTrue = viewModel.Model.Deadline;

            //Assert
            Assert.AreEqual(expectedDeadline, deadBeforeAnyChanges);

            Assert.AreEqual(expectedDeadline, deadAfterUseFalse);
            Assert.AreEqual(null, modelDeadAfterUseFalse);

            Assert.AreEqual(newDeadline, deadAfterUseTrue);
            Assert.AreEqual(newDeadline, modelDeadAfterUseTrue);
        }

        [Test]
        public void CachedStartTimeTest()
        {
            //Arrange
            DateTime? expectedStartTime = DateTime.Now;
            DateTime? newStartTime = DateTime.MaxValue;

            Quest model = new FakeQuest
            {
                StartTime = expectedStartTime
            };

            QuestViewModel viewModel = new QuestViewModel
            {
                Model = model
            };

            //Act
            DateTime? startBeforeAnyChanges = viewModel.StartTime;

            viewModel.UseStartTime = false;
            DateTime? startAfterUseFalse = viewModel.StartTime;
            DateTime? modelStartAfterUseFalse = viewModel.Model.StartTime;

            viewModel.StartTime = newStartTime;
            viewModel.UseStartTime = true;

            DateTime? startAfterUseTrue = viewModel.StartTime;
            DateTime? modelStartAfterUseTrue = viewModel.Model.StartTime;

            //Assert
            Assert.AreEqual(expectedStartTime, startBeforeAnyChanges);

            Assert.AreEqual(expectedStartTime, startAfterUseFalse);
            Assert.AreEqual(null, modelStartAfterUseFalse);

            Assert.AreEqual(newStartTime, startAfterUseTrue);
            Assert.AreEqual(newStartTime, modelStartAfterUseTrue);
        }
    }
}
