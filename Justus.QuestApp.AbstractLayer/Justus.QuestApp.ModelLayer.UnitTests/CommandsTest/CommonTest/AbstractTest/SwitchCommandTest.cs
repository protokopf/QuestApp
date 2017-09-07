using System;
using Justus.QuestApp.ModelLayer.Commands.Classic.Common.Abstracts;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest
{
    [TestFixture]
    internal class SwitchCommandTest
    {
        /// <summary>
        /// Help type for testing.
        /// </summary>
        private class SwitchCommandMock : SwitchCommand
        {
            public event Func<bool> OnInnerExecute;
            public event Func<bool> OnInnerUndo;
            public event Func<bool> OnInnerCommit; 


            protected override bool InnerExecute()
            {
                if (OnInnerExecute != null)
                {
                    return OnInnerExecute();
                }
                return false;
            }

            protected override bool InnerUndo()
            {
                if (OnInnerUndo != null)
                {
                    return OnInnerUndo();
                }
                return false;
            }

            protected override bool InnerCommit()
            {
                if (OnInnerCommit != null)
                {
                    return OnInnerCommit();
                }
                return false;
            }
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            bool hasInnerExecuted = false;

            SwitchCommandMock mock = new SwitchCommandMock();
            mock.OnInnerExecute += () =>
            {
                return hasInnerExecuted = true;
            };

            //Act
            bool executeResult = mock.Execute();

            //Assert
            Assert.IsTrue(executeResult);
            Assert.IsTrue(hasInnerExecuted);
        }

        [Test]
        public void ExecuteAfterCommitTest()
        {
            //Arrange
            bool hasInnerExecuted = false;

            SwitchCommandMock mock = new SwitchCommandMock();
            mock.OnInnerExecute += () =>
            {
                return hasInnerExecuted = true;
            };

            //Act
            mock.Commit();
            bool executeResult = mock.Execute();

            //Assert
            Assert.IsFalse(executeResult);
            Assert.IsFalse(hasInnerExecuted);
        }

        [Test]
        public void UndoWithoutExecuteTest()
        {
            //Arrange
            bool hasInnerUndo = false;

            SwitchCommandMock mock = new SwitchCommandMock();
            mock.OnInnerUndo += () =>
            {
                return hasInnerUndo = true;
            };

            //Act
            bool undoResult = mock.Undo();

            //Assert
            Assert.IsFalse(undoResult);
            Assert.IsFalse(hasInnerUndo);
        }

        [Test]
        public void UndoAfterExecuteTest()
        {
            //Arrange
            bool hasInnerExecute = false;
            bool hasInnerUndo = false;
            
            SwitchCommandMock mock = new SwitchCommandMock();
            mock.OnInnerExecute += () =>
            {
                return hasInnerExecute = true;
            };
            mock.OnInnerUndo += () =>
            {
                return hasInnerUndo = true;
            };

            //Act
            bool executeResult = mock.Execute();
            bool undoResult = mock.Undo();

            //Assert
            Assert.IsTrue(executeResult);
            Assert.IsTrue(undoResult);
            Assert.IsTrue(hasInnerExecute);
            Assert.IsTrue(hasInnerUndo);
        }

        [Test]
        public void UndoAfterExecuteCommitTest()
        {
            //Arrange
            bool hasInnerExecute = false;
            bool hasInnerUndo = false;

            SwitchCommandMock mock = new SwitchCommandMock();
            mock.OnInnerExecute += () =>
            {
                return hasInnerExecute = true;
            };
            mock.OnInnerUndo += () =>
            {
                return hasInnerUndo = true;
            };

            //Act
            bool executeResult = mock.Execute();
            mock.Commit();
            bool undoResult = mock.Undo();

            //Assert
            Assert.IsTrue(executeResult);
            Assert.IsFalse(undoResult);
            Assert.IsTrue(hasInnerExecute);
            Assert.IsFalse(hasInnerUndo);
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            SwitchCommandMock mock = new SwitchCommandMock();

            //Act
            bool isValid = mock.IsValid();

            //Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            bool hasInnerCommitExecuted = false;

            SwitchCommandMock mock = new SwitchCommandMock();
            mock.OnInnerCommit += () =>
            {
                return hasInnerCommitExecuted = true;
            };

            //Act
            bool commitResult = mock.Commit();

            //Assert
            Assert.IsTrue(hasInnerCommitExecuted);
            Assert.IsTrue(commitResult);
        }

        [Test]
        public void DoubleCommitTest()
        {
            //Arrange
            int innerCommitCallCounter = 0;

            SwitchCommandMock mock = new SwitchCommandMock();
            mock.OnInnerCommit += () =>
            {
                innerCommitCallCounter++;
                return true;
            };

            //Act
            bool firstCommitResult = mock.Commit();
            bool secondCommitResult = mock.Commit();

            //Assert
            Assert.AreEqual(1, innerCommitCallCounter);
            Assert.IsTrue(firstCommitResult);
            Assert.IsFalse(secondCommitResult);
        }
    }
}
