﻿using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Model.Progress
{
    /// <summary>
    /// Counts progress of recursive quests.
    /// </summary>
    public class RecursiveQuestProgressCounter : IQuestProgressCounter
    {
        /// <summary>
        /// Reference to list with leaf weights, which used to count LCM.
        /// </summary>
        private readonly HashSet<int> _leafWeights;

        /// <summary>
        /// Weights of done leafs.
        /// </summary>
        private readonly List<int> _doneLeafsWeights;

        /// <summary>
        /// Default constructor. Initialize inner leasWeights list.
        /// </summary>
        public RecursiveQuestProgressCounter()
        {
            _leafWeights = new HashSet<int>();
            _doneLeafsWeights = new List<int>();
        }

        #region IQuestProgressCounter implementation

        ///<inheritdoc/>
        public ProgressValue CountProgress(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));

            //1. Clear previous data about quest.
            _leafWeights.Clear();
            _doneLeafsWeights.Clear();

            //2. Count leaf weights and done leaf weights.
            FindLeafGenerationsWeightAndCountDoneWeights(quest,quest.Children, 1);

            //3. Find total weight, using LCM, and current weight.
            ProgressValue result;
            result.Total = LeastCommonMultipleOfNValues(_leafWeights);
            result.Current = CountActualWeight(_doneLeafsWeights, result.Total);

            return result;
        }

        #endregion

        #region Private methods

        private int CountActualWeight(List<int> doneWeights, int leastCommonMultiple)
        {
            int result = 0;
            int length = doneWeights.Count;
            for (int i = 0; i < length; ++i)
            {
                result += leastCommonMultiple/doneWeights[i];
            }
            return result;
        }

        /// <summary>
        /// Count all leaf weights and weights of leas done quests.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="children"></param>
        /// <param name="weight"></param>
        private void FindLeafGenerationsWeightAndCountDoneWeights(Quest parent, List<Quest> children, int weight)
        {
            int length = children.Count;

            if (length == 0)
            {           
                _leafWeights.Add(weight);
                if (parent.State == State.Done)
                {
                    _doneLeafsWeights.Add(weight);
                }
            }
            for (int i = 0; i < length; ++i)
            {
                FindLeafGenerationsWeightAndCountDoneWeights(children[i],children[i].Children, weight * length);
            }
        }

        /// <summary>
        /// Finds LCM of more than two values!
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private int LeastCommonMultipleOfNValues(IEnumerable<int> values)
        {
            int previous = 0;
            using (IEnumerator<int> enumerator = values.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return previous;
                }
                previous = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    previous = LeastCommonMultiple(previous, enumerator.Current);
                }
            }
            return previous;
        }

        /// <summary>
        /// Finds LCM.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int LeastCommonMultiple(int a, int b)
        {
            return a * b / GreatesCommonDevider(a, b);
        }

        /// <summary>
        /// Finds GCD.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int GreatesCommonDevider(int a, int b)
        {
            while (true)
            {
                if (b == 0) return a;
                var a1 = a;
                a = b;
                b = a1%b;
            }
        }

        #endregion
    }
}
