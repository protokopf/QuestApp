using Justus.QuestApp.ModelLayer.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Justus.QuestApp.ModelLayer.UnitTests.HelpersTest
{
    [TestFixture]
    class ServiceLocatorTest
    {
        #region Help types

        interface IAnotherFake
        {
            void GetSomethingAnother();
        }

        interface IFake
        {
            string GetSomething();
        }

        interface ICommon
        {
            void CommonAction();
        }

        class FakeOne : IFake
        {
            private string _str = string.Empty;

            public FakeOne()
            {

            }

            public FakeOne(string str)
            {
                _str = str;
            }

            public string GetSomething()
            {
                return _str;
            }
        }

        class FakeTwo : IFake
        {
            public string GetSomething()
            {
                return "FakeTwo";
            }
        }

        class FakeCommon : IFake, ICommon
        {
            public string GetSomething()
            {
                throw new NotImplementedException();
            }

            public void CommonAction()
            {
                throw new NotImplementedException();
            }
        }

        class AnotherFakeCommon : IAnotherFake, ICommon
        {
            public void GetSomethingAnother()
            {
                throw new NotImplementedException();
            }

            public void CommonAction()
            {
                throw new NotImplementedException();
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
            ServiceLocator.Register(() => new FakeOne());

            //Act
            IFake impl1 = ServiceLocator.Resolve<FakeOne>();

            //Assert
            Assert.IsNotNull(impl1);
            Assert.IsTrue(impl1 is FakeOne);
        }

        [Test]
        public void RegisterAndResolveByInterfaceTest()
        {
            //Arrange
            ServiceLocator.Register<IFake>(() => new FakeOne());

            //Act
            IFake impl1 = ServiceLocator.Resolve<IFake>();

            //Assert
            Assert.IsNotNull(impl1);
            Assert.IsTrue(impl1 is FakeOne);
        }

        [Test]
        public void RegisteredFuncReturnNullTest()
        {
            //Arrange
            ServiceLocator.Register<FakeOne>(() => { return null; });

            //Act
            IFake impl1 = ServiceLocator.Resolve<FakeOne>();

            //Assert
            Assert.IsNull(impl1);
        }

        [Test]
        public void RegisterTheSameTypeTwiceFailTest()
        {
            //Arrange
            bool first = ServiceLocator.Register<FakeOne>(() => new FakeOne("first"));
            bool second = ServiceLocator.Register<FakeOne>(() => new FakeOne("second"));

            //Act
            IFake impl1 = ServiceLocator.Resolve<FakeOne>();
            IFake impl2 = ServiceLocator.Resolve<FakeOne>();

            //Assert
            Assert.IsTrue(first);
            Assert.IsFalse(second);

            Assert.IsNotNull(impl1);
            Assert.IsNotNull(impl2);

            Assert.IsTrue(impl1 is FakeOne);
            Assert.IsTrue(impl2 is FakeOne);

            Assert.AreEqual("first", impl1.GetSomething());
            Assert.AreEqual("first", impl2.GetSomething());
        }

        [Test]
        public void RegisterTwoDifferentImplementationTest()
        {
            //Arrange
            bool first = ServiceLocator.Register<IFake>(() => new FakeOne());
            bool second = ServiceLocator.Register<IFake>(() => new FakeTwo());

            //Act
            IFake impl1 = ServiceLocator.Resolve<IFake>();
            IFake impl2 = ServiceLocator.Resolve<IFake>();

            //Assert
            Assert.IsTrue(first);
            Assert.IsFalse(second);

            Assert.IsNotNull(impl1);
            Assert.IsNotNull(impl2);

            Assert.IsTrue(impl1 is FakeOne);
            Assert.IsTrue(impl2 is FakeOne);
        }

        [Test]
        public void ResolveByImplButRegisterByInterfaceFailTest()
        {
            //Arrange
            ServiceLocator.Register<IFake>(() => new FakeTwo());

            //Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => ServiceLocator.Resolve<FakeTwo>());

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual($"Service {typeof(FakeTwo)} not found!", ex.Message);
        }

        [Test]
        public void ResolveNotRegisteredFailTest()
        {
            //Arrange

            //Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => ServiceLocator.Resolve<FakeOne>());

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual($"Service {typeof(FakeOne)} not found!", ex.Message);
        }

        [Test]
        public void ResolveAllSuccessfulTest()
        {
            //Arrange
            ServiceLocator.Register(() => new FakeCommon());
            ServiceLocator.Register(() => new FakeOne());
            ServiceLocator.Register(() => new AnotherFakeCommon());

            //Act
            List<ICommon> implementations = ServiceLocator.ResolveAll<ICommon>().ToList();

            //Assert
            Assert.IsNotNull(implementations);
            Assert.AreEqual(2, implementations.Count);

            Assert.IsTrue(implementations.Any(x => x.GetType() == typeof(FakeCommon)));
            Assert.IsTrue(implementations.Any(x => x.GetType() == typeof(AnotherFakeCommon)));
        }

        [Test]
        public void ResolveAllButNothingTest()
        {
            //Arrange
            ServiceLocator.Register(() => new FakeTwo());
            ServiceLocator.Register(() => new FakeOne());

            //Act
            List<ICommon> implementations = ServiceLocator.ResolveAll<ICommon>().ToList();

            //Assert
            Assert.IsNotNull(implementations);
            Assert.IsEmpty(implementations);
        }
    }
}
