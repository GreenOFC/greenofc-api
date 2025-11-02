using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.ProjectProfileReports
{
    public class ProjectProfileReportImportResponse
    {
        public bool IsSuccess { get; set; }

        public string ProjectProfileReportId { get; set; }
        
        public IEnumerable<ProjectProfileErrorDto> Errors { get; set; }
        public string ErrorMessage { get; set; }
    }
}
