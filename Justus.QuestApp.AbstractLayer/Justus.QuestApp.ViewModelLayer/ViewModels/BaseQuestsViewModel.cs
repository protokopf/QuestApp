using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// Base type for view models, that works with quest items.
    /// </summary>
    public class BaseQuestsViewModel : BaseViewModel
    {
        protected IQuestRepository _questRepository;
        protected ICommandManager _commandManager;
        protected ITaskWrapper _taskWrapper;

        /// <summary>
        /// Default constructor. Resolves references to quest repository and command manager.
        /// </summary>
        public BaseQuestsViewModel()
        {
            _questRepository = ServiceLocator.Resolve<IQuestRepository>();
            _commandManager = ServiceLocator.Resolve<ICommandManager>();
            _taskWrapper = ServiceLocator.Resolve<ITaskWrapper>();
        }

        public async void SaveChanges()
        {
            await _taskWrapper.Wrap(() => _questRepository.Save());
        } 

        public async void RefreshQuests()
        {

        }
    }
}
