using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest
{
    [TestFixture]
    class RecursiveQuestRepositoryTest
    {
        [Test]
        public void ObjectCreationFailsDueToStorageReferenceTest()
        {
            //Arrange
            string connString = "123";

            //Act
            ArgumentNullException ex =
                Assert.Throws<ArgumentNullException>(() => new RecursiveQuestRepository(null, connString));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("dataStorageInterface", ex.ParamName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t")]
        public void ObjectCreationFailsDueToConnStringTest(string connectionString)
        {
            //Arrange
            IDataAccessInterface<Quest> mockStorage = MockRepository.GenerateMock<IDataAccessInterface<Quest>>();

            //Act
            ArgumentException ex =
                Assert.Throws<ArgumentException>(() => new RecursiveQuestRepository(mockStorage, connectionString));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("Connection string should not be null or empty.", ex.Message);
        }

        [Test]
        public void InsertWhenQuestHaveChildrenTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage,"no matter");

            Quest quest = QuestHelper.CreateCompositeQuestFromAbove(2, 3);
            int expectedStoredQuests = QuestHelper.CountExpectedSubQuests(2, 3) + 1;

            //Act
            repository.Insert(quest);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);
        }

        [Test]
        public void InsertAndCheckBindingAndIdSettingTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            Quest quest = QuestHelper.CreateCompositeQuestFromAbove(1, 5);
            int expectedStoredQuests = QuestHelper.CountExpectedSubQuests(1, 5) + 1;

            //Act
            repository.Insert(quest);
            repository.PushQuests();

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
        }

        [Test]
        public void InsertNullTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => repository.Insert(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quest");
        }

        [Test]
        public void InsertAllNullTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => repository.InsertAll(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quests");
        }

        [Test]
        public void InsertAllWhenQuestsHaveChildrenTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            int questsCount = 5;

            List<Quest> quests = new List<Quest>();

            for (int i = 0; i < questsCount; ++i)
            {
                quests.Add(QuestHelper.CreateCompositeQuestFromAbove(2, 3));
            }
            int expectedStoredQuests = (QuestHelper.CountExpectedSubQuests(2, 3) + 1) * questsCount;

            //Act
            repository.InsertAll(quests);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);
        }

        [Test]
        public void RevertInsertTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            Quest quest = QuestHelper.CreateCompositeQuestFromAbove(1, 0);

            //Act
            repository.Insert(quest);
            bool revertResult = repository.RevertInsert(quest);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsTrue(revertResult);
            Assert.IsNull(storedItems);
        }

        [Test]
        public void InitializeIdWithoutStoredQuestsbTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            Quest quest = QuestHelper.CreateQuest();

            //Act
            repository.Insert(quest);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(1, storedItems.Count);
            Assert.AreEqual(1, storedItems[0].Id);
        }

        [Test]
        public void InitializeIdWithStoredQuestsTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateQuests(10);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            Quest quest = QuestHelper.CreateQuest();
            quest.Title = "New one";

            //Act
            repository.PullQuests();
            repository.Insert(quest);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(11, storedItems.Count);

            Quest newOne = storedItems.Find(x => x.Id == 11);
            Assert.IsNotNull(newOne);
            Assert.AreEqual("New one", newOne.Title);
        }

        [Test]
        public void InsertInStorageWithItemsTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateQuests(10);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            Quest quest = QuestHelper.CreateCompositeQuestFromAbove(2, 3);
            int expectedStoredQuests = QuestHelper.CountExpectedSubQuests(2, 3) + 1 + 10;

            //Act
            repository.Insert(quest);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);
        }

        [Test]
        public void UpdateTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateQuests(10);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            Quest updatedItem = QuestHelper.CreateQuest(fakeStorage.QuestStorage[3].Id);
            updatedItem.Title = "New title";

            repository.Update(updatedItem);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);
            Assert.IsNotNull(storedItems.Find(x => (x.Title == "New title") && (x.Id == 4)));
        }

        [Test]
        public void UpdateNullTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => repository.Update(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quest");
        }

        [Test]
        public void UpdateAllTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateQuests(10);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            List<Quest> toUpdate = new List<Quest>
            {
                QuestHelper.CreateQuest(fakeStorage.QuestStorage[3].Id),
                QuestHelper.CreateQuest(fakeStorage.QuestStorage[4].Id)
            };
            toUpdate[0].Title = "New title1";
            toUpdate[1].Title = "New title2";

            repository.UpdateAll(toUpdate);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);
            Assert.IsNotNull(storedItems.Find(x => (x.Title == "New title1") && (x.Id == 4)));
            Assert.IsNotNull(storedItems.Find(x => (x.Title == "New title2") && (x.Id == 5)));
        }

        [Test]
        public void UpdateAllNullTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => repository.UpdateAll(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quests");
        }

        [Test]
        public void UpdateTestWhichHasNewChildButNothingChanges()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateQuests(10);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            Quest updatedItem = fakeStorage.QuestStorage[3];
            Quest addedItem = QuestHelper.CreateQuest();
            addedItem.Title = "Added item";
            updatedItem.Children.Add(addedItem);

            repository.Update(updatedItem);

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);

            Quest added = storedItems.Find(x => (x.Title == "Added item"));

            Assert.IsNull(added);
        }

        [Test]
        public void RevertUpdateTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateQuests(10);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            Quest updatedItem = QuestHelper.CreateQuest(fakeStorage.QuestStorage[3].Id);
            updatedItem.Title = "New title";

            repository.Update(updatedItem);
            bool revertResult = repository.RevertUpdate(updatedItem);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsTrue(revertResult);
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);
            Assert.IsNull(storedItems.Find(x => (x.Title == "New title") && (x.Id == 4)));
        }

        [Test]
        public void SimpleUpdateTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateQuests(10);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            Quest updatedItem = fakeStorage.QuestStorage[3];
            updatedItem.Title = "Updated item";
            int updatedId = updatedItem.Id;

            repository.Update(updatedItem);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.AreEqual(10, storedItems.Count);

            Quest found = storedItems.Find(x => (x.Title == "Updated item"));

            Assert.IsNotNull(found);
            Assert.AreEqual(updatedId, found.Id);
            Assert.AreEqual("Updated item", found.Title);
        }

        [Test]
        public void GetMethodReturnBindedQuestTest()
        {
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateCompositeQuestFromBelow(1, 5, 1);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.PullQuests();
            Quest found = repository.Get(1);

            //Assert
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Id);
            Assert.AreEqual(0, found.ParentId);

            Assert.IsNotNull(found.Children);
            Assert.IsNull(found.Parent);

            Assert.AreEqual(5, found.Children.Count);
            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(found, found.Children[i].Parent);
            }
        }

        [Test]
        public void GetDoesNotFindElementTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateCompositeQuestFromBelow(1, 5, 1);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            Quest found = repository.Get(100);

            //Assert
            Assert.IsNull(found);
        }

        [Test]
        public void GetAllMethodReturnsAllBindedQuestsTest()
        {
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = new List<Quest>();
            for (int i = 0; i < 4; ++i)
            {
                int id = i * 6 + 1;
                fakeStorage.QuestStorage.AddRange(QuestHelper.CreateCompositeQuestFromBelow(1, 5, id));
            }

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.PullQuests();
            List<Quest> founds = repository.GetAll();

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
        }

        [Test]
        public void GetAllInRowUsesCachedLinesTest()
        {
            FakeQuestStorage fakeStorage = MockRepository.GeneratePartialMock<FakeQuestStorage>();
            fakeStorage.Stub(x => x.GetAll()).Repeat.Once();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.PullQuests();
            List<Quest> founds = repository.GetAll();
            repository.GetAll();

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
                fakeStorage.QuestStorage.AddRange(QuestHelper.CreateCompositeQuestFromBelow(1, 5, id));
            }

            int count = QuestHelper.CountSubQuests(fakeStorage.QuestStorage);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.Delete(fakeStorage.QuestStorage.Find(x => x.Id == 7));
            repository.PushQuests();

            List<Quest> fromLower = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(fromLower);
            Assert.AreEqual(count - 1, fromLower.Count);

            Assert.IsTrue(fromLower.Any(x => x.ParentId == 7));
            Assert.IsFalse(fromLower.Any(x => x.Id == 7));
        }

        [Test]
        public void DeleteQuestWithParentTest()
        {
            //Arrange
            IDataAccessInterface<Quest> innerStorage = MockRepository.GenerateStrictMock<IDataAccessInterface<Quest>>();
            innerStorage.Expect(st => st.Delete(Arg<int>.Is.Anything));
            innerStorage.Expect(st => st.Open(Arg<string>.Is.NotNull));
            innerStorage.Expect(st => st.Dispose());

            Quest parent = QuestHelper.CreateCompositeQuest(1, 1, QuestState.Progress);
            Quest toDelete = parent.Children.FirstOrDefault();
            toDelete.Id = 666;

            IQuestRepository repository = new RecursiveQuestRepository(innerStorage, "someConnectionString");

            //Act
            repository.Delete(toDelete);
            repository.PushQuests();

            //Assert
            int deletedId = (int)innerStorage.GetArgumentsForCallsMadeOn(rep => rep.Delete(Arg<int>.Is.Anything))[0][0];

            Assert.AreEqual(666, deletedId);
            Assert.IsFalse(parent.Children.Contains(toDelete));

            innerStorage.VerifyAllExpectations();
        }

        [Test]
        public void DeleteTestWithoutParent()
        {
            //Arrange
            List<Quest> allQuests = QuestHelper.CreateQuests(3);
            Quest toDelete = allQuests[0];
            toDelete.Id = 666;

            IDataAccessInterface<Quest> innerStorage = MockRepository.GenerateStrictMock<IDataAccessInterface<Quest>>();
            innerStorage.Expect(st => st.Delete(Arg<int>.Is.Anything)).Repeat.Once();
            innerStorage.Expect(st => st.Open(Arg<string>.Is.NotNull)).Repeat.Twice();
            innerStorage.Expect(st => st.Dispose()).Repeat.Twice();
            innerStorage.Expect(st => st.GetAll()).Repeat.Once().Return(allQuests);

            IQuestRepository repository = new RecursiveQuestRepository(innerStorage, "someConnectionString");

            //Act
            repository.PullQuests();
            repository.Delete(toDelete);
            repository.PushQuests();

            //Assert
            List<Quest> all = repository.GetAll();

            int deletedId = (int)innerStorage.GetArgumentsForCallsMadeOn(rep => rep.Delete(Arg<int>.Is.Anything))[0][0];

            Assert.IsFalse(all.Contains(toDelete));
            Assert.AreEqual(666, deletedId);

            innerStorage.VerifyAllExpectations();
        }

        [Test]
        public void DeleteNullTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => repository.Delete(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quest");
        }

        [Test]
        public void DeleteAllTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = QuestHelper.CreateCompositeQuestFromBelow(1, 5);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.DeleteAll();
            repository.PushQuests();

            List<Quest> fromLower = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(fromLower);
            Assert.IsEmpty(fromLower);
        }

        [Test]
        public void RevertDeleteTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();
            fakeStorage.QuestStorage = new List<Quest>();
            for (int i = 0; i < 4; ++i)
            {
                int id = i * 6 + 1;
                fakeStorage.QuestStorage.AddRange(QuestHelper.CreateCompositeQuestFromBelow(1, 5, id));
            }

            int count = QuestHelper.CountSubQuests(fakeStorage.QuestStorage);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.Delete(fakeStorage.QuestStorage.Find(x => x.Id == 7));
            repository.RevertDelete(fakeStorage.QuestStorage.Find(x => x.Id == 7));
            repository.PushQuests();

            List<Quest> fromLower = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(fromLower);
            Assert.AreEqual(count, fromLower.Count);

            Assert.IsTrue(fromLower.Any(x => x.ParentId == 7));
            Assert.IsTrue(fromLower.Any(x => x.Id == 7));
        }

        [Test]
        public void InsertRemovePushQuestsQuestTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            Quest quest = QuestHelper.CreateCompositeQuestFromAbove(1, 0);
            quest.Id = 666;

            //Act
            repository.Insert(quest);
            repository.Delete(quest);
            repository.PushQuests();

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsEmpty(storedItems);
        }

        [Test]
        public void PullQuestsTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = MockRepository.GeneratePartialMock<FakeQuestStorage>();
            fakeStorage.Expect(s => s.GetAll()).Repeat.Once();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.PullQuests();
            repository.GetAll();

            //Assert
            fakeStorage.VerifyAllExpectations();
        }

        [Test]
        public void PullQuestDataStorageReturnsNullTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = MockRepository.GeneratePartialMock<FakeQuestStorage>();
            fakeStorage.Expect(s => s.GetAll()).Repeat.Once().Return(null);

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            //Act
            repository.PullQuests();
            List<Quest> all = repository.GetAll();

            //Assert
            Assert.IsNotNull(all);
            Assert.IsEmpty(all);

            fakeStorage.VerifyAllExpectations();
        }

        [Test]
        public void PushQuestsNotClearAllQuestsFromMemoryTest()
        {
            //Arrange
            FakeQuestStorage fakeStorage = new FakeQuestStorage();

            IQuestRepository repository = new RecursiveQuestRepository(fakeStorage, "no matter");

            int questsCount = 5;

            List<Quest> quests = new List<Quest>();

            for (int i = 0; i < questsCount; ++i)
            {
                quests.Add(QuestHelper.CreateCompositeQuestFromAbove(2, 3));
            }
            int expectedStoredQuests = (QuestHelper.CountExpectedSubQuests(2, 3) + 1) * questsCount;

            //Act
            repository.InsertAll(quests);
            int countBeforePushQuests = repository.GetAll().Count;
            repository.PushQuests();
            int countAfterPushQuests = repository.GetAll().Count;

            List<Quest> storedItems = fakeStorage.QuestStorage;

            //Assert
            Assert.IsNotNull(storedItems);
            Assert.IsNotEmpty(storedItems);
            Assert.AreEqual(expectedStoredQuests, storedItems.Count);

     
            Assert.AreEqual(5, countBeforePushQuests);
            Assert.AreEqual(5, countAfterPushQuests);
        }

        [Test]
        public void DisposeCallPushQuestsTest()
        {
            //Arrange
            IDataAccessInterface<Quest> storage = MockRepository.GenerateStrictMock<IDataAccessInterface<Quest>>();
            storage.Expect(s => s.Open(Arg<string>.Is.NotNull)).Repeat.Once();
            storage.Expect(s => s.InsertAll(Arg<List<Quest>>.Is.NotNull)).Repeat.Once();
            storage.Expect(s => s.Dispose()).Repeat.Once();


            List<Quest> quests = QuestHelper.CreateQuests(2);

            IQuestRepository repository = new RecursiveQuestRepository(storage, "no matter");
            
            //Act
            repository.InsertAll(quests);
            repository.Dispose();

            //Assert
            storage.VerifyAllExpectations();
        }
    }
}
