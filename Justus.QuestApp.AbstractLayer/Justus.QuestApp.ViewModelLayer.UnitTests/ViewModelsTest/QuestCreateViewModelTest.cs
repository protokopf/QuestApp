using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
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
            Quest parentQuest = new FakeQuest();

            Command addCommand = MockRepository.GenerateStrictMock<Command>();
            addCommand.Expect(ac => ac.Execute()).
                Repeat.Once();

            IQuestCreator creator = MockRepository.GenerateStrictMock<IQuestCreator>();
            creator.Expect(cr => cr.Create()).
                Return(quest).
                Repeat.Once();

            IQuestRepository questRepository = MockRepository.GenerateStrictMock<IQuestRepository>();
            questRepository.Expect(qr => qr.Get(Arg<Predicate<Quest>>.Is.Anything)).
                Return(parentQuest).
                Repeat.Once();

            IRepositoryCommandsFactory factory = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            factory.Expect(f => f.AddQuest(Arg<Quest>.Is.Equal(quest), Arg<Quest>.Is.Equal(parentQuest)))
                .Return(addCommand)
                .Repeat.Once();
            

            QuestCreateViewModel viewModel = new QuestCreateViewModel(creator, factory, questRepository)
            {
                ParentId = parentId,
                Title = title,
                Description = decription,
                IsImportant = isImportant,
                StartTime = startTime,
                Deadline = deadline,
                UseStartTime = useStartTime,
                UseDeadline = useDeadLine
            };

            //Act
            viewModel.Save();

            //Assert
            addCommand.VerifyAllExpectations();
            creator.VerifyAllExpectations();
            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();

            Assert.AreEqual(title, quest.Title);
            Assert.AreEqual(decription, quest.Description);
            Assert.AreEqual(isImportant, quest.IsImportant);

            Assert.AreEqual(useStartTime, quest.StartTime == startTime);
            Assert.AreEqual(useDeadLine, quest.Deadline == deadline);

            Assert.AreEqual(!useStartTime, quest.StartTime == DateTime.MinValue);
            Assert.AreEqual(!useDeadLine, quest.Deadline == DateTime.MinValue);
        }
    }
}
