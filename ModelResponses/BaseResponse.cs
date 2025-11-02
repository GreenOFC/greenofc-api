using System;
using System.Collections.Generic;


namespace _24hplusdotnetcore.ModelResponses
{
    public class BaseResponse<Tdata>
    {
        public BaseResponse()
        {
        }

        public BaseResponse(string errorMsg)
        {
            ErrorMsg = errorMsg;
        }

        public bool IsFailure
        {
            get
            {
                if (ValidationErrors.Count > 0 || !string.IsNullOrEmpty(ErrorMsg)) return true;
                return false;
            }
        }

        public bool IsSuccess
        {
            get
            {
                if (ValidationErrors.Count > 0 || !string.IsNullOrEmpty(ErrorMsg)) return false;
                return true;
            }
        }

        public BaseResponse<Tdata> ReturnWithMessage(string errorMsg)
        {
            ErrorMsg = errorMsg;
            return this;
        }

        public BaseResponse<Tdata> MarkAsInvalidRequest()
        {
            ErrorMsg = "Invalid reequest information";
            return this;
        }

        public void AddValidationErrorMsg(string errorCode, string errorMsg)
        {
            var validationError = new ValidationError
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMsg
            };

            ValidationErrors.Add(validationError);
        }

        public Exception Exception { get; set; }

        public string ErrorMsg { get; set; }
        public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
        public Tdata Data { get; set; }
    }
}
