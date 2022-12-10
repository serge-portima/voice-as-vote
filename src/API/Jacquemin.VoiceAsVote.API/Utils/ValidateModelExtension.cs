using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Jacquemin.VoiceAsVote.API.Utils;

public static class ValidateModelExtension
{ 
    public static void ValidateModel<T>(this T model) where T : class
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        if (isValid)
        {
            return;
        }

        var message = string.Join(", ", validationResults.Select(result => result.ToString()).ToArray());

        throw new ArgumentOutOfRangeException(nameof(model), message);
    }
}