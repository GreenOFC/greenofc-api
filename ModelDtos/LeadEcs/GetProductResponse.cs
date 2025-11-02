using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class GetProductResponse
    {
        public string EmployeeType { get; set; }
        public string EmployeeDescription { get; set; }
        public IEnumerable<ProductDocumentDto> DocumentCollectings { get; set; }
    }

    public class ProductDocumentDto
    {
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal LoanMinAmount { get; set; }
        public decimal LoanMaxAmount { get; set; }
        public decimal LoanMinTenor { get; set; }
        public decimal LoanMaxTenor { get; set; }
    }
}
