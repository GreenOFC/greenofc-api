using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Mappings
{
    public class ImportFileMappingProfile : Profile
    {
        public ImportFileMappingProfile()
        {
            CreateMap<ImportFile, ImportFileDto>().ReverseMap();
            CreateMap<ImportFile, ImportFileResponse>().ReverseMap();
        }
    }
}
