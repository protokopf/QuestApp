using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Classic.Common.Abstracts;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest
{
    [TestFixture]
    class BaseStateUpdateCommandTest
    {
        private class MockCommand : BaseQuestCommand
        {
            public MockCommand(Quest quest, IQuestTree tree) : base(quest, tree)
            {
            }

            protected override bool InnerExecute()
            {
                throw new NotImplementedException();
            }

            protected override bool InnerUndo()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new MockCommand(null, tree));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }
    }
}
