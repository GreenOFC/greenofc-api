using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckInitContractModels;

namespace _24hplusdotnetcore.ModelResponses.MC
{
    public class MCCustomerPaymentVerifyResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        public string CustomerId { get; set; }
        public CustomerBirthYearVerifyResponse BirthYearVerifiCationResponse { get; set; }
        public CheckInitContractResponse ContractVerificationResponse { get; set; }
        public MCResponseDto IDVerificationResponse { get; set; }
    }
}
