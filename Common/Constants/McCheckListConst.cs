using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public class McCheckListConst
    {
        public static IReadOnlyList<GroupDocument> Documents = new GroupDocument[]
        {
            new GroupDocument
            {
                GroupId = 22,
                GroupName = "CMND/CCCD/CMQĐ",
                Mandatory = true,
                HasAlternate = false,
                Documents = new List<DocumentUpload>
                {
                    new DocumentUpload
                    {
                        Id = 4,
                        DocumentCode = "CivicIdentity",
                        DocumentName = "CCCD",
                        InputDocUid = "559124662598964b868a210069922848",
                        MapBpmVar = "DOC_CivicIdentity",
                    },
                    new DocumentUpload
                    {
                        Id = 6,
                        DocumentCode = "IdentityCard",
                        DocumentName = "CMND",
                        InputDocUid = "943054199583cdc9c54f601005419993",
                        MapBpmVar = "DOC_IdentityCard",
                    },
                    new DocumentUpload
                    {
                        Id = 7,
                        DocumentCode = "MilitaryIdentity",
                        DocumentName = "CMQĐ",
                        InputDocUid = "263825031598964c98ee968050949384",
                        MapBpmVar = "DOC_MilitaryIdentity",
                    }
                }
            },
            new GroupDocument
            {
                GroupId = 26,
                GroupName = "Hình ảnh khách hàng",
                Mandatory = true,
                HasAlternate = false,
                Documents = new List<DocumentUpload>
                {
                    new DocumentUpload
                    {
                        Id = 23,
                        DocumentCode = "FacePhoto",
                        DocumentName = "Hình ảnh khách hàng",
                        InputDocUid = "592712402583cdc9c53fc06001162945",
                        MapBpmVar = "DOC_FacePhoto",
                    },
                }
            }
        };
    }
}
