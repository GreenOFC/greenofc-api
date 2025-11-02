using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class LeadPtfCategoryGroupDto 
    {
        public string Id { get; set; }
        public string ProductLine { get; set; }
        public IEnumerable<LeadPtfCategoryDto> Categories { get; set; }
    }

    public class LeadPtfCategoryDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<LeadPtfProductDto> Products { get; set; }
    }

    public class LeadPtfProductDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
