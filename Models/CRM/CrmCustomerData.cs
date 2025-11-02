using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Models.CRM
{
    public class CrmCustomerData
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public CrmResult Result { get; set; }
    }

    public class CrmResult
    {
        [JsonProperty("records")]
        public Record[] Records { get; set; }

        [JsonProperty("nextPage")]
        public string NextPage { get; set; }
    }

    public class CRMBaseModel
    {
        [JsonProperty("potentialname")]
        public string Potentialname { get; set; }

        [JsonProperty("potential_no")]
        public string PotentialNo { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("related_to")]
        public AssignedUserId RelatedTo { get; set; }

        /// <summary>
        /// Ngày tạo hs GOF 
        /// </summary>
        [JsonProperty("closingdate")]
        public string Closingdate { get; set; }

        /// <summary>
        /// Ngày cập nhật hs GOF 
        /// </summary>
        [JsonProperty("cf_1108")]
        public string Cf1108 { get; set; }

        [JsonProperty("opportunity_type")]
        public string OpportunityType { get; set; }

        [JsonProperty("nextstep")]
        public string Nextstep { get; set; }

        [JsonProperty("leadsource")]
        public string Leadsource { get; set; }

        [JsonProperty("sales_stage")]
        public string SalesStage { get; set; }



        [JsonProperty("probability")]
        public string Probability { get; set; }

        [JsonProperty("campaignid")]
        public AssignedUserId Campaignid { get; set; }

        [JsonProperty("createdtime")]
        public DateTime? Createdtime { get; set; }

        [JsonProperty("modifiedtime")]
        public DateTime? Modifiedtime { get; set; }

        [JsonProperty("modifiedby")]
        public AssignedUserId Modifiedby { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("forecast_amount")]
        public string ForecastAmount { get; set; }

        [JsonProperty("isconvertedfromlead")]
        public string Isconvertedfromlead { get; set; }

        [JsonProperty("contact_id")]
        public AssignedUserId ContactId { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("starred")]
        public string Starred { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("cf_854")]
        public string Cf854 { get; set; }

        [JsonProperty("cf_884")]
        public string Cf884 { get; set; }

        [JsonProperty("cf_892")]
        public string Cf892 { get; set; } = "-";

        [JsonProperty("cf_948")]
        public string Cf948 { get; set; }

        [JsonProperty("cf_962")]
        public string Cf962 { get; set; }

        [JsonProperty("cf_964")]
        public string Cf964 { get; set; }

        [JsonProperty("cf_966")]
        public string Cf966 { get; set; }

        [JsonProperty("cf_968")]
        public string Cf968 { get; set; }

        [JsonProperty("cf_970")]
        public string Cf970 { get; set; }

        [JsonProperty("cf_972")]
        public string Cf972 { get; set; }

        [JsonProperty("cf_976")]
        public string Cf976 { get; set; }

        [JsonProperty("cf_978")]
        public string Cf978 { get; set; }

        [JsonProperty("cf_982")]
        public string Cf982 { get; set; }

        [JsonProperty("cf_984")]
        public string Cf984 { get; set; }

        [JsonProperty("cf_986")]
        public string Cf986 { get; set; }

        [JsonProperty("cf_988")]
        public string Cf988 { get; set; }

        [JsonProperty("cf_990")]
        public string Cf990 { get; set; }

        [JsonProperty("cf_992")]
        public DateTime? Cf992 { get; set; }

        [JsonProperty("cf_994")]
        public DateTime? Cf994 { get; set; }

        [JsonProperty("cf_996")]
        public string Cf996 { get; set; }

        [JsonProperty("cf_998")]
        public string Cf998 { get; set; }

        [JsonProperty("cf_1002")]
        public string Cf1002 { get; set; } = "-";

        [JsonProperty("cf_1008")]
        public string Cf1008 { get; set; }

        [JsonProperty("cf_1020")]
        public string Cf1020 { get; set; }

        [JsonProperty("cf_1026")]
        public string Cf1026 { get; set; }

        [JsonProperty("cf_1028")]
        public string Cf1028 { get; set; }

        /// <summary>
        /// Tình trạng hôn nhân
        /// </summary>
        [JsonProperty("cf_1030")]
        public string Cf1030 { get; set; }

        [JsonProperty("cf_1032")]
        public string Cf1032 { get; set; }

        [JsonProperty("cf_1036")]
        public string Cf1036 { get; set; }

        [JsonProperty("cf_1040")]
        public string Cf1040 { get; set; }

        [JsonProperty("cf_1042")]
        public string Cf1042 { get; set; }

        [JsonProperty("cf_1044")]
        public string Cf1044 { get; set; }

        [JsonProperty("cf_1046")]
        public string Cf1046 { get; set; }

        [JsonProperty("cf_1048")]
        public string Cf1048 { get; set; }

        [JsonProperty("cf_1050")]
        public string Cf1050 { get; set; }

        [JsonProperty("cf_1052")]
        public string Cf1052 { get; set; } = "-";

        [JsonProperty("cf_1054")]
        public string Cf1054 { get; set; }

        [JsonProperty("cf_1068")]
        public string Cf1068 { get; set; }

        [JsonProperty("cf_1170")]
        public string Cf1170 { get; set; }

        [JsonProperty("cf_1174")]
        public string Cf1174 { get; set; }

        [JsonProperty("cf_1176")]
        public string Cf1176 { get; set; }

        [JsonProperty("cf_1178")]
        public string Cf1178 { get; set; }

        [JsonProperty("cf_1184")]
        public string Cf1184 { get; set; }

        [JsonProperty("cf_1188")]
        public string Cf1188 { get; set; } = "-";

        [JsonProperty("cf_1190")]
        public string Cf1190 { get; set; }

        [JsonProperty("cf_1192")]
        public string Cf1192 { get; set; }

        [JsonProperty("cf_1194")]
        public string Cf1194 { get; set; }

        [JsonProperty("cf_1196")]
        public string Cf1196 { get; set; }

        [JsonProperty("cf_1198")]
        public string Cf1198 { get; set; }

        [JsonProperty("cf_1200")]
        public string Cf1200 { get; set; }

        [JsonProperty("cf_1202")]
        public string Cf1202 { get; set; }

        [JsonProperty("cf_1204")]
        public string Cf1204 { get; set; }

        [JsonProperty("cf_1206")]
        public string Cf1206 { get; set; }

        [JsonProperty("cf_1208")]
        public string Cf1208 { get; set; }

        [JsonProperty("cf_1210")]
        public string Cf1210 { get; set; } = "-";

        [JsonProperty("cf_1214")]
        public string Cf1214 { get; set; }

        [JsonProperty("cf_1216")]
        public string Cf1216 { get; set; }

        [JsonProperty("cf_1220")]
        public string Cf1220 { get; set; }

        [JsonProperty("cf_1222")]
        public string Cf1222 { get; set; }

        [JsonProperty("cf_1224")]
        public string Cf1224 { get; set; }

        [JsonProperty("cf_1226")]
        public string Cf1226 { get; set; }

        [JsonProperty("cf_1230")]
        public string Cf1230 { get; set; }

        [JsonProperty("cf_1232")]
        public string Cf1232 { get; set; }

        [JsonProperty("cf_1234")]
        public string Cf1234 { get; set; }

        [JsonProperty("cf_1238")]
        public string Cf1238 { get; set; }

        [JsonProperty("cf_1242")]
        public string Cf1242 { get; set; }

        [JsonProperty("cf_1244")]
        public string Cf1244 { get; set; }

        [JsonProperty("cf_1246")]
        public string Cf1246 { get; set; }

        [JsonProperty("cf_1254")]
        public string Cf1254 { get; set; }

        [JsonProperty("cf_1256")]
        public string Cf1256 { get; set; } = "-";

        [JsonProperty("cf_1258")]
        public string Cf1258 { get; set; }

        [JsonProperty("cf_1260")]
        public string Cf1260 { get; set; }

        [JsonProperty("cf_1262")]
        public string Cf1262 { get; set; }

        [JsonProperty("cf_1264")]
        public string Cf1264 { get; set; } = "????";

        [JsonProperty("cf_1266")]
        public string Cf1266 { get; set; }

        [JsonProperty("cf_1268")]
        public string Cf1268 { get; set; }

        [JsonProperty("cf_1270")]
        public string Cf1270 { get; set; }

        [JsonProperty("cf_1272")]
        public string Cf1272 { get; set; }

        [JsonProperty("cf_1274")]
        public string Cf1274 { get; set; }

        [JsonProperty("cf_1276")]
        public string Cf1276 { get; set; }

        [JsonProperty("cf_1278")]
        public string Cf1278 { get; set; }

        [JsonProperty("cf_1282")]
        public string Cf1282 { get; set; }

        [JsonProperty("cf_1284")]
        public string Cf1284 { get; set; }

        [JsonProperty("cf_1286")]
        public string Cf1286 { get; set; }

        [JsonProperty("cf_1294")]
        public string Cf1294 { get; set; }

        [JsonProperty("cf_1296")]
        public string Cf1296 { get; set; }

        [JsonProperty("cf_1298")]
        public string Cf1298 { get; set; }

        [JsonProperty("cf_1300")]
        public string Cf1300 { get; set; }

        [JsonProperty("cf_1302")]
        public string Cf1302 { get; set; }

        [JsonProperty("cf_1304")]
        public string Cf1304 { get; set; }

        [JsonProperty("cf_1308")]
        public string Cf1308 { get; set; }

        [JsonProperty("cf_1310")]
        public string Cf1310 { get; set; }

        [JsonProperty("cf_1326")]
        public string Cf1326 { get; set; }

        [JsonProperty("cf_1328")]
        public string Cf1328 { get; set; }

        [JsonProperty("cf_1330")]
        public string Cf1330 { get; set; }

        [JsonProperty("cf_1332")]
        public string Cf1332 { get; set; }

        [JsonProperty("cf_1334")]
        public string Cf1334 { get; set; }

        [JsonProperty("cf_1336")]
        public string Cf1336 { get; set; }

        [JsonProperty("cf_1338")]
        public string Cf1338 { get; set; }

        [JsonProperty("cf_1340")]
        public string Cf1340 { get; set; }

        [JsonProperty("cf_1350")]
        public string Cf1350 { get; set; }
        [JsonProperty("cf_1404")]
        public string Cf1404 { get; set; } = "-";
        [JsonProperty("cf_1408")]
        public string Cf1408 { get; set; }
        [JsonProperty("cf_1410")]
        public string Cf1410 { get; set; }
        [JsonProperty("cf_1412")]
        public string Cf1412 { get; set; }
        [JsonProperty("cf_1414")]
        public string Cf1414 { get; set; }
        [JsonProperty("cf_1416")]
        public string Cf1416 { get; set; }
        [JsonProperty("cf_1418")]
        public string Cf1418 { get; set; }
        [JsonProperty("cf_1420")]
        public string Cf1420 { get; set; }
        [JsonProperty("cf_1422")]
        public string Cf1422 { get; set; }
        [JsonProperty("cf_1424")]
        public string Cf1424 { get; set; }
        [JsonProperty("cf_1426")]
        public string Cf1426 { get; set; }
        [JsonProperty("cf_1428")]
        public string Cf1428 { get; set; }
        [JsonProperty("cf_1430")]
        public string Cf1430 { get; set; }
        [JsonProperty("cf_1432")]
        public string Cf1432 { get; set; }
        [JsonProperty("cf_1434")]
        public string Cf1434 { get; set; } = "Khách hàng mới";
        [JsonProperty("cf_1436")]
        public string Cf1436 { get; set; } = "?????";
        [JsonProperty("cf_1440")]
        public string Cf1440 { get; set; }
        [JsonProperty("cf_1442")]
        public string Cf1442 { get; set; }
        [JsonProperty("cf_1446")]
        public string Cf1446 { get; set; }
        [JsonProperty("cf_1494")]
        public string Cf1494 { get; set; }
        //Hồ sơ có Admin xử lý hay không?
        [JsonProperty("cf_1498")]
        public string Cf1498 { get; set; }
        // Hồ sơ được duyệt bởi code luồng
        [JsonProperty("cf_1638")]
        public string Cf1638 { get; set; }

        [JsonProperty("cf_1502")]
        public string Cf1502 { get; set; }

        /// <summary>
        /// Mã hợp đồng 
        /// </summary>
        [JsonProperty("cf_1504")]
        public string Cf1504 { get; set; }
        [JsonProperty("cf_1506")]
        public string Cf1506 { get; set; }
        [JsonProperty("cf_1512")]
        public string Cf1512 { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cf_1448")]
        public string Cf1448 { get; set; }

        [JsonProperty("cf_1450")]
        public string Cf1450 { get; set; }

        [JsonProperty("cf_1452")]
        public string Cf1452 { get; set; }

        [JsonProperty("cf_1454")]
        public string Cf1454 { get; set; }

        [JsonProperty("cf_1456")]
        public string Cf1456 { get; set; }

        [JsonProperty("cf_1458")]
        public string Cf1458 { get; set; }

        [JsonProperty("cf_1460")]
        public string Cf1460 { get; set; }

        [JsonProperty("cf_1462")]
        public string Cf1462 { get; set; }

        [JsonProperty("cf_1464")]
        public string Cf1464 { get; set; }

        [JsonProperty("cf_1466")]
        public string Cf1466 { get; set; }

        [JsonProperty("cf_1468")]
        public string Cf1468 { get; set; }

        [JsonProperty("cf_1470")]
        public string Cf1470 { get; set; }

        [JsonProperty("cf_1472")]
        public string Cf1472 { get; set; }

        [JsonProperty("cf_1474")]
        public string Cf1474 { get; set; }

        [JsonProperty("cf_1476")]
        public string Cf1476 { get; set; }

        [JsonProperty("cf_1478")]
        public string Cf1478 { get; set; }

        [JsonProperty("cf_1480")]
        public string Cf1480 { get; set; }

        [JsonProperty("cf_1482")]
        public string Cf1482 { get; set; }

        [JsonProperty("cf_1484")]
        public string Cf1484 { get; set; }

        [JsonProperty("cf_1486")]
        public string Cf1486 { get; set; }

        [JsonProperty("cf_1488")]
        public string Cf1488 { get; set; }

        [JsonProperty("cf_1490")]
        public string Cf1490 { get; set; }

        /// <summary>
        /// Đơn vị bán hàng	
        /// </summary>
        [JsonProperty("cf_1514")]
        public string Cf1514 { get; set; }

        /// <summary>
        /// ASM
        /// </summary>
        [JsonProperty("cf_1516")]
        public string Cf1516 { get; set; }

        /// <summary>
        /// SUB
        /// </summary>
        [JsonProperty("cf_1518")]
        public string Cf1518 { get; set; }

        /// <summary>
        /// Thu nhập qua lương	
        /// </summary>
        [JsonProperty("cf_1522")]
        public string Cf1522 { get; set; }

        /// <summary>
        /// Công nợ hiện tại
        /// </summary>
        [JsonProperty("cf_1524")]
        public string Cf1524 { get; set; }

        /// <summary>
        /// SĐT phụ
        /// </summary>
        [JsonProperty("cf_1526")]
        public string Cf1526 { get; set; }

        /// <summary>
        /// Ngày cấp CMND/CCCD cũ
        /// </summary>
        [JsonProperty("cf_1532")]
        public string Cf1532 { get; set; }

        /// <summary>
        /// Nơi Cấp CMND/CCCD cũ
        /// </summary>
        [JsonProperty("cf_1534")]
        public string Cf1534 { get; set; }

        /// <summary>
        /// Thời gian có thể nghe máy
        /// </summary>
        [JsonProperty("cf_1538")]
        public string Cf1538 { get; set; }

        /// <summary>
        /// Tài khoản MXH
        /// </summary>
        [JsonProperty("cf_1540")]
        public string Cf1540 { get; set; }

        /// <summary>
        /// Tài khoản MXH chi tiết
        /// </summary>
        [JsonProperty("cf_1542")]
        public string Cf1542 { get; set; }

        /// <summary>
        /// Tỉnh TP tạm trú
        /// </summary>
        [JsonProperty("cf_1560")]
        public string Cf1560 { get; set; }

        /// <summary>
        /// Quận Huyện tạm trú
        /// </summary>
        [JsonProperty("cf_1580")]
        public string Cf1580 { get; set; }

        /// <summary>
        /// Số nhà-tên đường tạm trú
        /// </summary>
        [JsonProperty("cf_1576")]
        public string Cf1576 { get; set; }

        /// <summary>
        /// Số nhà tên đường thường trú
        /// </summary>
        [JsonProperty("cf_1584")]
        public string Cf1584 { get; set; }

        /// <summary>
        /// Quận Huyện thường trú
        /// </summary>
        [JsonProperty("cf_1088")]
        public string Cf1088 { get; set; }

        /// <summary>
        /// Tỉnh TP thường trú
        /// </summary>
        [JsonProperty("cf_1586")]
        public string Cf1586 { get; set; }

        /// <summary>
        /// Người phụ thuộc
        /// </summary>
        [JsonProperty("cf_1092")]
        public string Cf1092 { get; set; }

        /// <summary>
        /// Số lượng TCTD hiện tại
        /// </summary>
        [JsonProperty("cf_1090")]
        public string Cf1090 { get; set; }

        /// <summary>
        /// Số nhà tên đường công ty
        /// </summary>
        [JsonProperty("cf_1006")]
        public string Cf1006 { get; set; }

        /// <summary>
        /// Loại bảo hiểm
        /// </summary>
        [JsonProperty("cf_1510")]
        public string Cf1510 { get; set; }

        /// <summary>
        /// Mã SIP
        /// </summary>
        [JsonProperty("cf_1078")]
        public string Cf1078 { get; set; }

        /// <summary>
        /// Tỉnh TP ngân hàng giải ngân
        /// </summary>
        [JsonProperty("cf_1604")]
        public string Cf1604 { get; set; }

        /// <summary>
        /// Chi nhánh đối tác giải ngân
        /// </summary>
        [JsonProperty("cf_1070")]
        public string Cf1070 { get; set; }

        /// <summary>
        /// Khách hàng đã từng có bảo hiểm
        /// </summary>
        [JsonProperty("cf_1612")]
        public string Cf1612 { get; set; }

        /// <summary>
        /// Đối tác giải ngân
        /// </summary>
        [JsonProperty("cf_1606")]
        public string Cf1606 { get; set; }

        /// <summary>
        /// Phương thức giải ngân
        /// </summary>
        [JsonProperty("cf_1618")]
        public string Cf1618 { get; set; }

        /// <summary>
        /// Số điện thoại nhà
        /// </summary>
        [JsonProperty("cf_1536")]
        public string Cf1536 { get; set; }

        /// <summary>
        /// sản phẩm vay
        /// </summary>
        [JsonProperty("cf_1528")]
        public string Cf1528 { get; set; } = "-";

        /// <summary>
        /// Xã thường trú
        /// </summary>
        [JsonProperty("cf_1018")]
        public string Cf1018 { get; set; }

        /// <summary>
        /// Xã công ty
        /// </summary>
        [JsonProperty("cf_1004")]
        public string Cf1004 { get; set; }

        /// <summary>
        /// Dịch vụ bảo hiểm	
        /// </summary>
        [JsonProperty("cf_1012")]
        public string Cf1012 { get; set; }

        /// <summary>
        /// Lý do trả về 2
        /// </summary>
        [JsonProperty("cf_1024")]
        public string Cf1024 { get; set; }

        /// <summary>
        /// Kênh
        /// </summary>
        [JsonProperty("cf_1630")]
        public string Cf1630 { get; set; }
    }

    public class Record : CRMBaseModel
    {
        [JsonProperty("assigned_user_id")]
        public AssignedUserId AssignedUserId { get; set; }
    }

    public class AssignedUserId
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class CRMRequestDto : CRMBaseModel
    {
        [JsonProperty("assigned_user_id")]
        public string AssignedUserId { get; set; }
    }

    public class CRMError
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class UpSertCrmResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("result")]
        public UpSertCrmResult Result { get; set; }
        [JsonProperty("error")]
        public CRMError Error { get; set; }
    }

    public class UpSertCrmResult
    {
        [JsonProperty("record")]
        public Record Record { get; set; }
    }
}
