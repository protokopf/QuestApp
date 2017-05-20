using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.ModelLayer.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.HelpersTest
{
    [TestFixture]
    class DefaultDataKeeperTest
    {
        #region Helpers 

        private class SomeType
        {
            public int ValueField { get; set; }

            public Object ObjectField { get; set; }
        }

        #endregion

        [TestCase(null)]
        [TestCase("")]
        [TestCase("\t\t")]
        public void KeepInvalidKeyStringTest(string key)
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            object data = new object();

            //Act
            ArgumentException ex = Assert.Throws<ArgumentException>(() => keeper.Keep(key, data));

            //Assert
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("Key should be valid non-empty string."));
        }

        [Test]
        public void KeepNullDataTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => keeper.Keep<Object>(key, null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("data", ex.ParamName);
        }

        [Test]
        public void KeepDefaultValueTypeTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";
            int data = default(int);

            bool wasKeeped = false;

            //Act
            Assert.DoesNotThrow(() => wasKeeped = keeper.Keep(key, data));

            //Assert
            Assert.IsTrue(wasKeeped);
        }

        [Test]
        public void KeepReferenceTypeSuccessfulTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            SomeType someData = new SomeType
            {
                ObjectField = new Object(),
                ValueField = 42
            };

            bool wasKeeped = false;

            //Act
            Assert.DoesNotThrow(() => wasKeeped = keeper.Keep(key, someData));

            //Assert
            Assert.IsTrue(wasKeeped);
        }

        [Test]
        public void KeepTwoValuesWithSameKeysTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            SomeType someData = new SomeType
            {
                ObjectField = new Object(),
                ValueField = 42
            };

            //Type here does not play any role. Keys equality is important.
            int anotherSomeData = 42;

            //Act
            bool wasKeepedFirst = keeper.Keep(key, someData);
            bool wasKeepedSecond = keeper.Keep(key, anotherSomeData);

            //Assert
            Assert.IsTrue(wasKeepedFirst);
            Assert.IsFalse(wasKeepedSecond);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("\t\t")]
        public void GetInvalidKeyStringTest(string key)
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            object data = new object();

            //Act
            ArgumentException ex = Assert.Throws<ArgumentException>(() => keeper.Get<Object>(key));

            //Assert
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("Key should be valid non-empty string."));
        }

        [Test]
        public void GetNonKeepedValueTypeTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            //Act
            int nonKeepedValue = keeper.Get<int>(key);

            //Assert
            Assert.AreEqual(0, nonKeepedValue);
        }

        [Test]
        public void GetNonKeepedReferenceTypeTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            //Act
            SomeType nonKeepedData = keeper.Get<SomeType>(key);

            //Assert
            Assert.IsNull(nonKeepedData);
        }

        [Test]
        public void GetKeepedButOtherTypeTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            SomeType someType = new SomeType();

            keeper.Keep(key, someType);

            //Act
            int keepedValue = keeper.Get<int>(key);

            //Assert
            Assert.AreEqual(0, keepedValue);
        }

        [Test]
        public void GetKeepedValueTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            SomeType someData = new SomeType
            {
                ObjectField = new Object(),
                ValueField = 42
            };

            keeper.Keep(key, someData);

            //Act
            SomeType keepedData = keeper.Get<SomeType>(key);

            //Assert
            Assert.IsNotNull(keepedData);
            Assert.AreEqual(someData, keepedData);
            Assert.AreEqual(someData.ObjectField, keepedData.ObjectField);
            Assert.AreEqual(someData.ValueField, keepedData.ValueField);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("\t\t")]
        public void DeleteInvalidKeyStringTest(string key)
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();

            //Act
            ArgumentException ex = Assert.Throws<ArgumentException>(() => keeper.Delete(key));

            //Assert
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("Key should be valid non-empty string."));
        }

        [Test]
        public void DeleteNonKeepedValueTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";

            //Act
            bool wasDeleted = keeper.Delete(key);

            //Assert
            Assert.IsFalse(wasDeleted);
        }

        [Test]
        public void DeleteKeepedValueTest()
        {
            //Arrange
            DefaultDataKeeper keeper = new DefaultDataKeeper();
            string key = "someKey";
            int value = 42;

            //Act
            keeper.Keep(key, value);

            int beforeDelete = keeper.Get<int>(key);
            bool wasDeleted = keeper.Delete(key);
            int afterDelete = keeper.Get<int>(key);

            //Assert
            Assert.AreEqual(value, beforeDelete);
            Assert.IsTrue(wasDeleted);
            Assert.AreEqual(0, afterDelete);
        }

    }
}
