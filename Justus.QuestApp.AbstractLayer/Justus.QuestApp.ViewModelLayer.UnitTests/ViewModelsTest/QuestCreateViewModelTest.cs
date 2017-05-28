using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class QuestCreateViewModelTest
    {
        [TestCase(false,false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void SaveTestWithStartTimeDeadline(bool useStartTime, bool useDeadLine)
        {
            //Arrange
            int parentId = 42;
            string title = "title";
            string decription = "description";
            bool isImportant = true;
            DateTime startTime = DateTime.Now;
            DateTime deadline = DateTime.Now;

            Quest quest = new FakeQuest();

            Command addCommand = MockRepository.GenerateStrictMock<Command>();
            addCommand.Expect(ac => ac.Execute()).
                Repeat.Once();

            IQuestRepository questRepository = MockRepository.GenerateStrictMock<IQuestRepository>();

            IRepositoryCommandsFactory factory = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            factory.Expect(f => f.AddQuest(Arg<Quest>.Is.Equal(quest)))
                .Return(addCommand)
                .Repeat.Once();


            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            QuestCreateViewModel viewModel = new QuestCreateViewModel(quest, questValidator, factory);

            viewModel.QuestDetails.ParentId = parentId;
            viewModel.QuestDetails.Title = title;
            viewModel.QuestDetails.Description = decription;
            viewModel.QuestDetails.IsImportant = isImportant;
            viewModel.QuestDetails.StartTime = startTime;
            viewModel.QuestDetails.Deadline = deadline;
            viewModel.QuestDetails.UseStartTime = useStartTime;
            viewModel.QuestDetails.UseDeadline = useDeadLine;
                

            //Act
            viewModel.Action();

            //Assert
            addCommand.VerifyAllExpectations();
            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();

            Assert.AreEqual(title, quest.Title);
            Assert.AreEqual(decription, quest.Description);
            Assert.AreEqual(isImportant, quest.IsImportant);

            Assert.AreEqual(useStartTime, quest.StartTime == startTime);
            Assert.AreEqual(useDeadLine, quest.Deadline == deadline);

            Assert.AreEqual(!useStartTime, quest.StartTime == default(DateTime));
            Assert.AreEqual(!useDeadLine, quest.Deadline == default(DateTime));
        }

        [Test]
        public void ViewModelTrimsTitleAndDescription()
        {
            //Arrange
            string nonTrimedTitle = "   hello   ";
            string nonTrimedDescription = "    description  \t";

            Quest quest = new FakeQuest();

            Command command = MockRepository.GenerateStrictMock<Command>();
            command.Expect(cm => cm.Execute()).
                Repeat.Once();

            IQuestRepository questRepository = MockRepository.GenerateStrictMock<IQuestRepository>();

            IRepositoryCommandsFactory factory = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            factory.Expect(f => f.AddQuest(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Once()
                .Return(command);

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            QuestCreateViewModel viewModel = new QuestCreateViewModel(quest, questValidator, factory);

            viewModel.QuestDetails.Title = nonTrimedTitle;
            viewModel.QuestDetails.Description = nonTrimedDescription;
            

            //Act
            viewModel.Action();

            //Assert
            Quest savedQuest =
                factory.GetArgumentsForCallsMadeOn(f => f.AddQuest(Arg<Quest>.Is.NotNull))[0][0]
                    as Quest;

            Assert.IsNotNull(savedQuest);
            Assert.AreEqual(nonTrimedTitle.Trim(), savedQuest.Title);
            Assert.AreEqual(nonTrimedDescription.Trim(), savedQuest.Description);

            command.VerifyAllExpectations();
            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
        }

        [Test]
        public void ValidateTest()
        {
            //Arrange
            Quest quest = new FakeQuest();
            ClarifiedResponse<int> response = new ClarifiedResponse<int>();

            IQuestRepository questRepository = MockRepository.GenerateStrictMock<IQuestRepository>();

            IRepositoryCommandsFactory factory = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();
            questValidator.Expect(qv => qv.Validate(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Once()
                .Return(response);

            QuestCreateViewModel viewModel = new QuestCreateViewModel(quest, questValidator, factory);

            //Act
            ClarifiedResponse<int> returnedResponse = viewModel.Validate();

            //Assert
            Assert.AreEqual(response, returnedResponse);

            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
        }
    }
}
