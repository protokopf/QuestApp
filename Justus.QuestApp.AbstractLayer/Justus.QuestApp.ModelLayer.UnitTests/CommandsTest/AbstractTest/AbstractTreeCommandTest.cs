using System;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest
{
    [TestFixture]
    class AbstractTreeCommandTest
    {
        private class AbstractTreeMock : AbstractTreeCommand
        {
            public AbstractTreeMock(IQuestTree questTree) : base(questTree)
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

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new AbstractTreeMock(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questTree", ex.ParamName);
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.Save()).Repeat.Once();

            AbstractTreeMock mock = new AbstractTreeMock(tree);

            //Act
            bool commitResult = mock.Commit();

            //Assert
            Assert.IsTrue(commitResult);

            tree.VerifyAllExpectations();
        }
    }
}
