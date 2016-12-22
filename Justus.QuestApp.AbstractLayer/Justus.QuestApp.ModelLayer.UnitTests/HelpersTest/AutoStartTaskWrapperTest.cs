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
    class AutoStartTaskWrapperTest
    {
        [Test]
        public void WrapNullTest()
        {
            //Arrange
            ITaskWrapper wrapper = new AutoStartTaskWrapper();
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
            ITaskWrapper wrapper = new AutoStartTaskWrapper();
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
            ITaskWrapper wrapper = new AutoStartTaskWrapper();
            int result = 0;

            //Act
            Task task = wrapper.Wrap(() => result = 2 + 2);

            Task.WaitAll();

            //Assert
            Assert.IsNotNull(task);

            Assert.AreEqual(4, result);
        }

        [Test]
        public void WrapFuncTest()
        {
            //Arrange
            ITaskWrapper wrapper = new AutoStartTaskWrapper();

            //Act
            Task<int> task = wrapper.Wrap(() => { return 2 + 2; });

            Task.WaitAll();

            //Assert
            Assert.IsNotNull(task);

            int result = task.Result;

            Assert.AreEqual(4, result);
        }
    }
}
