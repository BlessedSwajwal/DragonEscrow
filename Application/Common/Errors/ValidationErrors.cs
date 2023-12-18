using FluentValidation.Results;

namespace Application.Common.Errors;

public class ValidationErrors
{
    private List<string> Errors;
    private readonly ValidationResult _validationResult;

    public ValidationErrors(ValidationResult validationResult)
    {
        _validationResult = validationResult;
        Errors = _validationResult.Errors.Select(x => x.ErrorMessage).ToList();
    }

    public string GetValidationErrors()
    {
        string errorString = "";
        foreach (var error in Errors)
        {
            errorString += $"{error} \n";
        }
        return errorString;
    }
}
