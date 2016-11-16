using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
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
    class BindedQuestsDataStorageTest
    {

        #region Help methods

        private Quest CreateQuest(int id = 0)
        {
            return new FakeQuest()
            {
                Id = id,
                Title = "title",
                Description = "description",                
                CurrentState = QuestState.Idle,
                Children = new List<Quest>()              
            };
        }

        private Quest CreateCompositeQuestFromAbove(int compositionLevel, int childNumber)
        {
            Quest quest = CreateQuest();          
            if (compositionLevel == 0 || childNumber == 0)
            {
                return quest;
            }
            for (int i = 0; i < childNumber; ++i)
            {
                Quest child = CreateCompositeQuestFromAbove(compositionLevel - 1, childNumber);
                child.Parent = quest;
                quest.Children.Add(child);
            }
            return quest;
        }

        private List<Quest> CreateCompositeQuestFromBelow(int compositionLevel, int childNumber, int startId = 1)
        {
            List<Quest> result = new List<Quest>();

            while (compositionLevel != 0)
            {
                Quest quest = CreateQuest(startId);
                result.Add(quest);

                for (int i = 0; i < childNumber; ++i)
                {
                    Quest child = CreateQuest(++startId);
                    result.Add(child);
                    child.ParentId = quest.Id;
                }
                compositionLevel--;
                startId++;
            }
            return result;
        }

        private int CountSubQuests(List<Quest> childs)
        {
            if (childs == null || childs.Count == 0)
            {
                return 0;
            }
            int totalCount = childs.Count;
            foreach (Quest child in childs)
            {
                totalCount += CountSubQuests(child.Children);
            }
            return totalCount;
        }

        private int CountExpectedSubQuests(int depth, int child)
        {
            int total = 0;
            if (depth == 0 || child == 0)
            {
                return total;
            }      
            do
            {
                total += child;
                child = (int) Math.Pow(child, 2);
                --depth;
            } while (depth != 0);

            return total;
        }

        private List<Quest> CreateQuests(int count)
        {
            List<Quest> quests = new List<Quest>();
            for (int i = 0; i < count; ++i)
            {
                quests.Add(CreateQuest(i + 1));
            }
            return quests;
        }

        #endregion

        [Test]
        public void ConstructorArgumentNullTest()
        {
            //Arrange
            IDataAccessInterface<Quest> dl;

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => dl = new BindedQuestsDataStorage(null));

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
            FakeQuestStorage inner = new FakeQuestStorage();

            IDataAccessInterface<Quest> dl = new BindedQuestsDataStorage(inner);

            //Act
            ArgumentException ex = Assert.Throws<ArgumentException>(() => dl.Open(pathToStorage));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("Path should not be empty or null string.", ex.Message);

            Assert.IsNull(inner.QuestStorage);
        }

        [Test]
        public void IsClosedTest()
        {
            //Arrange
            string fakePath = "path,Baby";

            IDataAccessInterface<Quest> inner = new FakeQuestStorage();

            IDataAccessInterface<Quest> dl = new BindedQuestsDataStorage(inner);

            //Act
            dl.Open(fakePath);
            bool afterOpenClosed = dl.IsClosed();
            dl.Close();
            bool afterCloseClosed = dl.IsClosed();

            //Assert
            Assert.IsFalse(afterOpenClosed);
            Assert.IsTrue(afterCloseClosed);
        }

        [Test]
        public void InsertWhenQuestHaveChildrenTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            Quest quest = CreateCompositeQuestFromAbove(2,3);
            int expectedStoredQuests = CountExpectedSubQuests(2, 3) + 1;
            
            //Act
            bindedDs.Open("does not metter");
            bindedDs.Insert(quest);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void InsertAndCheckBindingAndIdSettingTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            Quest quest = CreateCompositeQuestFromAbove(1, 5);
            int expectedStoredQuests = CountExpectedSubQuests(1, 5) + 1;

            //Act
            bindedDs.Open("does not metter");
            bindedDs.Insert(quest);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);

            List<Quest> rootQuests = storedItems.FindAll(x => x.ParentId == 0);
            Assert.IsNotNull(rootQuests);
            Assert.AreEqual(1, rootQuests.Count);

            Quest root = rootQuests[0];
            Assert.IsNotNull(root);

            List<Quest> children = storedItems.FindAll(x => x.ParentId != 0);
            Assert.IsNotNull(children);
            Assert.AreEqual(expectedStoredQuests - 1, children.Count);

            foreach (Quest child in children)
            {
                Assert.IsTrue(child.Id != 0);
                Assert.AreEqual(root.Id, child.ParentId);
            }

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void InsertAllWhenQuestsHaveChildrenTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            int questsCount = 5;

            List<Quest> quests = new List<Quest>();

            for (int i = 0; i < questsCount; ++i)
            {
                quests.Add(CreateCompositeQuestFromAbove(2, 3));
            }
            int expectedStoredQuests = (CountExpectedSubQuests(2, 3) + 1)*questsCount;

            //Act
            bindedDs.Open("does not metter");
            bindedDs.InsertAll(quests);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void InitializeIdWithoutStoredQuestsbTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            Quest quest = CreateQuest();

            //Act
            bindedDs.Open("does not metter");
            bindedDs.Insert(quest);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(1, storedItems.Count);
            Assert.AreEqual(1, storedItems[0].Id);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void InitializeIdWithStoredQuestsTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = CreateQuests(10);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            Quest quest = CreateQuest();
            quest.Title = "New one";

            //Act
            bindedDs.Open("does not metter");
            bindedDs.Insert(quest);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(11, storedItems.Count);

            Quest newOne = storedItems.Find(x => x.Id == 11);
            Assert.IsNotNull(newOne);
            Assert.AreEqual("New one", newOne.Title);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void InsertInStorageWithItemsTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = CreateQuests(10);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            Quest quest = CreateCompositeQuestFromAbove(2, 3);
            int expectedStoredQuests = CountExpectedSubQuests(2, 3) + 1 + 10;

            //Act
            bindedDs.Open("does not metter");
            bindedDs.Insert(quest);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void UpdateTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = CreateQuests(10);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            Quest updatedItem = fakeStorage.QuestStorage[3];
            updatedItem.Title = "New title";

            bindedDs.Open("does not metter");
            bindedDs.Update(updatedItem);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);
            Assert.IsNotNull(storedItems.Find(x => (x.Title == "New title") && (x.Id == 4)));

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void UpdateTestWhichHasNewChildButNothingChanges()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = CreateQuests(10);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            Quest updatedItem = fakeStorage.QuestStorage[3];
            Quest addedItem = CreateQuest();
            addedItem.Title = "Added item";
            updatedItem.Children.Add(addedItem);

            bindedDs.Open("does not metter");
            bindedDs.Update(updatedItem);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);

            Quest added = storedItems.Find(x => (x.Title == "Added item"));

            Assert.IsNull(added);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void SimpleUpdateTest()
        {
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = CreateQuests(10);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            Quest updatedItem = fakeStorage.QuestStorage[3];
            updatedItem.Title = "Updated item";
            int updatedId = updatedItem.Id;

            bindedDs.Open("does not metter");
            bindedDs.Update(updatedItem);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);

            Quest found = storedItems.Find(x => (x.Title == "Updated item"));

            Assert.IsNotNull(found);
            Assert.AreEqual(updatedId, found.Id);
            Assert.AreEqual("Updated item", found.Title);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void GetMethodReturnBindedQuestTest()
        {
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = CreateCompositeQuestFromBelow(1, 5, 1);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            bindedDs.Open("does not metter");
            Quest found = bindedDs.Get(1);

            //Assert
            Assert.IsNotNull(found);
            Assert.AreEqual(1,found.Id);
            Assert.AreEqual(0,found.ParentId);

            Assert.IsNotNull(found.Children);
            Assert.IsNull(found.Parent);

            Assert.AreEqual(5, found.Children.Count);
            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(found, found.Children[i].Parent);
            }

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void GetAllMethodReturnsAllBindedQuestsTest()
        {
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = new List<Quest>();
            for (int i = 0; i < 4; ++i)
            {
                int id = i*6 + 1;
                fakeStorage.QuestStorage.AddRange(CreateCompositeQuestFromBelow(1, 5, id));
            }

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            bindedDs.Open("does not metter");
            List<Quest> founds = bindedDs.GetAll();

            //Assert
            Assert.IsNotNull(founds);
            Assert.AreEqual(4, founds.Count);

            for (int i = 0; i < 4; ++i)
            {
                Quest found = founds[i];

                Assert.IsNotNull(found.Children);
                Assert.IsNull(found.Parent);

                Assert.AreEqual(5, found.Children.Count);
                for (int j = 0; j < 5; ++j)
                {
                    Assert.AreEqual(found, found.Children[i].Parent);
                }
            }

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void GetAllInRowUsesCachedLinesTest()
        {
            FakeQuestStorage fakeStorage = MockRepository.GeneratePartialMock<FakeQuestStorage>();
            fakeStorage.Stub(x => x.GetAll()).Repeat.Once();

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            bindedDs.Open("does not metter");
            List<Quest> founds = bindedDs.GetAll();
            bindedDs.GetAll();

            //Assert
            Assert.IsNotNull(founds);
            Assert.IsEmpty(founds);

            fakeStorage.VerifyAllExpectations();
        }

        [Test]
        public void DeleteTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = new List<Quest>();
            for (int i = 0; i < 4; ++i)
            {
                int id = i * 6 + 1;
                fakeStorage.QuestStorage.AddRange(CreateCompositeQuestFromBelow(1, 5, id));
            }

            int count = CountSubQuests(fakeStorage.QuestStorage);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            bindedDs.Open("does not metter");
            bindedDs.Delete(7);

            List<Quest> fromLower = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(fromLower);
            Assert.AreEqual(count - 6, fromLower.Count);

            foreach (Quest quest in fromLower)
            {
                Assert.AreNotEqual(7, quest.ParentId);
                Assert.AreNotEqual(7, quest.Id);
            }

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void DeleteAllTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = CreateCompositeQuestFromBelow(1, 5);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            bindedDs.Open("does not metter");
            bindedDs.DeleteAll();

            List<Quest> fromLower = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(fromLower);
            Assert.IsEmpty(fromLower);

            //Clean-up
            bindedDs.Close();
        }

        [Test]
        public void GetAllAfterDeleteAllTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = MockRepository.GeneratePartialMock<FakeQuestStorage>();
            fakeStorage.Expect(x => x.GetAll()).Repeat.Once();
            fakeStorage.QuestStorage = CreateCompositeQuestFromBelow(1, 5);

            IDataAccessInterface<Quest> bindedDs = new BindedQuestsDataStorage(fakeStorage);

            //Act
            bindedDs.Open("does not metter");
            bindedDs.DeleteAll();

            List<Quest> allNow = bindedDs.GetAll();
            List<Quest> fromLower = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(fromLower);
            Assert.IsEmpty(fromLower);

            Assert.IsNotNull(allNow);
            Assert.IsEmpty(allNow);

            fakeStorage.VerifyAllExpectations();
        }

        //TO DO: Get all binds all parent-child quests test.
    }
}
