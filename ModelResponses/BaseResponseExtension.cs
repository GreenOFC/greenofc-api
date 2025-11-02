using FluentValidation.Results;

namespace _24hplusdotnetcore.ModelResponses
{
    public static class BaseResponseExtension
    {
        public static void MappingFluentValidation<Tdata>(this BaseResponse<Tdata> baseResponse, ValidationResult result)
        {
            if (result?.Errors == null)
            {
                return;
            }

            foreach (var error in result.Errors)
            {
                var validationError = new ValidationError
                {
                    ErrorCode = error.ErrorCode,
                    ErrorMessage = error.ErrorMessage
                };

                baseResponse.ValidationErrors.Add(validationError);
            }
        }
    }
}
