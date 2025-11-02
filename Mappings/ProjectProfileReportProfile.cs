using _24hplusdotnetcore.ModelDtos.ProjectProfileReports;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class ProjectProfileReportProfile : Profile
    {
        public ProjectProfileReportProfile()
        {
            CreateMap<ProjectProfileReportImportFileData, ProjectProfileReportDetail>();
            CreateMap<ProjectProfileReportDetail, ProjectProfileReportFileExport>()
                .ForMember(dest => dest.SaleCode, opt => opt.MapFrom(src => src.SaleInfomation.UserName))
                .ForMember(dest => dest.SaleName, opt => opt.MapFrom(src => src.SaleInfomation.FullName))
                .ForMember(dest => dest.TeamLeadName, opt => opt.MapFrom(src => src.TeamLeadInfo.FullName))
                .ForMember(dest => dest.PosName, opt => opt.MapFrom(src => src.PosInfo.Name))
                .ForMember(dest => dest.SaleChanelName, opt => opt.MapFrom(src => src.SaleChanelInfo.Name));

        }
    }
}
