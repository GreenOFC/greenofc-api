using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    public class GetShinhanDetailResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
        public ShinhanPersonalDto Personal { get; set; }
        public ShinhanWorkingDto Working { get; set; }
        public IEnumerable<ShinhanReferenceDto> Referees { get; set; }
        public ShinhanLoanDto Loan { get; set; }
        public ShinhanAddressDto ResidentAddress { get; set; }
        public ShinhanAddressDto TemporaryAddress { get; set; }
        public ShinhanSaleDto SaleInfo { get; set; }
        public ShinhanResultDto Result { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public string ProductLine { get; set; }
        public IEnumerable<ShinhanGroupDocumentDto> Documents { get; set; }
        public IEnumerable<ShinhanGroupDocumentDto> ReturnDocuments { get; set; }
        public string CaseNote { get; set; }
        public ShinhanUploadedMediaDto RecordFile { get; set; }
        public string UserName { get; set; }
        public string Creator { get; set; }
    }
}
