using Justus.QuestApp.ModelLayer.Helpers;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ModelLayer.UnitTests.HelpersTest
{
    [TestFixture]
    class ServiceLocatorTest
    {
        #region Help types

        interface FakeInterface
        {
            string GetSomething();
        }

        class FakeImplementationOne : FakeInterface
        {
            private string _str = string.Empty;

            public FakeImplementationOne()
            {

            }

            public FakeImplementationOne(string str)
            {
                _str = str;
            }

            public string GetSomething()
            {
                return _str;
            }
        }

        class FakeImplementationTwo : FakeInterface
        {
            public string GetSomething()
            {
                return "FakeImplementationTwo";
            }
        }

        #endregion

        [TearDown]
        public void TearDown()
        {
            ServiceLocator.ReleaseAll();
        }

        [Test]
        public void RegisterAndResolveTest()
        {
            //Arrange
            ServiceLocator.Register(() => new FakeImplementationOne());

            //Act
            FakeInterface impl1 = ServiceLocator.Resolve<FakeImplementationOne>();

            //Assert
            Assert.IsNotNull(impl1);
            Assert.IsTrue(impl1 is FakeImplementationOne);
        }

        [Test]
        public void RegisterAndResolveByInterfaceTest()
        {
            //Arrange
            ServiceLocator.Register<FakeInterface>(() => new FakeImplementationOne());

            //Act
            FakeInterface impl1 = ServiceLocator.Resolve<FakeInterface>();

            //Assert
            Assert.IsNotNull(impl1);
            Assert.IsTrue(impl1 is FakeImplementationOne);
        }

        [Test]
        public void RegisteredFuncReturnNullTest()
        {
            //Arrange
            ServiceLocator.Register<FakeImplementationOne>(() => { return null; });

            //Act
            FakeInterface impl1 = ServiceLocator.Resolve<FakeImplementationOne>();

            //Assert
            Assert.IsNull(impl1);
        }

        [Test]
        public void RegisterTheSameTypeTwiceFailTest()
        {
            //Arrange
            bool first = ServiceLocator.Register<FakeImplementationOne>(() => new FakeImplementationOne("first"));
            bool second = ServiceLocator.Register<FakeImplementationOne>(() => new FakeImplementationOne("second"));

            //Act
            FakeInterface impl1 = ServiceLocator.Resolve<FakeImplementationOne>();
            FakeInterface impl2 = ServiceLocator.Resolve<FakeImplementationOne>();

            //Assert
            Assert.IsTrue(first);
            Assert.IsFalse(second);

            Assert.IsNotNull(impl1);
            Assert.IsNotNull(impl2);

            Assert.IsTrue(impl1 is FakeImplementationOne);
            Assert.IsTrue(impl2 is FakeImplementationOne);

            Assert.AreEqual("first", impl1.GetSomething());
            Assert.AreEqual("first", impl2.GetSomething());
        }

        [Test]
        public void RegisterTwoDifferentImplementationTest()
        {
            //Arrange
            bool first = ServiceLocator.Register<FakeInterface>(() => new FakeImplementationOne());
            bool second = ServiceLocator.Register<FakeInterface>(() => new FakeImplementationTwo());

            //Act
            FakeInterface impl1 = ServiceLocator.Resolve<FakeInterface>();
            FakeInterface impl2 = ServiceLocator.Resolve<FakeInterface>();

            //Assert
            Assert.IsTrue(first);
            Assert.IsFalse(second);

            Assert.IsNotNull(impl1);
            Assert.IsNotNull(impl2);

            Assert.IsTrue(impl1 is FakeImplementationOne);
            Assert.IsTrue(impl2 is FakeImplementationOne);
        }

        [Test]
        public void ResolveByImplButRegisterByInterfaceFailTest()
        {
            //Arrange
            ServiceLocator.Register<FakeInterface>(() => new FakeImplementationTwo());

            //Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => ServiceLocator.Resolve<FakeImplementationTwo>());

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual($"Service {typeof(FakeImplementationTwo)} not found!", ex.Message);
        }

        [Test]
        public void ResolveNotRegisteredFailTest()
        {
            //Arrange

            //Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => ServiceLocator.Resolve<FakeImplementationOne>());

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual($"Service {typeof(FakeImplementationOne)} not found!", ex.Message);
        }
    }
}
