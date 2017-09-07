using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Helpers.Behaviours;
using Justus.QuestApp.AbstractLayer.Helpers.Extensions;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// Base type for view models, that works with quest items.
    /// </summary>
    public class QuestListViewModel : BaseViewModel, IRefreshable
    {
        protected readonly IQuestListModel QuestListModel;
        protected readonly IStateCommandsFactory StateCommads;
        protected readonly ITreeCommandsFactory TreeCommands;

        /// <summary>
        /// Default constructor. Resolves references to quest list model and command factories.
        /// </summary>
        public QuestListViewModel(
            IQuestListModel questListModel, 
            IStateCommandsFactory stateCommandsFactory, 
            ITreeCommandsFactory treeCommandsFactory)
        {
            questListModel.ThrowIfNull(nameof(questListModel));
            stateCommandsFactory.ThrowIfNull(nameof(stateCommandsFactory));
            treeCommandsFactory.ThrowIfNull(nameof(treeCommandsFactory));

            QuestListModel = questListModel;
            StateCommads = stateCommandsFactory;
            TreeCommands = treeCommandsFactory;
        }

        #region IRefreshable implementation

        ///<inehritdoc cref="IRefreshable"/>
        public void Refresh()
        {
            QuestListModel.Refresh();
        }

        #endregion

        /// <summary>
        /// Returns leaves of list model.
        /// </summary>
        public List<Quest> Leaves => QuestListModel.Leaves;

        /// <summary>
        /// Traverse to root of current quest hierarchy.
        /// </summary>
        public bool TraverseToRoot()
        {
            return QuestListModel.TraverseToRoot();
        }

        /// <summary>
        /// Traverse to parent of current quest.
        /// </summary>
        public bool TraverseToParent()
        {
            return QuestListModel.TraverseToParent();
        }

        /// <summary>
        /// Traverse to 'childPosition' child of current quest.
        /// </summary>
        /// <param name="leafNumber"></param>
        public bool TraverseToLeaf(int leafNumber)
        {
            return QuestListModel.TraverseToLeaf(leafNumber);
        }

        /// <summary>
        /// Get name of current parent quest.
        /// </summary>
        public string QuestsListTitle => QuestListModel.IsInTheRoot ? null : QuestListModel.Parent?.Title;

        /// <summary>
        /// Points, whether current quest hierarchy in root.
        /// </summary>
        public bool InTopRoot => QuestListModel.IsInTheRoot;

        /// <summary>
        /// Returns id of parent quest.
        /// </summary>
        public int RootId => QuestListModel.Parent.Id;

        /// <summary>
        /// Returns id of leaf.
        /// </summary>
        /// <param name="leafPosition"></param>
        /// <returns></returns>
        public int? GetLeafId(int leafPosition)
        {
            List<Quest> leaves = Leaves;
            if (!leaves.InRange(leafPosition))
            {
                return null;
            }
            return leaves[leafPosition].Id;
        }

        /// <summary>
        /// Starts given quest.
        /// </summary>
        /// <param name="position"></param>
        public Task StartQuest(int position)
        {
            Quest quest = GetQuestByPosition(position);
            return quest == null ? null : RunCommand(StateCommads.StartQuest(quest));
        }

        /// <summary>
        /// Deletes quest.
        /// </summary>
        /// <param name="position"></param>
        public Task DeleteQuest(int position)
        {
            Quest quest = GetQuestByPosition(position);
            if (quest == null)
            {
                return null;
            }
            Quest parent = QuestListModel.Parent;
            return RunCommand(TreeCommands.DeleteQuest(parent, quest));
        }

        //public async Task<int> ToggleImportance(int position)
        //{
        //    List<Quest> leaves = Leaves;
        //    int newPosition = -1;
        //    if (leaves.InRange(position))
        //    {
        //        IsBusy = true;
        //        await Task.Run(() =>
        //        {                   
        //            Quest currentQuest = leaves[position];
        //            bool currentImportance = currentQuest.IsImportant;

        //            //If quests is going to be important - it goes top up, otherwise - bottom down.
        //            //newPosition = currentImportance ? leaves.Count - 1 : 0;
        //            //leaves.Move(position, newPosition);

        //            currentQuest.IsImportant = !currentImportance;

        //            leaves = HandleQuests(leaves);

        //            newPosition = leaves.FindIndex(q => q == currentQuest);

        //            LastCommand = TreeCommands.UpdateQuest(currentQuest);
        //            LastCommand.Execute();
        //            QuestRepository.Action();
        //        });
        //        IsBusy = false;
        //    }
        //    return newPosition;

        //}

        #region Protected methods

        /// <summary>
        /// Returns quest by position. If position is wrong, null is returned.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected Quest GetQuestByPosition(int position)
        {
            List<Quest> leaves = Leaves;
            if (!leaves.InRange(position))
            {
                return null;
            }
            return leaves[position];
        }

        /// <summary>
        /// Execute and commit ICommand implementation within Task.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected Task RunCommand(ICommand command)
        {
            return Task.Run(() =>
            {
                command.Execute();
                command.Commit();
            });
        }

        #endregion
    }
}
