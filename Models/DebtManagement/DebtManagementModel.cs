using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models.DebtManagement
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.DebtManagement)]
    public class DebtManagementModel : BaseEntity
    {
        public string ContractCode { get; set; }
        public string GreenType { get; set; }
        public DebtPersonal Personal { get; set; }
        public DebtLoan Loan { get; set; }
        public DebtSaleInfo SaleInfo { get; set; }
        public DebtSaleInfo ModifierInfo { get; set; }
        public string ActualUpdatedDate { get; set; }
        public bool IsDuplicated { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public DateTime? OverDueDate { get; set; }
        public int? NumberOverDueDate { get; set; }
        public string Type { get; set; }
        public int RowNumber { get; set; }
    }
}
