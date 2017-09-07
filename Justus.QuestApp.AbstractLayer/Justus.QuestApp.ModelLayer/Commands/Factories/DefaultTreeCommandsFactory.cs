using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Tree;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers.Logic;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using UpToRootQuestCommand = Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers.Hierarchy.UpToRootQuestCommand;

namespace Justus.QuestApp.ModelLayer.Commands.Factories
{
    /// <summary>
    /// Default repository commands factory.
    /// </summary>
    public class DefaultTreeCommandsFactory : ITreeCommandsFactory
    {
        private readonly IQuestTree _repository;

        /// <summary>
        /// Receives dependency to IQuestTree.
        /// </summary>
        /// <param name="repository"></param>
        public DefaultTreeCommandsFactory(IQuestTree repository)
        {
            repository.ThrowIfNull(nameof(repository));
            _repository = repository;
        }

        #region ITreeCommandsFactory implementation

        ///<inheritdoc/>
        public ICommand AddQuest(Quest parent, Quest quest)
        {
            return new CompositeCommand(
                new AddQuestCommand(_repository, parent, quest),
                new CurrentQuestCommand(parent, new CompositeQuestCommand(             
                    new IsLeafAdjustQuestCommand(),
                    new UpdateQuestCommand(_repository)
                ))
             );
        }

        ///<inheritdoc/>
        public ICommand DeleteQuest(Quest parent, Quest quest)
        {
            return new CompositeCommand(
                new DeleteQuestCommand(_repository, parent, quest), 
                new CurrentQuestCommand(parent, 
                    new CompositeQuestCommand(
                        new IfMatchQuestCommand(p => p.State == State.Progress && p.Children.Count != 0, 
                            new IfElseQuestCommand(
                                new IfEachChildMatchQuestCommand(q => q.State == State.Done,
                                    new ChangeStateQuestCommand(State.Done)
                                ),
                                new IfEachChildMatchQuestCommand(q => q.State == State.Failed,
                                    new ChangeStateQuestCommand(State.Failed)
                                ) 
                            )
                        ), 
                        new IsLeafAdjustQuestCommand(), 
                        new UpToRootQuestCommand(_repository, 
                            new CompositeQuestCommand(
                                new RecountProgressQuestCommand(),
                                new UpdateQuestCommand(_repository)))
                    )
                )
            );
        }

        ///<inheritdoc/> 
        public ICommand UpdateQuest(Quest quest)
        {
            return new CurrentQuestCommand(
                quest, 
                new UpdateQuestCommand(_repository));
        }
         
        #endregion
    }
}
