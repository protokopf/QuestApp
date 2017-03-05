using System;
using Justus.QuestApp.AbstractLayer.Commands;

namespace Justus.QuestApp.View.Droid.StubServices
{
    public class StubCommandManager : ICommandManager
    {
        public void Add(Command command)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Do()
        {
            throw new NotImplementedException();
        }
    }
}