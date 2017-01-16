using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class ActiveQuestListViewModelTest
    {
        [TearDown]
        public void TearDown()
        {
            ServiceLocator.ReleaseAll();
        }

        [Test]
        public void FilterQuest()
        {
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            List<Quest> fromRepository = new List<Quest>()
            {
                new FakeQuest {CurrentState = QuestState.Done},
                new FakeQuest {CurrentState = QuestState.Failed},
                new FakeQuest {CurrentState = QuestState.Idle},
                new FakeQuest {CurrentState = QuestState.Progress}
            };

            repository.Expect(rep => rep.GetAll()).Repeat.Once().Return(fromRepository);

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => { return repository; });
            ServiceLocator.Register(() => { return comManager; });

            QuestListViewModel viewModel = new ActiveQuestListViewModel();

            //Act
            List<Quest> quests = viewModel.CurrentChildren;

            //Assert
            Assert.IsNotNull(quests);
            Assert.AreEqual(1, quests.Count);
            Assert.AreEqual(QuestState.Progress, quests[0].CurrentState);

            Assert.AreEqual(4, fromRepository.Count);
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Done));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Failed));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Idle));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Progress));

            repository.VerifyAllExpectations();
        }


    }
}
