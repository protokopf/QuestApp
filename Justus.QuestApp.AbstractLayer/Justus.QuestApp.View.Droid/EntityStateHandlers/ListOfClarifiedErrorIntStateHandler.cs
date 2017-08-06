using System.Collections.Generic;
using Android.OS;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;

namespace Justus.QuestApp.View.Droid.EntityStateHandlers
{
    /// <summary>
    /// State handler for clarified error with int message.
    /// </summary>
    public class ListOfClarifiedErrorIntStateHandler : IEntityStateHandler<IList<ClarifiedError<int>>>
    {
        private const string ErrorsKey = "ErrorsArray";
        private const string ClarificationsLey = "ClarificationsArray";

        #region IEntityStateHandler implementation

        ///<inehritdoc/>
        public bool Save(string key, IList<ClarifiedError<int>> entity, Bundle bundle)
        {
            if (string.IsNullOrWhiteSpace(key) || entity == null || bundle == null)
            {
                return false;
            }
            bundle.PutBundle(key, InnerCreateBundle(key, entity));
            return true;
        }

        ///<inehritdoc/>
        public bool Extract(string key, Bundle bundle, ref IList<ClarifiedError<int>> entity)
        {
            if (string.IsNullOrWhiteSpace(key) || bundle == null || entity == null)
            {
                return false;
            }
            Bundle innerBundle = bundle.GetBundle(key);
            if (innerBundle == null)
            {
                return false;
            }
            return InnerExtractValues(innerBundle, entity);
        }

        #endregion

        private bool InnerExtractValues(Bundle bundle, IList<ClarifiedError<int>> entity)
        {
            int[] errors = bundle.GetIntArray(ErrorsKey);
            int[] clarifications = bundle.GetIntArray(ClarificationsLey);
            return MergeArrays(errors, clarifications, entity);
            
        }

        private bool MergeArrays(int[] errors, int[] clarifications, IList<ClarifiedError<int>> entity)
        {
            if (errors == null || clarifications == null)
            {
                return false;
            }

            int errorsLength = errors.Length;

            if (errorsLength != clarifications.Length)
            {
                return false;
            }

            for (int i = 0; i < errorsLength; ++i)
            {
                entity.Add(new ClarifiedError<int>()
                {
                    Error = errors[i],
                    Clarification = clarifications[i]
                });
            }

            return true;
        }

        private Bundle InnerCreateBundle(string key, IList<ClarifiedError<int>> entity)
        {
            Bundle bundle = new Bundle();
            bundle.PutIntArray(ErrorsKey, GetErrorsArray(entity));
            bundle.PutIntArray(ClarificationsLey, GetClarificationsArray(entity));
            return bundle;
        }

        private int[] GetErrorsArray(IList<ClarifiedError<int>> entity)
        {
            int length = entity.Count;
            int[] errors = new int[length];
            for (int i = 0; i < length; ++i)
            {
                errors[i] = entity[i].Error;
            }
            return errors;
        }

        private int[] GetClarificationsArray(IList<ClarifiedError<int>> entity)
        {
            int length = entity.Count;
            int[] clarifications = new int[length];
            for (int i = 0; i < length; ++i)
            {
                clarifications[i] = entity[i].Clarification;
            }
            return clarifications;
        }
    }
}