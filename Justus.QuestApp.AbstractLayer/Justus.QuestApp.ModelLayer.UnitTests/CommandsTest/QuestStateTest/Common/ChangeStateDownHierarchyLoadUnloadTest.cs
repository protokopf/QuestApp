using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State.Common;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest.Common
{
    [TestFixture]
    class ChangeStateDownHierarchyLoadUnloadTest
    {
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
                repository.Expect(r => r.LoadChildren(Arg<Quest>.Is.Equal(quest))).
                    Repeat.Once();
                repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(quest))).
                    Repeat.Once();
            }


            ChangeStateDownHierarchyLoadUnload command = new ChangeStateDownHierarchyLoadUnload(parent,  repository, State.Idle);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(State.Idle, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Idle));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteAndCommitTest()
        {
            //Arrange
            Quest parent = QuestHelper.CreateCompositeQuest(2, 2, State.Progress);
            List<Quest> allHierarchy = new List<Quest>();
            QuestHelper.GatherAllHierarchy(parent, ref allHierarchy);

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(r => r.Save()).
                Repeat.Once();

            foreach (Quest quest in allHierarchy)
            {
                repository.Expect(r => r.LoadChildren(Arg<Quest>.Is.Equal(quest))).
                    Repeat.Once();
                repository.Expect(r => r.Update(Arg<Quest>.Is.Equal(quest))).
                    Repeat.Once();
                if (quest.Children != null && quest.Children.Count != 0)
                {
                    repository.Expect(r => r.UnloadChildren(Arg<Quest>.Is.Equal(quest))).
                        Repeat.Once();
                }

            }

            ChangeStateDownHierarchyLoadUnload command = new ChangeStateDownHierarchyLoadUnload(parent, repository, State.Idle);

            //Act
            command.Execute();
            bool commitResult = command.Commit();

            //Assert
            Assert.IsTrue(commitResult);
            Assert.AreEqual(State.Idle, parent.State);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(parent.Children, q => q.State == State.Idle));

            repository.VerifyAllExpectations();
        }
    }
}
