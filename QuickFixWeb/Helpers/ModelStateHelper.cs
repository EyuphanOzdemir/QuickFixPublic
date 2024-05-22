// ModelStateHelper.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using static QuickFixWeb.Consts.Consts;

namespace QuickFixWeb.Helpers
{
    public static class ModelStateHelper
    {
        public static bool HandleModelStateErrors(ModelStateDictionary modelState, Microsoft.AspNetCore.Mvc.RazorPages.PageModel pageModel)
        {
            if (!modelState.IsValid)
            {
                var modelErrors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var allErrors = string.Join(Environment.NewLine, modelErrors);
                pageModel.ViewData["Message"] = $"{Error_Indicator}Model Errors: {allErrors}";
                return true;
            }
            return false;
        }
    }
}