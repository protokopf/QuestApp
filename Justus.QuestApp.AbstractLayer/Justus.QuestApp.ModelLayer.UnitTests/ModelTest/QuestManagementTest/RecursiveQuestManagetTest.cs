using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ModelLayer.Model.QuestManagement;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest
{
    [TestFixture]
    class RecursiveQuestManagetTest
    {
        #region Help methods

        

        #endregion

        [Test]
        public void StartWithNullQuestTest()
        {
            //Arrange
            IQuestManager manager = new RecursiveQuestManager();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => manager.Start(null));


            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

    }
}
