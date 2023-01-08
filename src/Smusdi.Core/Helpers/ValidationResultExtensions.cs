using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Smusdi.Core.Helpers;

public static class ValidationResultExtensions
{
    public static ModelStateDictionary ToModelState(this ValidationResult result)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in result.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return modelStateDictionary;
    }
}
