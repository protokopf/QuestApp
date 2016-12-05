using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.ModelLayer.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ModelLayer.UnitTests.HelpersTest
{
    [TestFixture]
    class TaskWrapperTest
    {
        [Test]
        public void WrapNullTest()
        {
            //Arrange
            ITaskWrapper wrapper = new TaskWrapper();
            Action action = null;

            //Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => wrapper.Wrap(action));

            //Assert
            Assert.IsNotNull(exception);
            Assert.AreEqual("actionToWrap", exception.ParamName);
        }

        [Test]
        public void WrapWithResultTest()
        {
            //Arrange
            ITaskWrapper wrapper = new TaskWrapper();
            Func<int> func = null;

            //Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => wrapper.Wrap(func));

            //Assert
            Assert.IsNotNull(exception);
            Assert.AreEqual("functionWithResult", exception.ParamName);
        }

        [Test]
        public void WrapActionTest()
        {
            //Arrange
            ITaskWrapper wrapper = new TaskWrapper();
            int result = 0;

            //Act
            Task task = wrapper.Wrap(() => result = 2 + 2);

            //Assert
            Assert.IsNotNull(task);

            task.Start();
            task.Wait();

            Assert.AreEqual(4, result);
        }

        [Test]
        public void WrapFuncTest()
        {
            //Arrange
            ITaskWrapper wrapper = new TaskWrapper();

            //Act
            Task<int> task = wrapper.Wrap(() => { return 2 + 2; });

            //Assert
            Assert.IsNotNull(task);

            task.Start();
            task.Wait();

            int result = task.Result;

            Assert.AreEqual(4, result);
        }
    }
}
