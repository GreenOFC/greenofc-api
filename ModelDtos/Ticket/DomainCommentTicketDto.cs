using System.Collections.Generic;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.Ticket;

namespace _24hplusdotnetcore.Models.Ticket
{
    public class CreateCommentTicketModelDto : CommentTicketModelDto
    {

    }
    public class UpdateCommentTicketModelDto : CommentTicketModelDto
    {
        public string Id { get; set; }
    }
}