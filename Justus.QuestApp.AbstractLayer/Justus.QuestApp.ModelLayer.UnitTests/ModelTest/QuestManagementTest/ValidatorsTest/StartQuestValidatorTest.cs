using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.Model.QuestManagement;
using Justus.QuestApp.ModelLayer.Model.QuestManagement.Validators;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest.QuestManagementTest.ValidatorsTest
{
    [TestFixture]
    class StartQuestValidatorTest
    {
        [Test]
        public void ValidateProgressQuestTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Progress);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("You cannot start quest in progress!", result.Errors[0]);
        }

        [Test]
        public void ValidateDoneQuestTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Done);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("You cannot start the quest already done!", result.Errors[0]);
        }

        [Test]
        public void ValidateFailedQuestTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Failed);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("You cannot start the failed quest!", result.Errors[0]);
        }

        [Test]
        public void ValidateReadyQuestWithChildren()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Ready);
            quest.Children.Add(QuestHelper.CreateQuest(QuestState.Ready));

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("You can start only the most nested quest!", result.Errors[0]);
        }
    }
}
