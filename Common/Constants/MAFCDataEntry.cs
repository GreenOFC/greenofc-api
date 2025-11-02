namespace _24hplusdotnetcore.Common.Constants
{
    public struct MAFCDataEntry
    {
        public const string Channel = "24H";
        public const string UserId = "EXT_24H";
        public const string InputQDE = "inputQDE";
        public const string ProcQDEChangeState = "procQDEChangeState";
        public const string InputDDE = "inputDDE";
        public const string ProcDDEChangeState = "procDDEChangeState";
        public static readonly string[] ATProduct = {
            "737",
            "739",
            "1074",
            "1075",
            "1106",
            "1107",
            "1126",
            "1127",
            "1128",
            "1129",
            "1130",
            "1131",
            "1132",
            "1133",
            "1138",
            "1139",
            "1140",
            "1141",
            "1273",
            "1274",
            "1275",
            "1276",
        };
    }
    public struct MAFCCheckDup
    {
        public const string Auto = "MAFC_AUTO_CHECK_DUP";
        public const string ScreenCheckDup = "MAFC_CHECK_DUP";
        public const string PreCreate = "MAFC_PRE_CREATE";
    }
}
