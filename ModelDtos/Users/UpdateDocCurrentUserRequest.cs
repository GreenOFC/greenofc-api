using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UpdateDocCurrentUserRequest
    {
        public IEnumerable<GroupDocumentDto> Documents { get; set; }

        public bool IsSubmit { get; set; }
    }
}
