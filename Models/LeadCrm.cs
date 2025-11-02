using _24hplusdotnetcore.Common.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.RegularExpressions;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    public class LeadCrm
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string LeadCrmId { get; set; }
        public string Potentialname { get; set; }
        public string PotentialNo { get; set; }
        public string Amount { get; set; }
        public LeadCrmUser RelatedTo { get; set; }
        public string Closingdate { get; set; }
        public string OpportunityType { get; set; }
        public string Nextstep { get; set; }
        public string Leadsource { get; set; }
        public string SalesStage { get; set; }
        public LeadCrmUser AssignedUserId { get; set; }
        public string Probability { get; set; }
        public LeadCrmUser Campaignid { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Createdtime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Modifiedtime { get; set; }
        public LeadCrmUser Modifiedby { get; set; }
        public string Description { get; set; }
        public string ForecastAmount { get; set; }
        public string Isconvertedfromlead { get; set; }
        public LeadCrmUser ContactId { get; set; }
        public string Source { get; set; }
        public string Starred { get; set; }
        public string Tags { get; set; }
        public string Cf854 { get; set; }
        public string Cf884 { get; set; }
        public string Cf892 { get; set; }
        public string Cf948 { get; set; }
        public string Cf962 { get; set; }
        public string Cf964 { get; set; }
        public string Cf966 { get; set; }
        public string Cf968 { get; set; }
        public string Cf970 { get; set; }
        public string Cf972 { get; set; }
        public string Cf976 { get; set; }
        public string Cf978 { get; set; }
        public string Cf982 { get; set; }
        public string Cf984 { get; set; }
        public string Cf986 { get; set; }
        public string Cf988 { get; set; }
        public string Cf990 { get; set; }
        public DateTime? Cf992 { get; set; }
        public DateTime? Cf994 { get; set; }
        public string Cf996 { get; set; }
        public string Cf998 { get; set; }
        public string Cf1002 { get; set; }
        public string Cf1008 { get; set; }
        public string Cf1020 { get; set; }
        public string Cf1026 { get; set; }
        public string Cf1028 { get; set; }
        public string Cf1030 { get; set; }
        public string Cf1032 { get; set; }
        public string Cf1036 { get; set; }
        public string Cf1040 { get; set; }
        public string Cf1042 { get; set; }
        public string Cf1044 { get; set; }
        public string Cf1046 { get; set; }
        public string Cf1048 { get; set; }
        public string Cf1050 { get; set; }
        public string Cf1052 { get; set; }
        public string Cf1054 { get; set; }
        public string Cf1068 { get; set; }
        public string Cf1170 { get; set; }
        public string Cf1174 { get; set; }
        public string Cf1176 { get; set; }
        public string Cf1178 { get; set; }
        public string Cf1184 { get; set; }
        public string Cf1188 { get; set; }
        public string Cf1190 { get; set; }
        public string Cf1192 { get; set; }
        public string Cf1194 { get; set; }
        public string Cf1196 { get; set; }
        public string Cf1198 { get; set; }
        public string Cf1200 { get; set; }
        public string Cf1202 { get; set; }
        public string Cf1204 { get; set; }
        public string Cf1206 { get; set; }
        public string Cf1208 { get; set; }
        public string Cf1210 { get; set; }
        public string Cf1214 { get; set; }
        public string Cf1216 { get; set; }
        public string Cf1220 { get; set; }
        public string Cf1222 { get; set; }
        public string Cf1224 { get; set; }
        public string Cf1226 { get; set; }
        public string Cf1230 { get; set; }
        public string Cf1232 { get; set; }
        public string Cf1234 { get; set; }
        public string Cf1238 { get; set; }
        public string Cf1242 { get; set; }
        public string Cf1244 { get; set; }
        public string Cf1246 { get; set; }
        public string Cf1254 { get; set; }
        public string Cf1256 { get; set; }
        public string Cf1258 { get; set; }
        public string Cf1260 { get; set; }
        public string Cf1262 { get; set; }
        public string Cf1264 { get; set; }
        public string Cf1266 { get; set; }
        public string Cf1268 { get; set; }
        public string Cf1270 { get; set; }
        public string Cf1272 { get; set; }
        public string Cf1274 { get; set; }
        public string Cf1276 { get; set; }
        public string Cf1278 { get; set; }
        public string Cf1282 { get; set; }
        public string Cf1284 { get; set; }
        public string Cf1286 { get; set; }
        public string Cf1294 { get; set; }
        public string Cf1296 { get; set; }
        public string Cf1298 { get; set; }
        public string Cf1300 { get; set; }
        public string Cf1302 { get; set; }
        public string Cf1304 { get; set; }
        public string Cf1308 { get; set; }
        public string Cf1310 { get; set; }
        public string Cf1326 { get; set; }
        public string Cf1328 { get; set; }
        public string Cf1330 { get; set; }
        public string Cf1332 { get; set; }
        public string Cf1334 { get; set; }
        public string Cf1336 { get; set; }
        public string Cf1338 { get; set; }
        public string Cf1340 { get; set; }
        public string Cf1350 { get; set; }
        public string Cf1392 { get; set; }
        public string Cf1404 { get; set; }
        public string Cf1408 { get; set; }
        public string Cf1410 { get; set; }
        public string Cf1412 { get; set; }
        public string Cf1414 { get; set; }
        public string Cf1416 { get; set; }
        public string Cf1418 { get; set; }
        public string Cf1420 { get; set; }
        public string Cf1422 { get; set; }
        public string Cf1424 { get; set; }
        public string Cf1426 { get; set; }
        public string Cf1428 { get; set; }
        public string Cf1430 { get; set; }
        public string Cf1432 { get; set; }
        public string Cf1434 { get; set; }
        public string Cf1436 { get; set; }
        public string Cf1492 { get; set; }
        public string Cf1496 { get; set; }
        public PostbackMA PostbackMA { get; set; }

        public string GetStatusMessage()
        {
            return PostbackMA == null ? Cf1190 : GetStatusMessage(PostbackMA.Status);
        }

        public string GetStatusMessage(short status)
        {
            string key = string.Format(StatusMapping.STATUS_PREFIX, status);
            if (StatusMapping.CRM_STATUS_MESSAGE_MAPPING.TryGetValue(key, out string message))
            {
                return message;
            }
            return StatusMapping.CRM_STATUS_MESSAGE_MAPPING[StatusMapping.DEFAULT];
        }

        public void SetDCNote(string errorMessage)
        {
            Cf1174 = errorMessage;
        }
        public string GetSalesStaffName()
        {
            if(!string.IsNullOrEmpty(Cf1414))
            {
                return Cf1414;
            }

            var match = Regex.Match(AssignedUserId?.Label ?? string.Empty, "^([\\d\\w-]*) (.*)$");
            if (match.Success)
            {
                return match.Groups[2].Value;
            }

            return string.Empty;
        }
    }

    public class LeadCrmUser
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }

    public class PostbackMA
    {
        public string TransactionId { get; set; }
        public string DateOfLead { get; set; }
        public string DcCode { get; set; }
        public string DcName { get; set; }
        public string PlaceOfUpload { get; set; }
        public string DocumentCollected { get; set; }
        public string LastCastStatus { get; set; }
        public string DcLastNote { get; set; }
        public string AppSchedule { get; set; }
        public string LastCall { get; set; }
        public short Status { get; set; }
        public short DetailStatus { get; set; }
    }
}
