using System;
using Refit;

namespace _24hplusdotnetcore.ModelDtos
{
    public class CustomerResponseModel
    {
        [AliasAs("id")]
        public string Id { get; set; }
        [AliasAs("contractCode")]
        public string ContractCode { get; set; }
        [AliasAs("userName")]
        public string UserName { get; set; }
        [AliasAs("status")]
        public string Status { get; set; }
        [AliasAs("greenType")]
        public string GreenType { get; set; }
        [AliasAs("productLine")]
        public string ProductLine { get; set; }
        [AliasAs("createdDate")]
        public DateTime CreatedDate { get; set; }
        [AliasAs("modifiedDate")]
        public DateTime ModifiedDate { get; set; }
        [AliasAs("personal")]
        public PersonalResponseModel Personal { get; set; }
        [AliasAs("loan")]
        public LoanResponseModel Loan { get; set; }
        [AliasAs("result")]
        public ResultResponseModel Result { get; set; }
    }

    public class PersonalResponseModel
    {
        [AliasAs("name")]
        public string Name { get; set; }
        [AliasAs("idCard")]
        public string IdCard { get; set; }
        [AliasAs("phone")]
        public string Phone { get; set; }
    }

    public class LoanResponseModel
    {
        [AliasAs("product")]
        public string Product { get; set; }
    }
    public class ResultResponseModel
    {
        [AliasAs("status")]
        public string Status { get; set; }
        [AliasAs("returnStatus")]
        public string ReturnStatus { get; set; }
    }
}
