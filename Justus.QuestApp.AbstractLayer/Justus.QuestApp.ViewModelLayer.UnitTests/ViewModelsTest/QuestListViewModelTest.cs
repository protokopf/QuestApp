using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class QuestListViewModelTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            //Act
            ArgumentNullException modelNullEx = Assert.Throws<ArgumentNullException>(() => new QuestListViewModel(null, stateCommands, repoCommands));
            ArgumentNullException stateCommandslNullEx = Assert.Throws<ArgumentNullException>(() => new QuestListViewModel(model, null, repoCommands));
            ArgumentNullException repoCommandsNullEx = Assert.Throws<ArgumentNullException>(() => new QuestListViewModel(model, stateCommands, null));

            //Assert
            Assert.IsNotNull(modelNullEx);
            Assert.IsNotNull(stateCommandslNullEx);
            Assert.IsNotNull(repoCommandsNullEx);

            Assert.AreEqual("questListModel", modelNullEx.ParamName);
            Assert.AreEqual("stateCommandsFactory", stateCommandslNullEx.ParamName);
            Assert.AreEqual("treeCommandsFactory", repoCommandsNullEx.ParamName);
        }

        [Test]
        public void QuestsListTitleModelInRootTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(mo => mo.IsInTheRoot).
                Repeat.Once().
                Return(true);
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            string questsListTitle = viewModel.QuestsListTitle;

            //Assert
            Assert.IsNull(questsListTitle);
        }

        [Test]
        public void QuestsListTitleNotInRootTestTest()
        {
            //Arrange
            const string expectedTitle = "fakeTitleOfFakeQuest";

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(mo => mo.IsInTheRoot).
                Repeat.Once().
                Return(false);
            model.Expect(mo => mo.Parent).
                Repeat.Once().
                Return(new FakeQuest {Title = expectedTitle });

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            string questsListTitle = viewModel.QuestsListTitle;

            //Assert
            Assert.IsNotNull(questsListTitle);
            Assert.AreEqual(expectedTitle, questsListTitle);
        }

        [Test]
        public void LeavesTest()
        {
            //Arrange
            List<Quest> expectedQuests = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).
                Repeat.Once().
                Return(expectedQuests);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            List<Quest> quests = viewModel.Leaves;

            //Assert
            Assert.IsNotNull(quests);
            Assert.AreEqual(expectedQuests, quests);
            model.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToRootTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.TraverseToRoot()).
                Repeat.Once().
                Return(true);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            bool result  = viewModel.TraverseToRoot();

            //Assert
            Assert.IsTrue(result);
            model.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToParentTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.TraverseToParent()).
                Repeat.Once().
                Return(true);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            bool result = viewModel.TraverseToParent();

            //Assert
            Assert.IsTrue(result);
            model.VerifyAllExpectations();
        }

        [Test]
        public void TraverseToLeafTest()
        {
            //Arrange
            int leaf = 42;

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.TraverseToLeaf(Arg<int>.Is.Equal(leaf))).
                Repeat.Once().
                Return(true);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            bool result = viewModel.TraverseToLeaf(leaf);

            //Assert
            Assert.IsTrue(result);
            model.VerifyAllExpectations();
        }

        [Test]
        public void InTopRootTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.IsInTheRoot).
                Repeat.Once().
                Return(true);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            bool result = viewModel.InTopRoot;

            //Assert
            Assert.IsTrue(result);
            model.VerifyAllExpectations();
        }

        [Test]
        public void RootIdTest()
        {
            //Arrange
            int questId = 42;

            Quest parent = new FakeQuest()
            {
                Id = questId
            };

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(mo => mo.Parent).
                Repeat.Once().
                Return(parent);
            


            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            int id = viewModel.RootId;

            //Assert
            Assert.AreEqual(questId, id);
            model.VerifyAllExpectations();
        }

        [Test]
        public void GetLeafIdInvalidPositionTest()
        {
            //Arrange
            int position = 1;

            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).Repeat.Once().Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            int? id = viewModel.GetLeafId(position);

            //Assert
            Assert.AreEqual(null, id);
            model.VerifyAllExpectations();
        }

        [Test]
        public void GetLeafIdValidPositionTest()
        {
            //Arrange
            int position = 0;
            int questId = 42;

            List<Quest> leaves = new List<Quest>()
            {
                new FakeQuest() {Id = questId}
            };

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).Repeat.Once().Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            int? id = viewModel.GetLeafId(position);

            //Assert
            Assert.AreEqual(questId, id);
            model.VerifyAllExpectations();
        }

        [Test]
        public void DeleteQuestInvalidPositionTest()
        {
            //Arrange
            int position = 1;

            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).Repeat.Once().Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            Task deleteTask = viewModel.DeleteQuest(position);

            //Assert
            Assert.IsNull(deleteTask);
            model.VerifyAllExpectations();
        }

        [Test]
        public void DeleteQuestValidPositionTest()
        {
            //Arrange
            int position = 0;
            Quest parent = new FakeQuest();
            Quest toDelete = new FakeQuest();

            List<Quest> leaves = new List<Quest> {toDelete};

            ICommand deleteCommand = MockRepository.GenerateStrictMock<ICommand>();
            deleteCommand.Expect(dc => dc.Execute()).
                Repeat.Once().
                Return(true);
            deleteCommand.Expect(dc => dc.Commit()).
                Repeat.Once().
                Return(true);

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).
                Repeat.Once().
                Return(leaves);
            model.Expect(m => m.Parent).
                Repeat.Once().
                Return(parent);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();

            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            repoCommands.Expect(rc => rc.DeleteQuest(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(toDelete))).
                Repeat.Once().
                Return(deleteCommand);

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            Task deleteTask = viewModel.DeleteQuest(position);

            //Assert
            Assert.IsNotNull(deleteTask);
            deleteTask.Wait();

            model.VerifyAllExpectations();
            deleteCommand.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
        }

        [Test]
        public void StartQuestInvalidPositionTest()
        {
            //Arrange
            int position = 1;

            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).Repeat.Once().Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            stateCommands.Expect(sc => sc.StartQuest(Arg<Quest>.Is.Anything)).
                Repeat.Never();

            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            Task deleteTask = viewModel.StartQuest(position);

            //Assert
            Assert.IsNull(deleteTask);

            model.VerifyAllExpectations();
            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
        }

        [Test]
        public void StartQuestValidPositionTest()
        {
            //Arrange
            int position = 0;
            Quest toStart = new FakeQuest();

            List<Quest> leaves = new List<Quest> { toStart };

            ICommand startCommand = MockRepository.GenerateStrictMock<ICommand>();
            startCommand.Expect(dc => dc.Execute()).
                Repeat.Once().
                Return(true);
            startCommand.Expect(dc => dc.Commit()).
                Repeat.Once().
                Return(true);

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            stateCommands.Expect(sc => sc.StartQuest(Arg<Quest>.Is.Equal(toStart))).
                Repeat.Once().
                Return(startCommand);

            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            Task startTask = viewModel.StartQuest(position);

            //Assert
            Assert.IsNotNull(startTask);
            startTask.Wait();

            startCommand.VerifyAllExpectations();
            model.VerifyAllExpectations();
            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
        }

        [Test]
        public void RefreshTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Refresh()).
                Repeat.Once();

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(model, stateCommands, repoCommands);

            //Act
            viewModel.Refresh();

            //Assert
            model.VerifyAllExpectations();
        }

    }
}
