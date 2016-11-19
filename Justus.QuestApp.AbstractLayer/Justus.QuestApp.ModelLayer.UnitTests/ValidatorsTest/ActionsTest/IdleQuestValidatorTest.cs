﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.Validators.Actions;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ValidatorsTest.ActionsTest
{
    [TestFixture]
    class IdleQuestValidatorTest
    {
        [Test]
        public void NullQuestTest()
        {
            //Arrange
            IQuestValidator validator = new IdleQuestValidator();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => validator.Validate(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [TestCase(QuestState.Idle)]
        public void ValidateQuestWithWrongStateTest(QuestState state)
        {
            //Arrange
            IQuestValidator validator = new IdleQuestValidator();
            Quest quest = QuestHelper.CreateQuest(state);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_WRONG_STATE", result.Errors[0]);
        }

        [TestCase(QuestState.Failed)]
        [TestCase(QuestState.Progress)]
        [TestCase(QuestState.Done)]
        public void ValidateQuestSuccessfulTest(QuestState state)
        {
            //Arrange
            IQuestValidator validator = new IdleQuestValidator();
            Quest quest = QuestHelper.CreateQuest(state);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}