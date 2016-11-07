using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.DataLayer.Data;
using Justus.QuestApp.DataLayer.UnitTests.Data.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.DataLayer.UnitTests.Data
{
    [TestFixture]
    class GeneralQuestStorageTests
    {
        [Test]
        public void ConstructorArgumentNullTest()
        {
            //Arrange
            IDataAccessInterface<Quest> dl;

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => dl = new GeneralQuestStorage(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("dataStorage", ex.ParamName);
        }

        [TestCase("   ")]
        [TestCase(null)]
        [TestCase("")]
        public void PathToStorageFailTest(string pathToStorage)
        {
            //Arrange
            IDataAccessInterface<Quest> inner = MockRepository.GenerateStrictMock<IDataAccessInterface<Quest>>();
            inner.Expect(x => x.Open(Arg<string>.Is.Anything)).Repeat.Never();

            IDataAccessInterface<Quest> dl = new GeneralQuestStorage(inner);

            //Act
            ArgumentException ex = Assert.Throws<ArgumentException>(() => dl.Open(pathToStorage));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("Path should not be empty or null string.", ex.Message);
            inner.VerifyAllExpectations();
        }

        [Test]
        public void IsClosedTest()
        {
            //Arrange
            string fakePath = "path,Baby";
            bool hasBeenClosed = true;

            IDataAccessInterface<Quest> inner = MockRepository.GenerateStrictMock<IDataAccessInterface<Quest>>();
            
            inner.Expect(x => x.Open(null)).IgnoreArguments().Repeat.Once().Do(new Action<string>(s =>
            {
                hasBeenClosed = false;
            }));
            inner.Expect(x => x.Close()).Repeat.Once().Do(new Action(() =>
            {
                hasBeenClosed = true;
            }));
            inner.Expect(x => x.IsClosed()).Repeat.Twice().Do(new Func<bool>(() => hasBeenClosed));

            IDataAccessInterface<Quest> dl = new GeneralQuestStorage(inner);

            //Act
            dl.Open(fakePath);
            bool afterOpenClosed = dl.IsClosed();
            dl.Close();
            bool afterCloseClosed = dl.IsClosed();

            //Assert
            Assert.IsFalse(afterOpenClosed);
            Assert.IsTrue(afterCloseClosed);
            inner.VerifyAllExpectations();
        }

        [Test]
        public void InsertTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            //Act

            //Assert
        }
    }
}
