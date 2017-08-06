using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.View.Droid.ViewHolders.Error;

namespace Justus.QuestApp.View.Droid.Adapters.Dialogs
{
    /// <summary>
    /// Adapter for validation results.
    /// </summary>
    public class ValidationDialogFragmentAdapter : RecyclerView.Adapter
    {
        private readonly IList<ClarifiedError<int>> _errors;
        private readonly Context _context;

        /// <summary>
        /// Receives collection of clarified errors.
        /// </summary>
        /// <param name="validationDialogFragmentAdapterContext"></param>
        /// <param name="clarifiedErrors"></param>
        public ValidationDialogFragmentAdapter(Context validationDialogFragmentAdapterContext, IList<ClarifiedError<int>> clarifiedErrors)
        {
            if (clarifiedErrors == null)
            {
                throw new ArgumentNullException(nameof(clarifiedErrors));
            }
            if (validationDialogFragmentAdapterContext == null)
            {
                throw new ArgumentNullException(nameof(validationDialogFragmentAdapterContext));
            }
            _errors = clarifiedErrors;
            _context = validationDialogFragmentAdapterContext;
        }

        #region RecyclerView.Adapter overriding

        ///<inehritdoc/>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DefaultValidationErrorViewHolder myHolder = holder as DefaultValidationErrorViewHolder;
            if (myHolder != null)
            {
                myHolder.ErrorTextView.Text = _context.GetString(_errors[position].Error);
                myHolder.ClarificationTextView.Text = _context.GetString(_errors[position].Clarification);
            }
        }

        ///<inehritdoc/>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Android.Views.View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ValidationItemLayout, parent,false);
            return new DefaultValidationErrorViewHolder(view);
        }

        ///<inehritdoc/>
        public override int ItemCount => _errors.Count;

        #endregion
    }
}