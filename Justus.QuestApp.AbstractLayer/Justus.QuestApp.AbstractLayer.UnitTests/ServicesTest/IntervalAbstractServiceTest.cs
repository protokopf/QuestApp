using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Justus.QuestApp.AbstractLayer.Services;
using Rhino.Mocks;

namespace Justus.QuestApp.AbstractLayer.UnitTests.ServicesTest
{
    [TestFixture]
    class IntervalAbstractServiceTest
    {
        [Test]
        void StartStopServiceTest()
        {
            //Arrange
            IService service = MockRepository.GeneratePartialMock<IntervalAbstractService>();
            service.Expect()

            //Act

            //Assert
        }
    }
}
