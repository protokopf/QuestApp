using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.Commands.State.Common;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest
{
    [TestFixture]
    class CancelQuestCommandTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(CancelQuestCommand).IsSubclassOf(typeof(ChangeStateDownHierarchyLoadUnload)));
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, State.Progress);
            List<Quest> allHierarchy = new List<Quest>();
            QuestHelper.GatherAllHierarchy(parent, ref allHierarchy);

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            foreach (Quest quest in allHierarchy)
            {
                quest.Progress = 1;
                repository.Expect(r => r.LoadChildren(Arg<Quest>.Is.Equal(quest))).
                    Repeat.Once();
                repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(quest))).
                    Repeat.Once();
            }


            CancelQuestCommand command = new CancelQuestCommand(parent, repository);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Idle, parent.State);
            Assert.AreEqual(0, parent.Progress);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Idle));
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.Progress == 0));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteAndUndoTest()
        {
            //Arrange
            Random rand = new Random();

            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, State.Done);
            List<Quest> allHierarchy = new List<Quest>();

            QuestHelper.GatherAllHierarchy(parent, ref allHierarchy);

            int allHierarchyLength = allHierarchy.Count;

            double[] progresses = new double[allHierarchyLength];

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

           for(int i = 0; i < allHierarchyLength; ++i)
           {
               allHierarchy[i].Progress = progresses[i] = rand.NextDouble();
               int j = i;
               repository.Expect(r => r.LoadChildren(Arg<Quest>.Is.Equal(allHierarchy[j]))).
                    Repeat.Once();
               repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(allHierarchy[j]))).
                    Repeat.Once();
               repository.Expect(r => r.RevertUpdate(Arg<Quest>.Is.Equal(allHierarchy[j]))).
                    Repeat.Once();
                if (allHierarchy[j].Children != null && allHierarchy[j].Children.Count != 0)
                {
                    repository.Expect(r => r.UnloadChildren(Arg<Quest>.Is.Equal(allHierarchy[j]))).
                        Repeat.Once();
                }
            }

            CancelQuestCommand command = new CancelQuestCommand(parent, repository);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            for (int i = 0; i < allHierarchyLength; ++i)
            {
                Assert.AreEqual(progresses[i], allHierarchy[i].Progress);
            }
            Assert.AreEqual(State.Done, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Done));

            repository.VerifyAllExpectations();
        }
    }
}
