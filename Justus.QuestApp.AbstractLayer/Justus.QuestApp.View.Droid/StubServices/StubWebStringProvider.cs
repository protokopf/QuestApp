using Justus.QuestApp.AbstractLayer.Data;

namespace Justus.QuestApp.View.Droid.StubServices
{
    class StubWebStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return "http://localhost:37993/api/Quests";
        }
    }
}