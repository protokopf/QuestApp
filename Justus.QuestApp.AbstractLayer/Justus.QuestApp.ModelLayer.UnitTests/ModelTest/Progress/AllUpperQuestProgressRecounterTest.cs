using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Model.Progress;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest.Progress
{
    [TestFixture]
    class AllUpperQuestProgressRecounterTest
    {
        [Test]
        public void QuestNullTest()
        {
            //Arrange
            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();

            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter(repository);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => recounter.RecountProgress(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [TestCase(State.Progress)]
        [TestCase(State.Done)]
        [TestCase(State.Failed)]
        [TestCase(State.Idle)]
        public void QuestWithoutParentTest(State state)
        {
            //Arrange
            Quest q = QuestHelper.CreateQuest(state);
            q.Parent = null;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(rp => rp.Update(Arg<Quest>.Is.Equal(q))).Repeat.Once();

            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter(repository);


            //Act
            recounter.RecountProgress(q);

            //Assert
            Assert.AreEqual(state == State.Done ? 1 : 0, q.Progress);
            repository.VerifyAllExpectations();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void QuestWithOneParentWithoutSiblingsTest(bool isQuestDone)
        {
            //Arrange
            double progress = isQuestDone ? 1 : 0;
            Quest parent = QuestHelper.CreateQuest();
            parent.Children = new List<Quest>()
            {
                QuestHelper.CreateQuest()
            };

            Quest onlyChild = parent.Children[0];
            onlyChild.Progress = progress;


            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(rp => rp.Update(Arg<Quest>.Is.Equal(parent))).Repeat.Once();
            repository.Expect(rp => rp.Update(Arg<Quest>.Is.Equal(onlyChild))).Repeat.Once();

            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter(repository);


            //Act
            recounter.RecountProgress(parent);

            //Assert
            Assert.AreEqual(progress, parent.Progress);
        }

        [TestCase(new double[] { 0, 0, 0})]
        [TestCase(new double[] { 0, 0, 1 })]
        [TestCase(new double[] { 0, 1, 1 })]
        [TestCase(new double[] { 1, 1, 1 })]
        public void QuestWithParentAndSiblingsTest(double[] progresses)
        {
            //Arrange
            int count = progresses.Length;

            Quest parent = QuestHelper.CreateQuest();
            parent.Children = new List<Quest>();

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(rp => rp.Update(Arg<Quest>.Is.Equal(parent))).Repeat.Once();
            
            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter(repository);

            for (int i = 0; i < count; ++i)
            {
                Quest q = new Quest() {Progress = progresses[i]};
                parent.Children.Add(q);
                repository.Expect(rp => rp.Update(Arg<Quest>.Is.Equal(q))).Repeat.Once();
            }

            //Act
            recounter.RecountProgress(parent);

            //Assert
            double expectedProgress = progresses.Average(d => d);
            Assert.AreEqual(expectedProgress, parent.Progress);
        }
    }
}
