using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Tree;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers.Logic;

namespace Justus.QuestApp.ModelLayer.Commands.Factories
{
    /// <summary>
    /// Update quests and make changes in questTree.
    /// </summary>
    public class DefaultStateCommandsFactory : IStateCommandsFactory
    {
        private readonly IQuestTree _questTree;

        /// <summary>
        /// Receives dependency to quest questTree.
        /// </summary>
        /// <param name="questTree"></param>
        public DefaultStateCommandsFactory(IQuestTree questTree)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
        }

        #region IStateCommandsFactory implementation

        ///<inheritdoc/>
        public ICommand DoneQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new UpToRootQuestCommand(
                quest, _questTree,
                new CompositeQuestCommand(new IQuestCommand[]
                {
                    new IfEachChildMatchQuestCommand(
                        q => q.State == State.Done,
                        new ChangeStateQuestCommand(State.Done)
                    ),
                    new CompositeQuestCommand(new IQuestCommand[]
                    {
                        new RecountProgressQuestCommand(),
                        new UpdateQuestCommand(_questTree)
                    })
                })
            );

        }

        ///<inheritdoc/>
        public ICommand FailQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new UpToRootQuestCommand(quest, _questTree,
                new CompositeQuestCommand(new IQuestCommand[]
                {
                    new IfEachChildMatchQuestCommand(
                        q => q.State == State.Failed,
                        new ChangeStateQuestCommand(State.Failed)
                    ),
                    new CompositeQuestCommand(new IQuestCommand[]
                    {
                        new RecountProgressQuestCommand(),
                        new UpdateQuestCommand(_questTree)
                    })
                })
             );
        }

        ///<inheritdoc/>
        public ICommand StartQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new UpToRootQuestCommand(quest, _questTree, 
                new CompositeQuestCommand(new IQuestCommand[]
                {
                    new ChangeStateQuestCommand(State.Progress),
                    //new RecountProgressQuestCommand(),
                    new UpdateQuestCommand(_questTree) 
                })
            );
        }

        ///<inheritdoc/>
        public ICommand CancelQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new DownHierarchyQuestCommand(quest, 
                beforeTraverseCommand: new CompositeQuestCommand(new IQuestCommand[]
                {
                    //Load
                    new LoadChildrenQuestCommand(_questTree), 
                    //ChangeState
                    new ChangeStateQuestCommand(State.Idle), 
                    //RecountProgress
                    new SetProgressToZeroQuestCommand(), 
                    //Update
                    new UpdateQuestCommand(_questTree)
                }), 
                afterTraverseCommand: new CompositeQuestCommand(new IQuestCommand[]
                {
                    //Unload
                    new UnloadChildrenQuestCommand(_questTree)
                }));
        } 


        #endregion
    }
}
