using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.Adapters.Dialogs;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace Justus.QuestApp.View.Droid.Fragments.Dialogs
{
    /// <summary>
    /// Dialog fragment for displaying validation results.
    /// </summary>
    public class ValidationErrorsFragment : DialogFragment
    {
        private const string ClarifiedErrorsKey = "ClarifiedErrorsKey";

        private readonly IEntityStateHandler<IList<ClarifiedError<int>>> _clarifiedErrorsStateHandler;

        public ValidationErrorsFragment()
        {
            _clarifiedErrorsStateHandler = ServiceLocator.Resolve<IEntityStateHandler<IList<ClarifiedError<int>>>>();
        }

        public ValidationErrorsFragment(IList<ClarifiedError<int>> clarifiedErros) : base()
        {
            if (clarifiedErros == null)
            {
                throw new ArgumentNullException(nameof(clarifiedErros));
            }
            Bundle arguments = new Bundle();
            _clarifiedErrorsStateHandler = ServiceLocator.Resolve<IEntityStateHandler<IList<ClarifiedError<int>>>>();
            _clarifiedErrorsStateHandler.Save(ClarifiedErrorsKey, clarifiedErros, arguments);
            this.Arguments = arguments;
        }

        public static ValidationErrorsFragment NewInstance(IList<ClarifiedError<int>> clarifiedErros)
        {
            return new ValidationErrorsFragment(clarifiedErros);
        }

        #region DialogFragment overriding

        ///<inehrtidoc/>
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            RecyclerView recycler = LayoutInflater.From(Activity).Inflate(Resource.Layout.ValidationErrorsFragmentLayout, null) as RecyclerView;

            Context currentContext = this.Context;

            if (recycler != null)
            {
                IList<ClarifiedError<int>> errors = new List<ClarifiedError<int>>();

                if (Arguments != null)
                {
                    _clarifiedErrorsStateHandler.Extract(ClarifiedErrorsKey, Arguments, ref errors);
                }

                recycler.SetLayoutManager(new LinearLayoutManager(currentContext));
                recycler.SetAdapter(new ValidationDialogFragmentAdapter(currentContext, errors));
            }

            return new AlertDialog.Builder(currentContext).
                SetTitle(Resource.String.ValidationDialogTitle).
                SetPositiveButton(Android.Resource.String.Ok, ValidationOkHandler).
                SetView(recycler).
                Show();
        }

        #endregion

        private void ValidationOkHandler(object sender, DialogClickEventArgs e)
        {
            
        }
    }
}