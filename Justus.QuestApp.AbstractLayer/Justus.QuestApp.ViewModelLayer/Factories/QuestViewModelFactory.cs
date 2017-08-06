using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;

namespace Justus.QuestApp.ViewModelLayer.Factories
{
    /// <summary>
    /// Default implementation of IQuestViewModelFactory, that uses IQuestFactory to create Model of IQuestViewModel.
    /// </summary>
    public class QuestViewModelFactory : IQuestViewModelFactory
    {
        private readonly IQuestFactory _questFactory;

        /// <summary>
        /// Receives dependency on IQuestFactory.
        /// </summary>
        /// <param name="questFactory"></param>
        public QuestViewModelFactory(IQuestFactory questFactory)
        {
            questFactory.ThrowIfNull(nameof(questFactory));
            _questFactory = questFactory;
        }

        #region IQuestViewModelFactory implementation

        ///<inheritdoc cref="IQuestViewModelFactory"/>
        public IQuestViewModel CreateQuestViewModel()
        {
            return new QuestViewModel
            {
                Model = _questFactory.CreateQuest()
            };
        }

        #endregion
    }
}
