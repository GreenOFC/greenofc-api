using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class GetMcDetailResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
        public McPersonalDto Personal { get; set; }
        public McWorkingDto Working { get; set; }
        public IEnumerable<McReferenceDto> Referees { get; set; }
        public McLoanDto Loan { get; set; }
        public McAddressDto ResidentAddress { get; set; }
        public McAddressDto TemporaryAddress { get; set; }
        public McSaleDto SaleInfo { get; set; }
        public McResultDto Result { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public string ProductLine { get; set; }
        public IEnumerable<McGroupDocumentDto> Documents { get; set; }
        public IEnumerable<McGroupDocumentDto> ReturnDocuments { get; set; }
        public string CaseNote { get; set; }
        public McUploadedMediaDto RecordFile { get; set; }
        public string UserName { get; set; }
        public string Creator { get; set; }
    }
}
