using System;
using System.Collections.Generic;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.F88
{
    public class GetLeadF88Response
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContractCode { get; set; }
        public string Phone { get; set; }
        public string LoanCategory { get; set; }
        public DataConfigDto LoanCategoryData { get; set; }
        public string IdCard { get; set; }
        public string IdCardProvince { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public DataConfigDto ProvinceData { get; set; }
        public string DateOfBirth { get; set; }
        public string Description { get; set; }
        public string SignAddress { get; set; }
        public DataConfigDto SignAddressData { get; set; }
        public string Status { get; set; }
        public F88PostBackResponse PostBack { get; set; }
        public SaleInfoResponse SaleInfo {  get; set; }
        public PosInfoDto PosInfo { get; set; }
        public TeamLeadDto TeamLeadInfo { get; set; }
        public SaleChanelDto SaleChanelInfo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class F88PostBackResponse
    {
        public string Status { get; set; }
        public string DetailStatus { get; set; }
        public string LoanAmount { get; set; }
    }
}
