using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.EntityStateHandlers.VIewModels;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.View.Droid.UnitTests.EntityStateHandlersTest.ViewModels
{
    [TestFixture]
    class QuestCreateViewModelStateHandlerTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange && Act
            ArgumentNullException ex =
                Assert.Throws<ArgumentNullException>(() => new QuestCreateViewModelStateHandler(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("dateTimeStateHandler",ex.ParamName);
        }
    }
}
